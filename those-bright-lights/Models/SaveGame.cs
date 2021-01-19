using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Models
{
    // All desired fields which need to be saved
    public class SaveGame
    {
        public int clearedStage;
        public float damage;
        public int playerPosition;
        public int enemyPosition;
        public int weapon;
        public int score;
        public float musicVolume;
        public uint sessions;
    }
}
