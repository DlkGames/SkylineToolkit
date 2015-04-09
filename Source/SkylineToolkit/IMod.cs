using ICities;

namespace SkylineToolkit
{
    /// <summary>
    /// Basic implementation for a custom mod.
    /// </summary>
    public interface IMod : IUserMod, ILoadingExtension
    {
        /// <summary>
        /// Returns the name of the mod.
        /// </summary>
        string ModName { get; }

        /// <summary>
        /// Returns a short description for the mod.
        /// </summary>
        string ModDescription { get; }

        /// <summary>
        /// Returns the mod's current version.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Returns the author of the mod.
        /// </summary>
        string Author { get; }
    }
}
