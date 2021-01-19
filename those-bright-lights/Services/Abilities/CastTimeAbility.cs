using System;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Services.Abilities
{
    public class CastTimeAbility : TimeControlledAbility
    {
        private bool _castingInProgress;
        
        // #############################################################################################################
        // Constructor
        // ############################################################################################################# 
        public CastTimeAbility(int targetTime, Action ability) : base(targetTime, ability)
        {
        }
        // #############################################################################################################
        // Properties
        // #############################################################################################################
        public bool CastingInProgress => _castingInProgress;

        // #############################################################################################################
        // public Methods
        // ############################################################################################################# 
        public override void Fire()
        {
            if(_castingInProgress) return;
            _castingInProgress = true;
        }

        public override void Update(GameTime gameTime)
        {
            if(!_castingInProgress) return;
            base.Update(gameTime);
            InvokeCoolDownUpdate();
            if (!TargetTimeReached) return;
            _castingInProgress = false;
            _elapsedTime = 0;
            Ability();
        }
    }
}