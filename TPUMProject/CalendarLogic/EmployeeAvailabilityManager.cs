using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml.Serialization;
using CalendarData;

namespace CalendarLogic
{
    public class EmployeeAvailabilityManager : IEmployeeAvailabilityManager
    {
        private ObservableCollection<IAvailability> availabilities;
        private IEmployee _owningEmployee;
        private int activeEmployeeId = 0;

        private readonly object _dataLock = new object();
        WebSocketConnection _wclient = null;

        public ObservableCollection<IAvailability> getAvailabilities()
        {
            lock (_dataLock)
            {
                return availabilities;
            }
        }

        public void setActiveEmployeeId(int activeEmployeeId)
        {
            lock (_dataLock)
            {
                _owningEmployee.Availabilities().CollectionChanged += onAvailabilitesChange;
                var newAvailabilities = _owningEmployee.Availabilities().ToList();
                List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));
                availabilities = new ObservableCollection<IAvailability>(newLogicAvailabilities);
            }
        }

        public int getActiveEmployeeId()
        {
            lock (_dataLock)
            {
                return activeEmployeeId;
            }
        }

        public static IAvailability Convert(CalendarData.IAvailability a)
        {
            return new Availability(a);
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            var newAvailabilities = _owningEmployee.Availabilities().ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));

            lock (_dataLock)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in e.NewItems)
                    {
                        availabilities.Add(Convert((CalendarData.IAvailability)item));
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
                        availabilities.Remove(Convert((CalendarData.IAvailability)item));
                    }
                }
            }
        }

        public EmployeeAvailabilityManager(IEmployee owningEmployee)
        {
            _owningEmployee = owningEmployee;
            //availabilities = new ObservableCollection<IAvailability>();
            //_owningEmployee.Availabilities().CollectionChanged += onAvailabilitesChange;
            activeEmployeeId = _owningEmployee.GetId();
            _owningEmployee.Availabilities().CollectionChanged += onAvailabilitesChange;
            var newAvailabilities = _owningEmployee.Availabilities().ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));
            availabilities = new ObservableCollection<IAvailability>(newLogicAvailabilities);

            BindingOperations.EnableCollectionSynchronization(availabilities, _dataLock);
        }

        public EmployeeAvailabilityManager(int owningEmployeeId)
        {
            EmployeeMaker maker = new EmployeeMaker();
            _owningEmployee = maker.CreateEmployee(owningEmployeeId);
            //availabilities = new ObservableCollection<IAvailability>();
            //_owningEmployee.Availabilities().CollectionChanged += onAvailabilitesChange;
            activeEmployeeId = _owningEmployee.GetId();
            _owningEmployee.Availabilities().CollectionChanged += onAvailabilitesChange;
            var newAvailabilities = _owningEmployee.Availabilities().ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarData.IAvailability, IAvailability>(Convert));
            availabilities = new ObservableCollection<IAvailability>(newLogicAvailabilities);

            BindingOperations.EnableCollectionSynchronization(availabilities, _dataLock);
        }

        public void AddAvailability(Guid id, DateTime startTime, DateTime endTime)
        {
            lock (_dataLock)
            {
                EmployeeAvailabilitites ea = new EmployeeAvailabilitites(activeEmployeeId);
                ea.AddAvailabilityToList(id,startTime,endTime);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(EmployeeAvailabilitites));
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, ea);
                    string m = textWriter.ToString();
                    send(m);
                }
            }
        }

        public void removeAvailability(Guid id)
        {
            lock (_dataLock)
            {
                _owningEmployee.removeAvailability(id);
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
    }
}
