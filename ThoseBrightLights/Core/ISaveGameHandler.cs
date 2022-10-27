using System;
using System.Collections.Generic;
using System.Text;
using ThoseBrightLights.Models;

namespace ThoseBrightLights.Core
{
    // Enum for determining which slot to use (instead of any int's)
    public enum SaveSlot
    {
        Slot1,
        Slot2,
        Slot3
    }

    // Interface with all the desired components
    public interface ISaveGameHandler
    {
        void Save();
        void Load();

        SaveSlot SaveSlot { get; set; }

        bool SaveExists(SaveSlot saveSlot);

        void CreateSave();

        SaveGame SaveGame { get; set; }
    }
}
