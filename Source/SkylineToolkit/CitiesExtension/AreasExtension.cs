using ICities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.CitiesExtension
{
    public abstract class AreasExtension : IAreasExtension, ICitiesAreasExtension
    {
        private bool isCreated;

        public bool IsCreated
        {
            get { return isCreated; }
            private set { isCreated = value; }
        }

        public IManagers Managers
        {
            get { return this.AreasManager.managers; }
        }

        public IAreas AreasManager { get; private set; }

        public int MaxAreaCount
        {
            get
            {
                return this.AreasManager.maxAreaCount;
            }
            set
            {
                this.AreasManager.maxAreaCount = value;
            }
        }

        public int UnlockedAreaCount
        {
            get { return this.AreasManager.unlockedAreaCount; }
        }

        public Vector2 StartTile
        {
            get { return new Vector2(this.StartTileX, StartTileZ); }
        }

        public int StartTileX
        {
            get { return this.AreasManager.startTileX; }
        }

        public int StartTileZ
        {
            get { return this.AreasManager.startTileZ; }
        }

        public bool IsAreaUnlocked(int x, int z)
        {
            return this.AreasManager.IsAreaUnlocked(x, z);
        }

        public bool CanUnlockArea(int x, int z)
        {
            return this.AreasManager.CanUnlockArea(x, z);
        }

        public int GetAreaPrice(int x, int z)
        {
            return this.AreasManager.GetAreaPrice(x, z);
        }

        public void UnlockArea(int x, int z, bool requireMoney)
        {
            this.AreasManager.UnlockArea(x, z, requireMoney);
        }

        public virtual bool OnCanUnlockArea(int x, int z, bool originalResult)
        {
            return originalResult;
        }

        public virtual void OnCreated(IAreas areas)
        {
            this.AreasManager = areas;
            this.isCreated = true;
        }

        public virtual int OnGetAreaPrice(uint ore, uint oil, uint forest, uint fertility, uint water, bool road, bool train, bool ship, bool plane, float landFlatness, int originalPrice)
        {
            return originalPrice;
        }

        public virtual void OnReleased()
        {
        }

        public virtual void OnUnlockArea(int x, int z)
        {
        }
    }
}
