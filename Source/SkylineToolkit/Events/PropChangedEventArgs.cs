
namespace SkylineToolkit.Events
{
    public class PropChangedEventArgs<T> : SkylineEventArgs
    {
        public string PropertyName { get; set; }

        public T NewValue { get; set; }

        public PropChangedEventArgs(string propertyName, T newValue)
        {
            this.PropertyName = propertyName;
            this.NewValue = newValue;
        }
    }
}
