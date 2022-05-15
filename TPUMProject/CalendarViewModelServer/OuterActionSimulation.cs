using System;
using System.Threading;
using Timer = System.Timers.Timer;
using System.Timers;

namespace CalendarLogicServer
{
    internal class OuterActionSimulation
    {
        public IEmployeeAvailabilityManager manager { get; }
        private Timer timer;
        private SynchronizationContext context = SynchronizationContext.Current;
        private Random gen = new Random();

        private Func<bool> action;

        public bool shouldStop { get; private set; } = false;

        public OuterActionSimulation(IEmployeeAvailabilityManager manager, float interval)
        {
            this.manager = manager;
            action = new Func<bool> (AddRandomAvailabilityAction);

            timer = new Timer(interval * 1000);
            timer.Elapsed += ProgressSimulation;
            timer.AutoReset = true;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        private DateTime RandomDay()
        {
            DateTime today = DateTime.Today;
            DateTime start = new DateTime(today.Year, today.Month, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }


        private void ProgressSimulation(object state, ElapsedEventArgs args)
        {
            context.Post((obj) =>
            {
                action?.Invoke();
            }, null);
        }

        private bool AddRandomAvailabilityAction()
        {
            DateTime rndDay = RandomDay();
            manager.addAvailability(rndDay, rndDay);
            return true;
        }
    }
}
