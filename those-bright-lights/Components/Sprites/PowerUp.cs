using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using NLog;
using SE_Praktikum.Components.Sprites.Weapons;
using SE_Praktikum.Extensions;
using SE_Praktikum.Models;
using SE_Praktikum.Services;

namespace SE_Praktikum.Components.Sprites
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
                    break;
            }
        }
    }




        
        
        

}

