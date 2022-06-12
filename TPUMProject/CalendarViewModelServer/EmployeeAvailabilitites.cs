using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CalendarViewModelServer
{
    [Serializable()]
    public class EmployeeAvailabilitites : ISerializable
    {
        public int Id { get; set; }
        public List<Availability> Availabilitites { get; set; }

        public bool isAdded = false;

        public EmployeeAvailabilitites()
        {
            Id = 0;
            Availabilitites = new List<Availability>();
        }

        public EmployeeAvailabilitites(int id, List<Availability> availabilitites)
        {
            Id = id;
            Availabilitites = availabilitites;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", Id);
            info.AddValue("availabilitites", Availabilitites);
            info.AddValue("isAdded", isAdded);
        }

        public EmployeeAvailabilitites(SerializationInfo info, StreamingContext context)
        {
            Id = (int)info.GetValue("id", typeof(int));
            Availabilitites = (List<Availability>)info.GetValue("availabilities", typeof(List<Availability>));
        }
    }
}
