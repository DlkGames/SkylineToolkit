using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class HitPointInfo : IComparable<HitPointInfo>
    {
        public UIComponent Component { get; private set; }

        public Vector3 HitPoint { get; private set; }

        public float Distance { get; private set; }

        public HitPointInfo(UIHitInfo info)
        {
            this.Component = info.component;
            this.HitPoint = info.point;
            this.Distance = info.distance;
        }

        public HitPointInfo(UIComponent component, Vector3 hitPoint, float distance)
        {
            this.Component = component;
            this.HitPoint = hitPoint;
            this.Distance = distance;
        }

        public int CompareTo(HitPointInfo other)
        {
            return other.Component.renderOrder.CompareTo(this.Component.renderOrder);
        }
    }
}
