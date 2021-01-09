using System;
using System.Data;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Services
{
    public abstract class TimeControlledAbility
    {
        
        /// <summary>
        /// Elapsed time in Milliseconds
        /// </summary>
        protected int _elapsedTime;
        
        /// <summary>
        /// Time that must be reached in order for something to happen
        /// </summary>
        protected int _targetTime;
        
        
        protected readonly Action _ability;

        protected TimeControlledAbility(int targetTime, Action ability)
        {
            _targetTime = targetTime;
            _ability = ability;
        }
        // #############################################################################################################
        // Events
        // #############################################################################################################
        public event EventHandler<CooldownProgressArgs> OnCoolDownUpdate;
        public event EventHandler OnCoolAbilityAvailable;
        
        
        // #############################################################################################################
        // Properties
        // ############################################################################################################# 
        
        public float CoolDownProgress
        {
            get
            {
                var progress = _elapsedTime / (float) _targetTime;
                return progress > 1 ? 1 : progress;
            }
        }
       
        // #############################################################################################################
        // public Methods
        // ############################################################################################################# 
        public bool TargetTimeReached
        {
            get => _elapsedTime >= _targetTime;
        }

        public abstract void Fire();

        public virtual void Update(GameTime gameTime)
        {
            if (TargetTimeReached) return;
            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
        }

        // #############################################################################################################
        // private / protected methods
        // #############################################################################################################
        /// <summary>
        /// Update the subscriber about the cooldown progress
        /// Use values between 0 and 1
        /// </summary>
        protected virtual void InvokeCoolDownUpdate()
        {
            var e = new CooldownProgressArgs(CoolDownProgress);
            OnCoolDownUpdate?.Invoke(this, e);
        }
        protected virtual void InvokeOnCoolAbilityAvailable()
        {
            OnCoolAbilityAvailable?.Invoke(this, EventArgs.Empty);
        }


        // #############################################################################################################
        // Subclasses
        // #############################################################################################################
        public class CooldownProgressArgs : EventArgs
        {
            /// <summary>
            /// Value should be between 0 and 1.
            /// 1 means the cooldown is done and the ability is available.
            /// </summary>
            public float Progress { get; set; }

            public CooldownProgressArgs(float progress)
            {
                Progress = progress;
            }
        }
    }
}