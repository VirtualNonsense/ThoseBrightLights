using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Services
{
    public class SoundHandler<T> where T : Enum
    {
        // Class for determining which soundeffect is used more easily
        private Dictionary<T, SoundEffect> _soundEffect;

        // If there is a Soundeffect then give it back
        public SoundEffect Get(T key)
        {
            if (_soundEffect.ContainsKey(key))
            {
                return _soundEffect[key];
            }
            return null;
        }

        // Constructor
        public SoundHandler()
        {
            _soundEffect = new Dictionary<T, SoundEffect>(); 
        }

        // Soundeffect created map them
        public void Add(T key, SoundEffect soundEffect)
        {
            _soundEffect.Add(key, soundEffect);
        }

        // Play the sound
        public void Play(T key)
        {
            _soundEffect[key].CreateInstance().Play();
        }
    }
}
