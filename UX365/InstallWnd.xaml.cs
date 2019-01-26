using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UX365
{
    /// <summary>
    /// Interaction logic for InstallWnd.xaml
    /// </summary>
    public partial class InstallWnd : Window
    {
        public InstallWnd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileInfo ServiceExe = new FileInfo("UX365Service.exe");
            if(!ServiceExe.Exists)
            {
                MessageBox.Show("UX365Service.exe not found", "UX365 Installer");
                Environment.Exit(404);
            }
            if (MessageBox.Show("Install?", "UX365 Installer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    using (ServiceController service = new ServiceController("UX365"))
                    {
                        if (service.Status == ServiceControllerStatus.Running)
                            service.Stop();
                        service.WaitForStatus(ServiceControllerStatus.Stopped);
                    }
                    var up = Process.Start(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe", "-u \"" + ServiceExe.FullName + "\"");
                    up.WaitForExit();
                }
                catch { }
                var p  = Process.Start(@"C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe", ServiceExe.FullName);
                p.WaitForExit();
                if(MessageBox.Show("The result is: " + p.ExitCode + "\r\n\r\nDo you want to start the service?", "UX365 Installer", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (ServiceController service = new ServiceController("UX365"))
                        {
                            if (service.Status == ServiceControllerStatus.Stopped)
                                service.Start();

                            service.WaitForStatus(ServiceControllerStatus.Running);
                        }
                        MessageBox.Show("Done.", "UX365 Installer");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Can't start UX365 service:\r\n" + ex.Message, "UX365 Installer");
                    }
                }

                Environment.Exit(0);
            }
            else
                Environment.Exit(1);
        }
    }
}
