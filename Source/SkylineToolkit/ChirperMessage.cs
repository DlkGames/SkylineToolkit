using ICities;

namespace SkylineToolkit
{
    internal class ChirperMessage : IChirperMessage
    {
        public uint senderID { get; set; }

        public string senderName { get; set; }

        public string text { get; set; }
    }
}
