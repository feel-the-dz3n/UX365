using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace UX365ServiceKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (ServiceController service = new ServiceController("UX365"))
                {
                    if (service.Status == ServiceControllerStatus.Running)
                        service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
            catch { }
            Environment.Exit(0);
        }
    }
}
