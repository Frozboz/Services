using FileReaderService.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileReaderService.Logic
{
    public class FileReaderLogic
    {
        ILog _log;
        Settings _settings;
        internal Timer _pollTimer;
        internal Timer _pulseTimer;
        internal int _numTotalPolls;
        internal int _numPulsePolls;

        public FileReaderLogic(Settings settings, ILog log)
        {
            _log = log;
            _settings = settings;

            _log.Info(_settings.ToString());
            StartTimers();
        }

        public void StartTimers()
        {
            _pollTimer = new Timer(ProcessPollTimerEvent, 0, _settings.SecondsBetweenPolls * 1000, Timeout.Infinite);
            _pulseTimer = new Timer(ProcessPulseTimerEvent, 0, _settings.PulseIntervalSeconds * 1000, Timeout.Infinite);
        }

        private void ProcessPollTimerEvent(object obj)
        {
            _numTotalPolls++;
            _numPulsePolls++;
            _log.Info($"Polling: {_settings.PolledDirectory}.  Next poll in {_settings.SecondsBetweenPolls} sec");
            _pollTimer.Change(_settings.SecondsBetweenPolls * 1000, Timeout.Infinite);
        }

        private void ProcessPulseTimerEvent(object obj)
        {
            _log.Info($"Pulse timer expired.  Number of polls last {_settings.PulseIntervalSeconds} sec: {_numPulsePolls}, total: {_numTotalPolls}");
            _numPulsePolls = 0;
            _pulseTimer.Change(_settings.PulseIntervalSeconds * 1000, Timeout.Infinite);
        }
    }
}
