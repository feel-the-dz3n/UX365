using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace UX365Control
{
    public class UXControl
    {
        public string[] args = null;
        public MainWindow MainWnd = null;
        public bool SafeMode = false;

        public UXControl(string[] a)
        {
            args = a;

            MainWnd = new MainWindow(args);



            for (int i = 0; i < args.Length; i++)
            {
                try
                {
                    if (args[i] == "-wnd")
                    {
                        MainWnd.Show();
                    }

                    if (args[i] == "-safe-kill")
                    {
                        if (MessageBox.Show("UX365 was not responding and due to it terminated. Do you want to start it in the Safe Mode?\r\n\r\n* settings will be reseted", "UX365", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            SafeMode = true;
                            Properties.Settings.Default.Reset();
                        }
                    }

                    if (args[i] == "-safe")
                    {
                        if (MessageBox.Show("Do you want to start UX365 in the Safe Mode?\r\n\r\n* settings will be reseted", "UX365", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            SafeMode = true;
                            Properties.Settings.Default.Reset();
                        }
                    }
                }
                catch { }
            }

            try
            {
                if (Debugger.IsAttached)
                    throw new Exception("Debugger is attached");

                Process Checker = new Process()
                {
                    StartInfo =
                    {
                        FileName = Path.Combine(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.FullName, "UX365StatusChecker.exe"),
                        Arguments = Process.GetCurrentProcess().Id.ToString() + " \""+System.Reflection.Assembly.GetExecutingAssembly().Location+"\""
                    }
                };
                Checker.Start();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("Can't start Status Checker: " + ex.Message + "\r\nYour applications may become broken!\r\n\r\nDo you want to continue?", "UX365", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    Environment.Exit(2);
                }
            }

            /*if (ServiceProcess == null)
            {
                MessageBox.Show("Only service can start UX365. Please, install this program correctly.", "Can't start UX365", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }*/

            MainWnd.Show();

            UX365Core.Core.Initialize(MainWnd.tbKEK);
        }
    }
}
