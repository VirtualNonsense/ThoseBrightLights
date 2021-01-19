using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors
{
    public abstract class PowerUp : Actor 
    {
        protected List<PowerUp> Powerups;

        public PowerUp(AnimationHandler animationHandler, float health = 0.01f, SoundEffect soundEffect = null) : base(
            animationHandler, soundEffect)
        {
            Health = health;
            Powerups = new List<PowerUp>();
            
        }
            

        public void Effect(PowerUp Powerups)
        {

        }

        protected override void ExecuteInteraction(Actor other)
        {
            switch(other)
            {
                case Bullet b:
                    _tool = b;
                    _lastAggressor = b.Parent;
                    Health -= b.Damage;
                    break;
                
                case Spaceship s:
                    IsRemoveAble = true;
                    _impactSound?.Play();
                    break;
            }
        }

        protected override LevelEventArgs.ActorDiedEventArgs GetOnDeadEventArgs()
        {
            return new LevelEventArgs.PowerUpDiedEventArgs();
        }
    }




        
        
        

}

