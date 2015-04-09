using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.CitiesExtension
{
    public interface ICitiesAreasExtension : IAreasExtension, ICitiesExtension
    {
        IAreas AreasManager { get; }

        int MaxAreaCount { get; set; }

        int UnlockedAreaCount { get; }

        Vector2 StartTile { get; }

        int StartTileX { get; }

        int StartTileZ { get; }

        bool IsAreaUnlocked(int x, int z);

        bool CanUnlockArea(int x, int z);

        int GetAreaPrice(int x, int z);

        void UnlockArea(int x, int z, bool requireMoney);
    }
}
