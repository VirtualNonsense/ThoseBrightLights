using System;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Services
{
    public class CooldownAbility
    {
        private int _currentTimer;
        private int _cooldown;

        public CooldownAbility(int cooldown, Action ability)
        {
            _currentTimer = cooldown;
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
            _currentTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Fire()
        {
            if (_currentTimer < Cooldown) return;
            _currentTimer = 0;
            _ability();
        }
    }
}