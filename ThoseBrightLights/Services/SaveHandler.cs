using System.Collections.Generic;
using System.IO;
using NLog;
using ThoseBrightLights.Core;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Services
{
    public class SaveHandler
    {
        private readonly Logger _logger;

        public SaveHandler()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        // Mapping
        Dictionary<SaveSlot, string> _saveSlot = new Dictionary<SaveSlot, string>()
        {
            { SaveSlot.Slot1, "Save/Savestate_1.txt" },
            { SaveSlot.Slot2, "Save/Savestate_2.txt" },
            { SaveSlot.Slot3, "Save/Savestate_3.txt" },
        };

        // Save function: Writes a savefile
        public void Save(SaveGame saveGame, SaveSlot slot)
        {
            var dir = _saveSlot[slot].Split("/")[0];
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (StreamWriter sw = new StreamWriter(_saveSlot[slot]))
            {
                sw.Write($"Level_Passed:{saveGame.clearedStage};");
                sw.Write($"Damage:{saveGame.damage};");
                sw.Write($"Player_Position:{saveGame.playerPosition};");
                sw.Write($"Enemy_Position:{saveGame.enemyPosition};");
                sw.Write($"Weapon:{saveGame.weapon};");
                sw.Write($"Score:{saveGame.score};");
                sw.Write($"Music_Volume:{saveGame.musicVolume};");
                sw.Write($"Sessions_played:{saveGame.sessions};");
            }
        }

        // Getting the datas of a savefile
        public SaveGame Load(SaveSlot slot)
        {
            SaveGame saveGame = new SaveGame();

            using (StreamReader sr = new StreamReader(_saveSlot[slot]))
            {
                string s = sr.ReadToEnd().Trim();
                foreach (var a in s.Split(";"))
                {
                    // Checks the value of all possible fields
                    var keyValuePair = a.Split(':');
                    switch (keyValuePair[0])
                    {
                        case "Level_Passed":
                            saveGame.clearedStage = int.Parse(keyValuePair[1]);
                            break;
                        case "Damage":
                            saveGame.damage = float.Parse(keyValuePair[1]);
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
                            _logger.Warn($"unknown keyword {keyValuePair}");
                            break;
                    }
                }
            }

            return saveGame;
        }

        // Check if there is one already existing
        public bool SaveExists(SaveSlot saveSlot)
        {
            return File.Exists(_saveSlot[saveSlot]);
        }
    }
}