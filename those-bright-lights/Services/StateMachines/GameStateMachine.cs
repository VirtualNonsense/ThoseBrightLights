using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using NLog;
using SE_Praktikum.Core.GameStates;
using Stateless;

namespace SE_Praktikum.Services.StateMachines
{
    public class GameStateMachine : IObservable<GameState>
    {
        private readonly ILogger _logger;
        private readonly Subject<GameState> _subject;
        private readonly StateMachine<State, StateTrigger> _machine;
        private readonly Dictionary<State, GameState> _stateMap;
        
        public GameStateMachine(Splashscreen splashscreen)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _subject = new Subject<GameState>();
            _stateMap = new Dictionary<State, GameState>
            {
                 {State.SplashScreen, splashscreen}
            };
            _machine = new StateMachine<State, StateTrigger>(State.Init);
            _machine.Configure(State.Init).Permit(StateTrigger.InitFinished, State.SplashScreen).OnEntry(onEntry);
            _machine.Configure(State.SplashScreen).Permit(StateTrigger.Next, State.Menu).OnEntry(onEntry);
            _machine.Configure(State.Menu).Permit(StateTrigger.Quit, State.Quit).OnEntry(onEntry);
            _machine.Fire(StateTrigger.InitFinished);
        }
        
        public IDisposable Subscribe(IObserver<GameState> observer)
        {
            _logger.Trace("Subscribing");
            var d = _subject.Subscribe(observer);
            try
            {
                observer.OnNext(_stateMap[_machine.State]);
            }
            catch (KeyNotFoundException e)
            {
                _logger.Error(e);
                throw;
            }
            return d;
        }

        private void onEntry()
        {
            _subject.OnNext(_stateMap[_machine.State]);
        }

        
        private void Skip()
        {
            _machine.Fire(StateTrigger.Next);
        }


        private enum State
        {
            Init,
            SplashScreen,
            Menu,
            Quit
        }

        private enum StateTrigger
        {
            InitFinished,
            Next,
            Start,
            GoToSettings,
            Back,
            Quit,
        }
        
    }
}