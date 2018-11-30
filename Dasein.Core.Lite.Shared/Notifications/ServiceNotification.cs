using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dasein.Core.Lite.Shared
{
    public class ServiceNotification : IServiceNotification, IAppNotification
    {
        public ServiceNotification()
        {
            Status = ServiceStatus.ACTIVE;
        }

        public ServiceNotification(ServiceStatus status)
        {
            Status = status;
        }

        public ServiceStatus Status { get; set; }

        public String Message => String.Format("Service status is [{0}]", Status);

        public string Reason => Message;

        public string Subject => "ServiceMonitoring";

        public Object Payload { get; set; }

        public string Notification => Message;
    }
}
