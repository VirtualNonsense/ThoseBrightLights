using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SE_Praktikum.Core;

namespace SE_Praktikum.Services
{
    public class SaveHandler
    {
        Dictionary<SaveSlot, string> _saveSlot = new Dictionary<SaveSlot, string>()
        {
            { SaveSlot.Slot1,"Save/Savestate_1.txt" },
            { SaveSlot.Slot2,"Save/Savestate_2.txt" },
            { SaveSlot.Slot3,"Save/Savestate_3.txt" },
        };

        public void Save(SaveGame saveGame, SaveSlot slot)
        {
            using (StreamWriter sw = new StreamWriter(_saveSlot[slot]))
            {
                sw.WriteLine($"Level_Passed:{saveGame.clearedStage}");
                sw.WriteLine($"Damage:{saveGame.damage}");
                sw.WriteLine($"Player_Position:{saveGame.playerPosition}");
                sw.WriteLine($"Enemy_Position:{saveGame.enemyPosition}");
                sw.WriteLine($"Weapon:{saveGame.weapon}");
                sw.WriteLine($"Score:{saveGame.score}");
                sw.WriteLine($"Music_Volume:{saveGame.musicVolume}");
                sw.WriteLine($"Sessions_played:{saveGame.sessions}");
            }
        }

        public SaveGame Load(SaveSlot slot)
        {
            SaveGame saveGame = new SaveGame();

            using (StreamReader sr = new StreamReader(_saveSlot[slot]))
            {

                string s = sr.ReadToEnd().Trim();
                foreach (var a in s.Split("\r\n"))
                {
                    var keyValuePair = a.Split(':');
                    switch (keyValuePair[0])
                    {
                        case "Level_Passed":
                            saveGame.clearedStage = int.Parse(keyValuePair[1]);
                            break;
                        case "Damage":
                            saveGame.damage = int.Parse(keyValuePair[1]);
                            break;
                        case "Player_Position":
                            saveGame.playerPosition = int.Parse(keyValuePair[1]);
                            break;
                        case "Enemy_Position":
                            saveGame.enemyPosition = int.Parse(keyValuePair[1]);
                            break;
                        case "Weapon":
                            saveGame.weapon = int.Parse(keyValuePair[1]);
                            break;
                        case "Score":
                            saveGame.score = int.Parse(keyValuePair[1]);
                            break;
                        case "Music_Volume":
                            saveGame.musicVolume = float.Parse(keyValuePair[1]);
                            break;
                        case "Sessions_played":
                            saveGame.sessions = uint.Parse(keyValuePair[1]);
                            break;
                        default:
                            Console.WriteLine("What??");
                            break;
                    }
                }
            }
            return saveGame;
        }

        public bool SaveExists(SaveSlot saveSlot)
        {
            return File.Exists(_saveSlot[saveSlot]);
        }


    }
}
