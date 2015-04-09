
namespace ColossalFramework.UI
{
    public class UndoData
    {
        public string Text { get; set; }

        public int Index { get; set; }

        public UndoData(string text, int index)
        {
            this.Text = text;
            this.Index = index;
        }
    }
}
