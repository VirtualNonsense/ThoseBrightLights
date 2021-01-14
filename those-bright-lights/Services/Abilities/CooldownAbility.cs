using System;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Services.Abilities
{
    public class CooldownAbility : TimeControlledAbility
    {

        
        // #############################################################################################################
        // Constructor
        // #############################################################################################################
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