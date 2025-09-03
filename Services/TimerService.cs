using System.Windows.Threading;

namespace FireTestingApp_net8.Services
{
    public class TimerService
    {
        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;

        public event EventHandler? TimeUpdated;

        public TimerService()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        public void SetMinutes(int minutes)
        {
            _timeLeft = TimeSpan.FromMinutes(minutes);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public TimeSpan GetTimeLeft()
        {
            return _timeLeft;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (_timeLeft.TotalSeconds > 0)
            {
                _timeLeft = _timeLeft - TimeSpan.FromSeconds(1);
                TimeUpdated?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                _timer.Stop();
            }
        }
    }
}
