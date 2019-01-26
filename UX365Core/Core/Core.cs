using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace UX365Core
{
    public static class Core
    {
        public static List<Window> RegisteredWindows = new List<Window>();
        private static bool _isPaused = false;
        public static bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;
                Log.PrintMethod(typeof(Core), " -> " + value.ToString());
            }
        }

        public static void Initialize(System.Windows.Controls.TextBox output = null)
        {
            Log.Initialize(output);
            Log.PrintMethod(typeof(Core), "(...)");

            IsPaused = false;

            Properties.Load();
            
            UpdateWindows();
        }

        public static void UpdateWindows()
        {
            new Thread(UpdateWindowsThread).Start();
        }

        private static void UpdateWindowsThread()
        {
            Log.PrintMethod(typeof(Core));

            foreach(var wnd in WindowHelper.GetAvailableWindows())
            {
                if (!RegisteredWindows.Contains(wnd))
                {
                    RegisteredWindows.Add(wnd);
                    Draw.UpdateSkin(wnd);
                }
            }
        }

        public static void ApplyNewSkin(WindowSkin skin)
        {
            Log.PrintMethod(typeof(Core), $"({skin.SkinName})");

            Properties.Config.UsedSkin = skin;
            Properties.Save();

            for (int i = 0; i < RegisteredWindows.Count; i++)
            {
                Window wnd = RegisteredWindows[i];

                wnd.SetSkin(skin);
            }

            Log.PrintMethod(typeof(Core), $" end");
        }
        

        public static void Unload(bool quit = false, int exitCode = 0)
        {
            Log.PrintMethod(typeof(Core), $"({quit}, {exitCode})");

            Properties.Save();

            try
            {
                IsPaused = true;
                for (int i = 0; i < RegisteredWindows.Count; i++)
                {
                    Window wnd = RegisteredWindows[i];
                    wnd.SetSkin(null); // or Draw.KillSkin(wnd);
                }
            }
            finally
            {
                Log.Unload();

                if (quit)
                    Environment.Exit(exitCode);
            }
        }

        public static UX365Core.WindowSkin LoadSkin(string filename)
        {
            if (filename == null)
                return null;

            Log.PrintMethod(typeof(Core), $"({filename})");

            try
            {
                WindowSkin skin = new WindowSkin();
                filename = new FileInfo(filename).FullName; // to load assembly we need to have full name

                Assembly assembly = Assembly.LoadFile(filename);
                
                // Search for UX365Skin class (type)
                Type SkinType = null;
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Name.ToLower() == "ux365skin")
                    {
                        SkinType = type;
                        break;
                    }
                }

                if (SkinType == null) // If class not found
                    throw new Exception("File is broken or it is not a UX365 skin (UX365Skin class not found)");
                
                // Create UX365 instance from dll  // like: new UX365();
                skin = (WindowSkin)Activator.CreateInstance(SkinType);

                // Search for GetControl method in Type
                MethodInfo GetControlMethod = null;
                foreach (var method in skin.GetType().GetMethods())
                {
                    if (method.Name.ToLower() == "getcontrol")
                    {
                        GetControlMethod = method;
                        break;
                    }
                }

                if (GetControlMethod == null) // If method not found
                    throw new Exception("File is broken or it is not a UX365 skin (GetControl method not found)");

                // Everything is okay
                skin.SkinAssembly = assembly;
                skin.MethodGetControl = GetControlMethod;

                Log.WriteLine("Skin " + skin.SkinName + " loaded.");

                return skin;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Can't load {new FileInfo(filename).Name}: {ex.Message} ({ex.GetType().Name})", "UX365 Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return null;
            }
        }
    }
}
