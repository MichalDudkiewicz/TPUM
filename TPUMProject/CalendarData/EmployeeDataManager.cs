using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Serialization;

namespace CalendarData
{
    public class EmployeeDataManager : IEmployeeDataManager
    {
        private IEmployee _owningEmployee;
        private int activeEmployeeId = 0;
        private readonly object _dataLock = new object();
        WebSocketConnection _wclient = null;
        private ObservableCollection<IAvailability> availabilities;

        InfoTracker tracker;
        InfoReporter reporter;

        public bool startedAdding = false;


        public EmployeeDataManager(int id)
        {
            activeEmployeeId = id;
            _owningEmployee = new Employee(id);

            tracker = new InfoTracker();
            reporter = new InfoReporter("BoolReporter");
            reporter.Subscribe(tracker);

            var newAvailabilities = _owningEmployee.Availabilities().ToList();
            availabilities = new ObservableCollection<IAvailability>(newAvailabilities);

            _owningEmployee.Availabilities().CollectionChanged += onCollectionChanged;

            BindingOperations.EnableCollectionSynchronization(availabilities, _dataLock);
        }

        public ObservableCollection<IAvailability> Availabilities()
        {
            lock (_dataLock)
            {
                return availabilities;
            }
        }

        int ActiveEmployeeId()
        {
            lock (_dataLock)
            {
                return activeEmployeeId;
            }
        }

        private void onCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            lock (_dataLock)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        availabilities.Add((CalendarData.IAvailability)item);
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    availabilities.Clear();
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    foreach (var item in e.OldItems)
                    {
                        availabilities.Remove((CalendarData.IAvailability)item);
                    }
                }
            }
        }


        public void AddAvailability(Guid id, DateTime startTime, DateTime endTime)
        {
            lock (_dataLock)
            {
                EmployeeAvailabilitites ea = new EmployeeAvailabilitites(activeEmployeeId);
                ea.AddAvailabilityToList(id, startTime, endTime);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(EmployeeAvailabilitites));
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, ea);
                    string m = textWriter.ToString();
                    send(m);
                }
            }
        }

        private void parseAndStore(string message)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(EmployeeAvailabilitites));
            StringReader reader = new StringReader(message);
            EmployeeAvailabilitites ea = (EmployeeAvailabilitites)deserializer.Deserialize(reader);

            tracker.TrackBool(ea.isAdded);

            reader.Close();
            if(reporter.receivedValue)
            {
                foreach (CalendarData.IAvailability a in ea.Availabilitites)
                {
                    lock (_dataLock)
                    {
                        _owningEmployee.addAvailability(a);
                    }
                }
            }
        }

        private async void send(string message)
        {
            await _wclient.SendAsync(message);
        }

        public async Task connect()
        {
            Uri uri = new Uri("ws://localhost:6966");
            _wclient = await WebSocketClient.Connect(uri, message => Console.WriteLine(message));

            _wclient.onMessage = (data) =>
            {
                parseAndStore($"{data}");
            };
        }

        public async Task disconnect()
        {

        }

        public void removeAvailability(Guid id)
        {
            lock (_dataLock)
            {
                _owningEmployee.removeAvailability(id);
            }
        }

        int IEmployeeDataManager.ActiveEmployeeId()
        {
            lock (_dataLock)
            {
                return activeEmployeeId;
            }
        }
    }

    public class InfoTracker : IObservable<bool>
    {
        public InfoTracker()
        {
            observers = new List<IObserver<bool>>();
        }

        private List<IObserver<bool>> observers;

        public IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<bool>> _observers;
            private IObserver<bool> _observer;

            public Unsubscriber(List<IObserver<bool>> observers, IObserver<bool> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void TrackBool(Nullable<bool> added)
        {
            foreach (var observer in observers)
            {
                if (!added.HasValue)
                    observer.OnError(new ArgumentNullException());
                else
                    observer.OnNext(added.Value);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }
    }

    public class InfoReporter : IObserver<bool>
    {
        private IDisposable unsubscriber;
        private string instName;

        public bool receivedValue = false;

        public InfoReporter(string name)
        {
            this.instName = name;
        }

        public string Name
        { get { return this.instName; } }

        public virtual void Subscribe(IObservable<bool> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnCompleted()
        {
            Console.WriteLine("Tracker has completed transmitting data to {0}.", this.Name);
            this.Unsubscribe();
        }

        public virtual void OnError(Exception e)
        {
            Console.WriteLine("{0}: The result cannot be determined.", this.Name);
        }

        public virtual void OnNext(bool value)
        {
            Console.WriteLine("{0}: Received info", this.Name);
            receivedValue = value;
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
    }
}
