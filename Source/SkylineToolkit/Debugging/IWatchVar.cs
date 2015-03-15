
namespace SkylineToolkit.Debugging
{
    public interface IWatchVar
    {
        string Name { get; }

        object Value { get; }

        string ToString();
    }
}
