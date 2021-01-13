using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using NLog;
using SE_Praktikum.Components.Sprites.Actors.Spaceships;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites.Actors
{
    public abstract class PowerUp : Actor 
    {
        private Logger logger;
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
                case Spaceship s:
                    IsRemoveAble = true;
                    _impactSound?.Play();
                    break;
            }
        }
    }




        
        
        

}

