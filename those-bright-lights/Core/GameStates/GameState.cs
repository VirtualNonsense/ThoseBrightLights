using System;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SE_Praktikum.Services.StateMachines;

namespace SE_Praktikum.Core.GameStates
{
    public abstract class GameState : IObservable<GameStateMachine.GameStateMachineTrigger>
    {
        protected Subject<GameStateMachine.GameStateMachineTrigger> _subject;
        protected GameState()
        {
            _subject = new Subject<GameStateMachine.GameStateMachineTrigger>();
        }

        public abstract void LoadContent();

        public abstract void UnloadContent();
        
        /// <summary>
        /// Updates every component within state
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// For cleanup;
        /// </summary>
        public abstract void PostUpdate(GameTime gameTime);

        /// <summary>
        /// Draw everything within specific state;
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);


        public IDisposable Subscribe(IObserver<GameStateMachine.GameStateMachineTrigger> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}