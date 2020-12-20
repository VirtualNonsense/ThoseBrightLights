using System;
using Microsoft.Xna.Framework;

namespace SE_Praktikum.Services
{
    public class CooldownAbility
    {
        private int _currentTimer;
        private readonly int _cooldown;

        public CooldownAbility(int cooldown, Action ability)
        {
            _currentTimer = cooldown;
            _cooldown = cooldown;
            _ability = ability;
        }

        private readonly Action _ability;

        public void Update(GameTime gameTime)
        {
            _currentTimer += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public void Fire()
        {
            if (_currentTimer < _cooldown) return;
            _currentTimer = 0;
            _ability();
        }
    }
}