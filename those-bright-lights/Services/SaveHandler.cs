using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SE_Praktikum.Services
{
    public class SaveHandler
    {
        public void Save(SaveGame saveGame)
        {
            using (StreamWriter sw = new StreamWriter("Savestate_1.txt"))
            {
                sw.WriteLine($"Level_Passed:{saveGame.clearedStage}");
                sw.WriteLine($"Damage:{saveGame.damage}");
                sw.WriteLine($"Player_Position:{saveGame.playerPosition}");
                sw.WriteLine($"Enemy_Position:{saveGame.enemyPosition}");
                sw.WriteLine($"Weapon:{saveGame.weapon}");
            }

        }
    }
}
