using System;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Services
{
    public class CooldownAbility
    {
        private int _elapsedTime;
        private int _cooldown;

        public event EventHandler<CooldownProgressArgs> OnCoolDownUpdate;
        public event EventHandler OnCoolAbilityAvailable;

        public CooldownAbility(int cooldown, Action ability)
        {
            _elapsedTime = cooldown;
            _cooldown = cooldown;
            _ability = ability;
        }

        private readonly Action _ability;

        public int Cooldown
        {
            get => _cooldown;
            set
            {
                if (value < 0) return;
                _cooldown = value;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_elapsedTime >= Cooldown) return;
            _elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            var progress = _cooldown / (float) _elapsedTime;
            progress = progress > 1 ? 1 : progress;
            InvokeCoolDownUpdate(progress);
            if(progress >= 1)
                InvokeOnCoolAbilityAvailable();
        }

        public void Fire()
        {
            if (_elapsedTime < Cooldown) return;
            _elapsedTime = 0;
            _ability();
        }

        /// <summary>
        /// Update the subscriber about the cooldown progress
        /// Use values between 0 and 1
        /// </summary>
        /// <param name="progress"></param>
        protected virtual void InvokeCoolDownUpdate(float progress)
        {
            progress = progress > 1 ? 1 : progress;
            var e = new CooldownProgressArgs(progress);
            OnCoolDownUpdate?.Invoke(this, e);
        }
        protected virtual void InvokeOnCoolAbilityAvailable()
        {
            OnCoolAbilityAvailable?.Invoke(this, EventArgs.Empty);
        }
        
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