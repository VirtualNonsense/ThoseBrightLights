﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SE_Praktikum.Components;
using SE_Praktikum.Components.Sprites.Actors;
using SE_Praktikum.Components.Sprites.Actors.Bullets;
using SE_Praktikum.Core;

namespace SE_Praktikum.Models
{
    public class LevelEventArgs : EventArgs
    {
        public class ShotBulletEventArgs : LevelEventArgs
        {
            public Bullet Bullet;
        }

        public class ExplosionEventArgs : LevelEventArgs
        {
            public Particle Particle;
        }

        public class ActorDiedEventArgs : LevelEventArgs
        {
            /// <summary>
            /// Actor that died
            /// </summary>
            public Actor Victim;
            /// <summary>
            /// Murderer
            /// </summary>
            public Actor Aggressor;
            /// <summary>
            /// Actor that was used to commit the deed
            /// (can be equal to aggressor)
            /// </summary>
            public Actor Tool;

        }
        public class PlayerDiedEventArgs : ActorDiedEventArgs 
        {
        }

        public class TileDiedEventArgs : ActorDiedEventArgs
        {
        }

        public class EnemyDiedEventArgs : ActorDiedEventArgs
        {
        }

        public class BossDiedEventArgs : EnemyDiedEventArgs
        {
        }

        public class WeaponDiedEventArgs : ActorDiedEventArgs
        {
        }

        public class BulletDiedEventArgs : ActorDiedEventArgs
        {
            
        }

        public class PowerUpDiedEventArgs : ActorDiedEventArgs
        {
            
        }

        public class WinningZoneReachedEventArgs : LevelEventArgs
        {
            
        }

        public class InvincibilityChangedEventArgs : LevelEventArgs
        {
            public Actor Target;
        }
    }
}
