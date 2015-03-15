namespace SkylineToolkit.Debugging
{
    public class WatchVar<T> : WatchVar
    {
        new public T Value
        {
            get
            {
                return (T)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        public WatchVar(string name)
            : base(name) { }

        public WatchVar(string name, T value)
            : base(name, value) { }
    }
}
