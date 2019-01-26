using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UX365Core
{
    [Serializable]
    public class UX365ConfigClass
    {
        [XmlIgnore]
        public WindowSkin UsedSkin = null;

        public string UsedSkinFullPath
        {
            set
            {
                UsedSkin = Core.LoadSkin(value);
            }
            get
            {
                return null;  // FIX ME

                if (UsedSkin == null)
                    return null;
                else
                    return UsedSkin.SkinAssembly.Location;

            }
        }

        public List<string> ExcludeWindows = new List<string>();
        public List<string> ExcludeProcesses = new List<string>();
        public bool OnlyUX365Control = true;

        public UX365ConfigClass() { }
    }


    public class Properties
    {
        public static UX365ConfigClass Config = new UX365ConfigClass();
        private const string FileName = "ux365config.xml";
        public static void Load()
        {
            Log.PrintMethod(typeof(Properties));

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(UX365ConfigClass));
                using (FileStream stream = File.Open(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    UX365ConfigClass c = (UX365ConfigClass)s.Deserialize(stream);
                    Config = c;
                }
            }
            catch (FileNotFoundException)
            {
                Save();
            }
            catch (Exception ex)
            {
                Log.PrintMethod(typeof(Properties), " - can't load: " + ex.Message + " (" + ex.GetType().Name + ")");
            }
        }

        public static void Save()
        {
            Log.PrintMethod(typeof(Properties));

            try
            {
                XmlSerializer s = new XmlSerializer(typeof(UX365ConfigClass));
                using (FileStream stream = File.Open(FileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    s.Serialize(stream, Config);
                }
            }
            catch (Exception ex)
            {
                Log.PrintMethod(typeof(Properties), " - can't load: " + ex.Message + " (" + ex.GetType().Name + ")");
            }
        }
       
    }

    
}
