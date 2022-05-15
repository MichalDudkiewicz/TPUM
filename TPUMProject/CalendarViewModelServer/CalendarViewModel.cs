using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CalendarViewModelServer
{
    public class ViewModel : ICalendarViewModel
    {
        private CalendarModelServer.ICalendarModel calendarModel;
        public event EventHandler<string> SendData;

        private ObservableCollection<CalendarViewModelServer.Availability> _availabilites;

        public ObservableCollection<CalendarViewModelServer.Availability> Availabilities
        {
            get
            {
                return _availabilites;
            }
        }

        private void onAvailabilitesChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<CalendarModelServer.IAvailability> senderCollection = sender as ObservableCollection<CalendarModelServer.IAvailability>;
            var newAvailabilities = senderCollection.ToList();
            List<IAvailability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarModelServer.IAvailability, IAvailability>(Convert));
            
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                List<Availability> availabilitiesAdded = new List<Availability>();
                foreach (var item in e.NewItems)
                {
                    Availability av = Convert((CalendarModelServer.IAvailability)item);
                    _availabilites.Add(av);
                    availabilitiesAdded.Add(av);
                }
                EmployeeAvailabilitites ea = new EmployeeAvailabilitites(ActiveEmployeeId, availabilitiesAdded);
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(EmployeeAvailabilitites));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, ea);
                    string m = textWriter.ToString();
                    SendData?.Invoke(ea, m);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                _availabilites.Clear();
                EmployeeAvailabilitites ea = new EmployeeAvailabilitites(ActiveEmployeeId, _availabilites.ToList());
                MemoryStream stream = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, ea);
                string message = System.Text.Encoding.UTF8.GetString(stream.ToArray());
                SendData?.Invoke(ea, message);
                stream.Close();
            }
        }

        public static Availability Convert(CalendarModelServer.IAvailability a)
        {
            return new Availability(a);
        }

        public ViewModel()
        {
            calendarModel = new CalendarModelServer.CalendarModel();

            _availabilites = new ObservableCollection<Availability>();
            ActiveEmployeeId = 0;
        }

        public ViewModel(CalendarModelServer.ICalendarModel model)
        {
            calendarModel = model;

            _availabilites = new ObservableCollection<Availability>();
            ActiveEmployeeId = 0;
        }

        public void receiveData(string message)
        {        
            XmlSerializer deserializer = new XmlSerializer(typeof(EmployeeAvailabilitites));
            StringReader reader = new StringReader(message);
            EmployeeAvailabilitites ea = (EmployeeAvailabilitites)deserializer.Deserialize(reader);
            reader.Close();
            foreach (Availability av in ea.Availabilitites)
            {
                calendarModel.AddActiveEmployeeAvailability(av.startTime, av.endTime);
            }
        }

        public int ActiveEmployeeId
        {
            get { return calendarModel.getActiveEmployeeId(); }
            set {
                Availabilities.Clear();
                calendarModel.availabilities().CollectionChanged -= onAvailabilitesChange;
                calendarModel.setActiveEmployeeId(value);


                List<CalendarModelServer.IAvailability> newAvailabilities = calendarModel.availabilities().ToList();
                List<Availability> newLogicAvailabilities = newAvailabilities.ConvertAll(new Converter<CalendarModelServer.IAvailability, Availability>(Convert));

                foreach (var a in newLogicAvailabilities)
                {
                    _availabilites.Add(a);
                }

                calendarModel.availabilities().CollectionChanged += onAvailabilitesChange;
            }
        }
    }
}
