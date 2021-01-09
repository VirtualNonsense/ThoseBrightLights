using System;
using System.Collections.Generic;
using System.Text;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Core;

namespace SE_Praktikum.Models
{
    public class LevelEvent : EventArgs
    {
        public class GameQuit : LevelEvent
        {

        }

        public class ShootBullet : LevelEvent
        {
            public Bullet Bullet;
        }

        public class Explosion : LevelEvent
        {
            public Particle Particle;
        }


    }
}
