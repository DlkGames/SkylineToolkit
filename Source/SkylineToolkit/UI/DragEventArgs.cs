using ColossalFramework.UI;
using UnityEngine;

namespace SkylineToolkit.UI
{
    public class DragEventArgs : ControlEventArgs
    {
        public DragEventArgs(UIDragEventParameter originalParams)
            : base(originalParams)
        {
            this.State = (DragDropState)originalParams.state;
            this.Data = originalParams.data;
            this.Position = originalParams.position;
            this.Target = originalParams.target.ToSkylineToolkitControl();
        }

        public DragEventArgs(IColossalControl source, DragDropState state, object data, Vector2 position, IColossalControl target)
        {
            this.State = state;
            this.Data = data;
            this.Position = position;
            this.Source = source;
            this.Target = target;
        }
        
        public DragDropState State { get; set; }

        public object Data { get; set; }

        public Vector2 Position { get; set; }

        public IColossalControl Source { get; set; }

        public IColossalControl Target { get; set; }

        public void Cancel() {
            ((UIDragEventParameter)this.OriginalParameters).Cancel();
        }
    }
}
