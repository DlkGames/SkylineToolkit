using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkylineToolkit.Options
{
    public class XmlOptionsProvider : BaseOptionsProvider
    {
        public XmlOptionsProvider(ModOptions modOptions)
            : base(modOptions)
        {
        }

        public override void Save()
        {
            throw new NotImplementedException();
        }

        public override void Reload()
        {
            throw new NotImplementedException();
        }
    }
}
