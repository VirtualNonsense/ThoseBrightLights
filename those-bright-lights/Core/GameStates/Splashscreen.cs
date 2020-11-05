using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SE_Praktikum.Models;
using SE_Praktikum.Services.ParticleEmitter;

namespace SE_Praktikum.Core.GameStates
{
    public class Splashscreen : GameState
    {
        private IScreen _screen;
        private readonly ExplosionEmitter _explosionEmitter;
        public Song _song;
        private readonly AnimationHandlerFactory _factory;
        private CollisionCube c1;
        private CollisionCube c2;
        private List<Actor> _actors;
        private Logger _logger;

        public Splashscreen(IScreen parent, ExplosionEmitter explosionEmitter)
        {
            _screen = parent;
            _explosionEmitter = explosionEmitter;
            _factory = factory;
            _logger = LogManager.GetCurrentClassLogger();
        }
        
        public override void LoadContent(ContentManager contentManager)
        {
            var p = new Animation(contentManager.Load<Texture2D>("Artwork/Effects/explosion_45_45"), 7);
            _explosionEmitter.Animation = p;
            //_explosionEmitter.SpawnArea = new Rectangle(500, 100, 500, 100);

            _song = contentManager.Load<Song>("Audio/Music/Song3_remaster2_mp3");
            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;
            
            var animation = new Animation(contentManager.Load<Texture2D>("Artwork/misc/testboxoderso"),1);
            var animationSettings = new AnimationSettings();
            
            c1 = new CollisionCube(_factory.GetAnimationHandler(animation, animationSettings), new Input{Down = Keys.S, Left=Keys.A,Right = Keys.D,Up = Keys.W}, _screen);
            c2 = new CollisionCube(_factory.GetAnimationHandler(animation, animationSettings), new Input{Down = Keys.Down, Left=Keys.Left,Right = Keys.Right,Up = Keys.Up}, _screen);
            _actors = new List<Actor>(){c1,c2};      
            c1.Position = new Vector2(200,200);
        }

        public override void UnloadContent()
        {
            throw new System.NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //_explosionEmitter.Update(gameTime);
            c1.Update(gameTime);
            c2.Update(gameTime);
            PostUpdate(gameTime);
            
        }


        public override void PostUpdate(GameTime gameTime)
        {
            foreach (var spriteA in _actors)
            {
                foreach (var spriteB in _actors)
                {
                    if (spriteA == spriteB)
                        continue;
                    if(spriteA.Intersects(spriteB))
                    {
                        _logger.Trace("collision");
                        spriteB.BaseCollide(spriteA);
                    }
                    else
                    {
                        _logger.Trace("no collision");
                    }
                }
            }
            for (int i = 0; i < _actors.Count; i++)
            {
                if (_actors[i].IsRemoveAble)
                {
                    _logger.Trace("actor removed");
                    _actors.RemoveAt(i);
                    i--;
                }
            }
                
            //throw new System.NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            foreach(var sprite in _actors)
                sprite.Draw(gameTime,spriteBatch);
            spriteBatch.End();
        }
    }
}