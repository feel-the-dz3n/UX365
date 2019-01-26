using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Windows;

namespace UX365Service
{
    public partial class UxService : ServiceBase
    {
        public List<Process> ControlProcess = new List<Process>();

        public UxService()
        {
            InitializeComponent();
        }

        public void RegisterUX365Process(Process p)
        {
            if(ControlProcess.Contains(p))
                ControlProcess.Add(p);
        }

        public void CheckStatusThread()
        {
            while(true)
            {
                Thread.Sleep(3000);

                for (int i = 0; i < ControlProcess.Count; i++)
                {
                    if (ControlProcess[i].HasExited)
                    {
                        ControlProcess.Remove(ControlProcess[i]);
                        continue;
                    }
                    if (!ControlProcess[i].Responding)
                    {
                        ControlProcess[i].Kill();
                        ControlProcess[i].WaitForExit();

                        ControlProcess[i].StartInfo.Arguments = "-service " + Process.GetCurrentProcess().Id + " -safe -wnd";
                        ControlProcess[i].Start();
                    }
                }
            }
        }
        

        protected override void OnStart(string[] args)
        {
            foreach (var UX in Process.GetProcessesByName("UX365Control"))
            {
                ControlProcess.Add(UX);
            }

            new Thread(CheckStatusThread).Start();
            
            //File.Create("C:\\start").Dispose();
        }

        protected override void OnStop()
        {
            for(int i = 0; i < ControlProcess.Count; i++)
            {
                ControlProcess[i].Kill();
            }
            //File.Create("C:\\stop").Dispose();
        }
    }
}
