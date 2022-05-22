using System;
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

        public EmployeeDataManager(int id)
        {
            activeEmployeeId = id;
            _owningEmployee = new Employee(id);

            var newAvailabilities = _owningEmployee.Availabilities().ToList();
            availabilities = new ObservableCollection<IAvailability>(newAvailabilities);


            _owningEmployee.Availabilities().CollectionChanged += onCollectionChanged;

            BindingOperations.EnableCollectionSynchronization(availabilities, _dataLock);
        }

        public ObservableCollection<IAvailability> Availabilities()
        {
            return availabilities;
        }

        int ActiveEmployeeId()
        {
            return activeEmployeeId;
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
            reader.Close();
            foreach (CalendarData.IAvailability a in ea.Availabilitites)
            {
                _owningEmployee.addAvailability(a);
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
            return activeEmployeeId;
        }
    }
}
