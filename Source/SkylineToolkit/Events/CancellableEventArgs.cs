
namespace SkylineToolkit.Events
{
    public class CancellableEventArgs : SkylineEventArgs
    {
        private bool cancel = false;

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }

    }
}
