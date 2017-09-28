using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ProjetSynthese
{
    public static class StaticInventoryPass
    {
        [NotNull]
        public static Inventory Inventory { get; set; }
    }
}

