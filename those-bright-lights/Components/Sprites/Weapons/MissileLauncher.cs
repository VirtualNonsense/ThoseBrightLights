using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SE_Praktikum.Services;
using SE_Praktikum.Services.Factories;

namespace SE_Praktikum.Components.Sprites.Weapons
{
    public class MissileLauncher : Weapon
    {
        private List<Missile> _missiles;
        //TODO: does the missileLauncher know the direction himself?
        private Vector2 _direction;
        public MissileLauncher() : base()
        {
        }

        public override void Shoot(Vector2 direction)
        {
            _missiles.Add(new Missile(AnimationHandler,direction));
            //add a new missile to the list
        }

        public override void Update(GameTime gameTime, List<Actor> actors)
        {
            int index = 0;
            while(index < _missiles.Count)
            {
                foreach (var actor in actors)
                {
                    Vector2? pos = _missiles[index].Intersects(actor);
                    if (pos != null)
                    {
                        _missiles.RemoveAt(index);
                        OnOnHit();
                    }
                    else
                        index++;

                }
            }
            //TODO: check every object in list against all objects they can collide with -> delete from list and call event in object
        }
    }
}