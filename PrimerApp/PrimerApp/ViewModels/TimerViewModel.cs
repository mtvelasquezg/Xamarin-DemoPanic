using PrimerApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PrimerApp.ViewModels
{

    public class TimerViewModel : NotificationObject
    {

        private Timer _timer;

        static Random generator = new Random();

        static int sec = generator.Next(300, 600);

        private TimeSpan _totalSeconds = new TimeSpan(0, 0, 0, sec);
        public TimeSpan TotalSeconds
        {
            get { return _totalSeconds; }
            set
            {
                _totalSeconds = value;
                OnPropertyChanged();
            }
        }

        public Command StartCommand { get; set; }
        public Command PauseCommand { get; set; }
        public Command StopCommand { get; set; }

       
        public TimerViewModel()
        {
            StartCommand = new Command(StartTimerCommand);
            PauseCommand = new Command(PauseTimerCommand);
            StopCommand = new Command(StopTimerCommand);
            //make sure you put this in the constructor
            _timer = new Timer(TimeSpan.FromSeconds(1), CountDown);
            TotalSeconds = _totalSeconds;
            StartTimerCommand();
        }
        public void StartTimerCommand()
        {
            _timer.Start();
        }

        /// <summary>
        /// Counts down the timer
        /// </summary>
        private void CountDown()
        {
            if (_totalSeconds.TotalSeconds == 0)
            {
                //do something after hitting 0, in this example it just stops/resets the timer
                StopTimerCommand();
            }
            else
            {
                TotalSeconds = _totalSeconds.Subtract(new TimeSpan(0, 0, 0, 1));
            }
        }

        /// <summary>
        /// Pauses the timer
        /// </summary>
        private void PauseTimerCommand()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Stops and resets the timer
        /// </summary>
        private void StopTimerCommand()
        {
            TotalSeconds = new TimeSpan(0, 0, 0, 10);
            _timer.Stop();
        }
    }
}
