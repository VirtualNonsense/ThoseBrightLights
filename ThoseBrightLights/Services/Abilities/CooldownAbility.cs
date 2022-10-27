using System;
using Microsoft.Xna.Framework;

namespace ThoseBrightLights.Services.Abilities
{
    public class CooldownAbility : TimeControlledAbility
    {
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
        /// <summary>
        /// When fired the timer starts ticking and the ability action is called as soon as the elapsed time reaches the targettime
        /// </summary>
        /// <param name="cooldown"></param>
        /// <param name="ability"></param>
        public CooldownAbility(int cooldown, Action ability) : base(cooldown, ability)
        {
            _elapsedTime = cooldown;
        }

        
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        
        public bool AbilityAvailable => TargetTimeReached;
        
        // #############################################################################################################
        // Public Methods
        // #############################################################################################################
        public override void Update(GameTime gameTime)
        {
            if(TargetTimeReached) return;
            base.Update(gameTime);
            InvokeCoolDownUpdate();
            if(AbilityAvailable)
                InvokeOnCoolAbilityAvailable();
        }

        public override void Fire()
        {
            if (!AbilityAvailable) return;
            _elapsedTime = 0;
            Ability();
        }

    }
}