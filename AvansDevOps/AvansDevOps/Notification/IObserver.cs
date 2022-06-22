using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Notification
{
    public interface IObserver
    {
        void Update(NotificationSubject subject);
    }
}
