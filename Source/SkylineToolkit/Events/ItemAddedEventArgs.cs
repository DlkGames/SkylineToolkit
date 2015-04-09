
namespace SkylineToolkit.Events
{
    public class ItemAddedEventArgs<T> : SkylineEventArgs
    {
        public T AddedItem { get; set; }

        public ItemAddedEventArgs(T addedItem)
        {
            this.AddedItem = addedItem;
        }
    }
}
