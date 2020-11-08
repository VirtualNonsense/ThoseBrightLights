using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using SE_Praktikum.Models;
using SE_Praktikum.Services.ParticleEmitter;
using Microsoft.Xna.Framework.Input;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private IScreen _screen;
        private readonly ExplosionEmitter _explosionEmitter;
        public Song _song;
        List<SoundEffect> soundEffects;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter)
        {
            _screen = parent;
            _explosionEmitter = explosionEmitter;
            soundEffects = new List<SoundEffect>();
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            var p = new Animation(contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"), 7);
            _explosionEmitter.Animation = p;
            //_explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);

            // MUSIC
            _song = contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;

            // SOUNDS
            soundEffects.Add(contentManager.Load<SoundEffect>("Audio/Sound_Effects/Savepoint (1)"));
            soundEffects.Add(contentManager.Load<SoundEffect>("Audio/Sound_Effects/Shot2"));

            //// Fire and forget play
            //soundEffects[0].Play();

            //// Play that can be manipulated after the fact
            //var instance = soundEffects[0].CreateInstance();
            //instance.IsLooped = true;
            //instance.Play();
        }

        public override void UnloadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            _explosionEmitter.Update(gameTime);


            // SOUND

            if (Keyboard.GetState().IsKeyDown(Keys.F10))
                soundEffects[0].CreateInstance().Play();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                soundEffects[1].CreateInstance().Play();
        }

        public override void PostUpdate()
        {
            //throw new System.NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            _explosionEmitter.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}