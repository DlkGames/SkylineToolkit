using System;


#if NOUNITY
namespace DlkGames.Unity.TestHelpers.TestableUnity
#else
namespace UnityEngine
#endif
{
    public class AddComponentMenu : Attribute
    {
        public string Text
        {
            get;
            set;
        }

        public AddComponentMenu(string text)
        {
            this.Text = text;
        }
    }
}
