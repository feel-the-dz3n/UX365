using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace UX365ExtraSkin
{
    public class UX365Skin : UX365Core.WindowSkin
    {
        public UX365Skin()
        {
            Initialize("UX365 Extra Skin");
        }
        public Window GetControl()
        {
            return new SkinWindow1();
        }
    }
}
