using ICities;

namespace SkylineToolkit
{
    public interface IMod : IUserMod, ILoadingExtension
    {
        string ModName { get; }

        string ModDescription { get; }
    }
}
