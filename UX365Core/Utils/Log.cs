using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UX365Core
{
    public static class Log
    {
        private static StreamWriter writer;
        private static System.Windows.Controls.TextBox OutputBox;
        public static void Initialize(System.Windows.Controls.TextBox outputGUI = null)
        {
            writer = new StreamWriter("UX365.log");
            writer.AutoFlush = true;
            OutputBox = outputGUI;
        }

        public static void Unload()
        {
            if (writer != null)
                writer.Dispose();
        }
        
        public static void PrintMethod(Type type, string method, string text, bool printInWindow = true)
        {
#if IMPLEMENTED
            MethodInfo info = type.GetMethod(method);

            if (!info.IsPrivate)
                WriteLine(" -> " +type.FullName + "." + info.Name + text, printInWindow);
            else
                WriteLine(" -> " +type.FullName + "." + method + text, printInWindow);
#else
            WriteLine(" -> " + type.FullName + "." + method + text, printInWindow);
#endif
        }

        public static void PrintMethod(Type type, string text = "()", bool printInWindow = true)
        {
            WriteLine(" -> " + type.FullName + "." + new StackFrame(1).GetMethod().Name + text, printInWindow);
        }

        public static void PrintMethod(Action method, string text = "()", bool printInWindow = true)
        {
            WriteLine(" -> " + method.Method.Name + "." + new StackFrame(1).GetMethod().Name + text, printInWindow);
        }

        public static void WriteLine(string text, bool printInWindow = true)
        {
            if (writer != null)
                writer.WriteLine(text);

            if(OutputBox != null && printInWindow)
            {
                if(!OutputBox.Dispatcher.CheckAccess())
                {
                    OutputBox.Dispatcher.Invoke(new Action(() => { OutputBox.AppendText(text + Environment.NewLine); }));
                }
                else
                    OutputBox.AppendText(text + Environment.NewLine);
            }
        }

        public static void Write(string text, bool printInWindow = true)
        {
            if (writer != null)
                writer.Write(text);

            if (OutputBox != null && printInWindow)
            {
                if (!OutputBox.Dispatcher.CheckAccess())
                {
                    OutputBox.Dispatcher.Invoke(new Action(() => { OutputBox.AppendText(text); }));
                }
                else
                    OutputBox.AppendText(text);
            }
        }

    }
}
