using SE_Praktikum.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SE_Praktikum.Core
{
    interface ISaveContainer
    {
        SaveGame SaveGame { get; set; }
    }
}
