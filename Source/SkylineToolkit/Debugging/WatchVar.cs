
namespace SkylineToolkit.Debugging
{
    public abstract class WatchVar : IWatchVar
    {
        public string Name
        {
            get;
            private set;
        }

        public object Value
        {
            get;
            protected set;
        }

        public WatchVar(string name)
        {
            this.Name = name;
        }

        public WatchVar(string name, object value)
            : this(name)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            if (this.Value == null)
            {
                return "<null>";
            }

            return this.Value.ToString();
        }
    }
}
