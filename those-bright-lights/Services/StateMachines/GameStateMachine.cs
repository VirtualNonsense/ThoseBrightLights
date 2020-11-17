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
        
        public GameStateMachine(Splashscreen splashscreen, 
                                MainMenu menu,
                                Settings settings,
                                LevelSelect levelSelect)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _subject = new Subject<GameState>();
            _stateMap = new Dictionary<State, GameState>
            {
                {State.SplashScreen, splashscreen},
                {State.Menu, menu},
                {State.Settings, settings},
                {State.LevelSelect, levelSelect}
            };
            _machine = new StateMachine<State, GameStateMachineTrigger>(State.Init);
            _machine.Configure(State.Init).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.InitFinished, State.SplashScreen);
            _machine.Configure(State.SplashScreen).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.SkipSplashScreen, State.Menu);
            _machine.Configure(State.Menu).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.StartLevelSelect, State.LevelSelect)
                .Permit(GameStateMachineTrigger.QuitGame, State.Quit)
                .Permit(GameStateMachineTrigger.StartSettings, State.Settings);
            _machine.Configure(State.Settings).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.Back, State.Menu);
            _machine.Configure(State.LevelSelect).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.Back, State.Menu);
            _machine.Configure(State.Quit).OnEntry(onComplete);
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

        private void onComplete()
        {
            _subject.OnCompleted();
        }


        private enum State
        {
            Init,
            SplashScreen,
            Menu,
            Settings,
            LevelSelect,
            Quit
        }

        public enum GameStateMachineTrigger
        {
            InitFinished,
            SkipSplashScreen,
            StartGame,
            StartSettings,
            StartLevelSelect,
            Back,
            QuitGame
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