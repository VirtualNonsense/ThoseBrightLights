using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Services
{
    public class SoundHandler<T> where T : Enum
    {
        private Dictionary<T, SoundEffect> _soundEffect;
        public SoundHandler()
        {
            _soundEffect = new Dictionary<T, SoundEffect>(); 
        }

        public void Add(T key, SoundEffect soundEffect)
        {
            _soundEffect.Add(key, soundEffect);
        }

        public void Play(T key)
        {
            _soundEffect[key].CreateInstance().Play();
        }
    }
}
