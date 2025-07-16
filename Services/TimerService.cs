using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FireTestingApp_net8.Services
{
    public class TimerService
    {
        public event EventHandler? TimeUpdated;

        private DispatcherTimer timer;
        private TimeSpan timeLeft;

        public TimerService()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        public void SetMinutes(int minutes)
        {
            timeLeft = TimeSpan.FromMinutes(minutes);
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public TimeSpan GetTimeLeft()
        {
            return timeLeft;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft = timeLeft - TimeSpan.FromSeconds(1);
                TimeUpdated?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                timer.Stop();
            }
        }
    }
}
