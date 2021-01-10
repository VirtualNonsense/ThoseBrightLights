using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using NLog;
using SE_Praktikum.Core.GameStates;
using Stateless;
using Stateless.Graph;

namespace SE_Praktikum.Services.StateMachines
{
    public class GameStateMachine : IObservable<GameState>, IObserver<GameStateMachine.GameStateMachineTrigger>
    {
        private readonly ILogger _logger;
        private readonly Subject<GameState> _subject;
        private readonly StateMachine<State, GameStateMachineTrigger> _machine;
        private readonly Dictionary<State, GameState> _stateMap;
        
        public GameStateMachine(Splashscreen splashscreen,
                                SaveSelect saveSelect,
                                MainMenu menu,
                                Settings settings,
                                LevelSelect levelSelect,
                                InGame inGame)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _subject = new Subject<GameState>();
            _stateMap = new Dictionary<State, GameState>
            {
                {State.SplashScreen, splashscreen},
                {State.SaveSlotSelection, saveSelect},
                {State.Menu, menu},
                {State.Settings, settings},
                {State.LevelSelect, levelSelect},
                {State.InGame, inGame}
            };
            _machine = new StateMachine<State, GameStateMachineTrigger>(State.SplashScreen);
            _machine.Configure(State.SplashScreen).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.SkipSplashScreen, State.SaveSlotSelection);


            _machine.Configure(State.SaveSlotSelection).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.SaveSlotSelected, State.Menu);
            
            _machine.Configure(State.Menu).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.StartLevelSelect, State.LevelSelect)
                // .Permit(GameStateMachineTrigger.StartGame, State.InGame)
                .Permit(GameStateMachineTrigger.QuitGame, State.Quit)
                .Permit(GameStateMachineTrigger.StartSettings, State.Settings)
                .Permit(GameStateMachineTrigger.BackToSaveSlotSelection, State.SaveSlotSelection);
            
            _machine.Configure(State.Settings).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.Back, State.Menu);
            
            _machine.Configure(State.LevelSelect).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.Back, State.Menu)
                .Permit(GameStateMachineTrigger.StartGame, State.InGame);
            
            _machine.Configure(State.InGame).OnEntry(onEntry)
                .Permit(GameStateMachineTrigger.SaveAndQuit, State.Quit)
                .Permit(GameStateMachineTrigger.SaveAndBackToMenu, State.LevelSelect);
            
            _machine.Configure(State.Quit).OnEntry(onComplete);

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
            SplashScreen,
            SaveSlotSelection,
            Menu,
            Settings,
            LevelSelect,
            InGame,
            Quit
        }

        public enum GameStateMachineTrigger
        {
            SkipSplashScreen,
            SaveSlotSelected,
            BackToSaveSlotSelection,
            StartGame,
            StartSettings,
            StartLevelSelect,
            SaveAndQuit,
            SaveAndBackToMenu,
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
        public string DotGraph => UmlDotGraph.Format(_machine.GetInfo());
    }
}