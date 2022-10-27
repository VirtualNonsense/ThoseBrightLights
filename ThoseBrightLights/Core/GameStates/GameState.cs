using System;
using System.Reactive.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThoseBrightLights.Services.StateMachines;

namespace ThoseBrightLights.Core.GameStates
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
        /// For cleanup or postprocessing;
        /// </summary>
        public abstract void PostUpdate(GameTime gameTime);

        /// <summary>
        /// Draw everything within specific state;
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public abstract void Draw();

        /// <summary>
        /// for state machine integration
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable Subscribe(IObserver<GameStateMachine.GameStateMachineTrigger> observer)
        {
            return _subject.Subscribe(observer);
        }
    }
}