using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Core
{
    public enum SaveSlot
    {
        Slot1,
        Slot2,
        Slot3
    }
    interface ISaveGameHandler
    {
        void Save();
        void Load();

        SaveSlot SaveSlot { get; set; }
    }
}
