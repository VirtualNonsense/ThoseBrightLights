using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using NLog;
using SE_Praktikum.Core.GameStates;
using Stateless;

namespace SE_Praktikum.Services.StateMachines
{
    public class GameStateMachine : IObservable<GameState>, IObserver<GameStateMachine.GameStateMachineTrigger>
    {
        private readonly ILogger _logger;
        private readonly Subject<GameState> _subject;
        private readonly StateMachine<State, GameStateMachineTrigger> _machine;
        private readonly Dictionary<State, GameState> _stateMap;
        
        public GameStateMachine(Splashscreen splashscreen, MainMenu menu)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _subject = new Subject<GameState>();
            _stateMap = new Dictionary<State, GameState>
            {
                {State.SplashScreen, splashscreen},
                {State.Menu, menu}
            };
            _machine = new StateMachine<State, GameStateMachineTrigger>(State.Init);
            _machine.Configure(State.Init).Permit(GameStateMachineTrigger.InitFinished, State.SplashScreen).OnEntry(onEntry);
            _machine.Configure(State.SplashScreen).Permit(GameStateMachineTrigger.SkipSplashScreen, State.Menu).OnEntry(onEntry);
            _machine.Configure(State.Menu).Permit(GameStateMachineTrigger.QuitGame, State.Quit).OnEntry(onEntry);
            _machine.Fire(GameStateMachineTrigger.InitFinished);
            foreach (var mapEntry in _stateMap)
            {
                mapEntry.Value.Subscribe(this);
            }
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


        private enum State
        {
            Init,
            SplashScreen,
            Menu,
            Quit
        }

        public enum GameStateMachineTrigger
        {
            InitFinished,
            SkipSplashScreen,
            StartGame,
            StartSettings,
            QuitGame,
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(GameStateMachineTrigger value)
        {
            try
            {
                _machine.Fire(value);
            }
            catch (Exception e)
            {
                _logger.Warn(e, $"Unable to Perform Transition From {_machine.State} with {value}");
            }
        }
    }
}