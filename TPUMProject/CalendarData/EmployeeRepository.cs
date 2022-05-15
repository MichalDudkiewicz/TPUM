using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CalendarData
{
    public class EmployeeRepository : IRepository<IEmployee>
    {
        private bool deadlineLock;
        public event Action<bool> onDeadlineLockChange;
        private readonly object mutex = new object();
        WebSocketConnection _wclient = null;

        public EmployeeRepository()
        {
            repositoryEntities = new Dictionary<int, IEmployee>();
        }

        public override void defaultInitialize()
        {
            IEmployee employee = new Employee(0);
            repositoryEntities.Add(0, employee);

            IEmployee employee2 = new Employee(1);
            repositoryEntities.Add(1, employee2);

            connect();
        }

        private async void connect()
        {
            Uri uri = new Uri("ws://localhost:6966");
            _wclient = await WebSocketClient.Connect(uri, message => Console.WriteLine(message));

            _wclient.onMessage = (data) =>
            {
                parseAndStore($"{data}");
            };
        }

        private void parseAndStore(string message)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(EmployeeAvailabilitites));
            StringReader reader = new StringReader(message);
            EmployeeAvailabilitites ea = (EmployeeAvailabilitites)deserializer.Deserialize(reader);
            reader.Close();
            foreach (Availability a in ea.Availabilitites)
            {
                GetById(ea.Id).addAvailability(a.startTime, a.endTime);
            }
        }

        public override void AddAvailability(int employeeId, DateTime startTime, DateTime endTime)
        {
            List<Availability> availabilitiesAdded = new List<Availability>();
            Availability av = new Availability(startTime, endTime);
            availabilitiesAdded.Add(av);
            EmployeeAvailabilitites ea = new EmployeeAvailabilitites(employeeId, availabilitiesAdded);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EmployeeAvailabilitites));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ea);
                string m = textWriter.ToString();
                send(m);
            }
        }

        private async void send(string message)
        {
            await _wclient.SendAsync(message);
        }

        public bool DeadlineLock
        {
            get {
                lock (mutex)
                {
                    return deadlineLock;
                }
            }
            set { 
                lock(mutex)
                {
                    deadlineLock = value;
                    onDeadlineLockChange?.Invoke(value);
                }
            }
        }

        public EmployeeRepository(List<IEmployee> employees)
        {
            deadlineLock = false;
            repositoryEntities = employees.ToDictionary(keySelector: e => e.GetId(), elementSelector: e => e);
        }
    }
}
