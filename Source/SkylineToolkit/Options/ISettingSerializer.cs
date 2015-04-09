using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    /// <summary>
    /// Implementation for a custom setting value serializer used by an <see cref="IOptionsProvider"/> to save
    /// non standard setting types.
    /// </summary>
    /// <typeparam name="TSetting">The type of the non serialized setting.</typeparam>
    /// <typeparam name="TSerializedSetting">The type of the setting after serialization.</typeparam>
    public interface ISettingSerializer<TSetting, TSerializedSetting>
    {
        /// <summary>
        /// Serializes the value of <paramref name="setting"/> and returns the serialized representation.
        /// </summary>
        /// <param name="provider">The options provider which tries to serialize the setting value.</param>
        /// <param name="setting">The setting value to be serialized</param>
        /// <returns>The serialized setting value.</returns>
        TSerializedSetting Serialize(IOptionsProvider provider, TSetting setting);

        /// <summary>
        /// Deserializes a serialized representation of a setting and returns it's actual value.
        /// </summary>
        /// <param name="provider">The options provider which tries to deserialize the setting value.</param>
        /// <param name="value">The serialized value.</param>
        /// <returns>The actual deserialized value.</returns>
        TSetting Deserialize(IOptionsProvider provider, TSerializedSetting value);
    }
}
