using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UX365StatusChecker
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if(args.Length >= 1)
            {
                Process UxProcess = Process.GetProcessById(int.Parse(args[0]));
                string Path = args[1];

                while (true)
                {
                    Thread.Sleep(3000);

                    if (UxProcess.HasExited)
                    {
                        Environment.Exit(0);
                    }
                    if (!UxProcess.Responding)
                    {
                        string OldStartInfo = UxProcess.StartInfo.FileName;

                        UxProcess.Kill();


                        UxProcess = new Process()
                        {
                            StartInfo =
                            {
                                FileName = Path,
                                Arguments = "-safe-kill"
                            }
                        };
                        UxProcess.Start();
                        Environment.Exit(0);
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow(args));
        }
    }
}
