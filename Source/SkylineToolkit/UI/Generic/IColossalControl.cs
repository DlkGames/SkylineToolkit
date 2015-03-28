using ColossalFramework.UI;
using SkylineToolkit.UI;
using System;

namespace SkylineToolkit.UI.Generic
{
    public interface IColossalControl<T> : IColossalControl
        where T : UIComponent
    {
        new T UIComponent { get; }

        ColossalControl<TControl> AddComponent<TControl>() 
            where TControl : UIComponent;

        TControl FindChild<TControl>(string filter) 
            where TControl : IColossalControl;

        IColossalControl FindColossalChild<TControl>(string filter) 
            where TControl : UIComponent;

        void RemoveComponent<TControl>(TControl control) 
            where TControl : UIComponent;
    }
}
