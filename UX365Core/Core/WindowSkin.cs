using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UX365Core
{
    public class WindowSkin
    {
        public string SkinName = "Unknown Skin";
        public void Initialize(string name)
        {
            SkinName = name;
        }

        public System.Windows.Window GetUserControl()
        {
            return (System.Windows.Window)MethodGetControl.Invoke(this, new object[] { });
        }

        public Assembly SkinAssembly = null;
        public MethodInfo MethodGetControl = null;
    }
}
