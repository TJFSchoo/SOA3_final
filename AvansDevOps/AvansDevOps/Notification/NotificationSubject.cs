using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvansDevOps.Notification
{
    public abstract class NotificationSubject : ISubject
    {
        private readonly List<IObserver> _observers;
        protected NotificationSubject()
        {
            _observers = new List<IObserver>();
        }

        public void Register(IObserver observer)
        {
            if (_observers.Contains(observer))
                throw new NotSupportedException("Observer is already registered.");

            _observers.Add(observer);
        }

        public void Unregister(IObserver observer)
        {
            if (!_observers.Contains(observer))
                throw new NotSupportedException("Observer is not registered.");

            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            _observers.ForEach(o => o.Update(this));
        }

        public List<IObserver> GetObservers()
        {
            return _observers;
        }
    }
}
