using SE_Praktikum.Components.Sprites.Weapons;
using System;
using System.Collections.Generic;
using System.Text;

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


    }
}
