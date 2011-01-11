using System;
using System.Diagnostics;

namespace LSRI.AuditoryGames.Utils
{
    /// <summary>
    /// StopwatchPlus is used to measure the general performance of Silverlight functionality. 
    /// 
    /// Silverlight does not provide a high resolution timer as is available in many operating systems,
    /// so the resolution of this timer is limited to milliseconds. This class is best used to measure
    /// the relative performance of functions over many iterations.
    /// </summary>
    /// @code
    ///  StopwatchPlus sp1 = new StopwatchPlus(
    ///     sw2 => Debug.WriteLine("Time! {0}", sw2.EllapsedMilliseconds));
    ///  Thread.Sleep(500);
    ///  sp1.Stop();    // this will call the stopAction defined in the constructor
    /// @endcode
    /// @author Aaron &lt; http://www.wiredprairie.us/blog/index.php/archives/1030 &gt;
    /// @author Nicolas Van Labeke &lt; http://www.lsri.nottingham.ac.uk/nvl/ &gt;
    /// @version 1.0 Added StopWatchPlus into project
    /// @version 1.1 Added Step action point
    public sealed class StopwatchPlus : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private long _startTick;
        private long _elapsed;
        private bool _isRunning;
        private String _name;
        private Action<StopwatchPlus> _startAction;
        private Action<StopwatchPlus> _stopAction;
        private Action<StopwatchPlus> _stepAction;

        /// <summary>
        /// Creates an instance of the StopwatchPlus class and starts the timer. By
        /// providing a value to the name parameter, the Debug Console automatically
        /// will include the current values when the timer was started and stopped with
        /// this name.
        /// </summary>
        /// <param name="name"></param>
        public StopwatchPlus(string name)
            : this(name, WriteStart, WriteResults, WriteStep) { } 

        /// <summary>
        /// Creates an instance of the StopwatchPlus class and starts the timer.
        /// </summary>
        /// <param name="stopAction">Action to take when the Stop method is called.</param>
        public StopwatchPlus(Action<StopwatchPlus> stopAction)
            :this(null, stopAction)
        {
        }

        /// <summary>
        /// Creates an instance of the StopwatchPlus class and starts the timer.
        /// </summary>
        /// <param name="startAction">Action to take when the timer starts.</param>
        /// <param name="stopAction">Action to take when the Stop method is called.</param>
        public StopwatchPlus(Action<StopwatchPlus> startAction,
            Action<StopwatchPlus> stopAction)
            :this(null, startAction, stopAction)
        {
        }

        /// <summary>
        /// Creates an instance of the StopwatchPlus class and starts the timer.
        /// </summary>
        /// <param name="startAction">Action to take when the timer starts.</param>
        /// <param name="stopAction">Action to take when the Stop method is called.</param>
        public StopwatchPlus(Action<StopwatchPlus> startAction,
            Action<StopwatchPlus> stopAction,
            Action<StopwatchPlus> stepAction)
            : this(null, startAction, stopAction,stepAction)
        {
        }

        /// <summary>
        /// Creates an instance of the StopwatchPlus class and starts the timer.
        /// </summary>
        /// <param name="name">Name of this stop watch (used as output)</param>
        /// <param name="startAction">Action to take when the timer starts.</param>
        /// <param name="stopAction">Action to take when the Stop method is called.</param>
        public StopwatchPlus(string name,
            Action<StopwatchPlus> startAction,
            Action<StopwatchPlus> stopAction,
            Action<StopwatchPlus> stepAction)
        {
            _name = name;
            _startAction = startAction;
            _stopAction = stopAction;
            _stepAction = stepAction;
            Start();
        }

        /// <summary>
        /// Name of the stopwatch
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Completely resets and deactivates the timer.
        /// </summary>
        public void Reset()
        {
            _elapsed = 0;
            _isRunning = false;
            _startTick = 0;
        }

        /// <summary>
        /// Begins the timer.
        /// </summary>
        public void Start()
        {
            if (!_isRunning)
            {
                _startTick = GetCurrentTicks();
                _isRunning = true;
                if (_startAction != null)
                {
                    _startAction(this);
                }
            }
        }

        /// <summary>
        /// Stops the current timer.
        /// </summary>
        public void Step(String comment)
        {
            if (_isRunning)
            {
                _elapsed += GetCurrentTicks() - _startTick;
                if (_stepAction != null)
                {
                    _stepAction(this);
                }
            }
        }

        /// <summary>
        /// Stops the current timer.
        /// </summary>
        public void Stop()
        {
            if (_isRunning)
            {
                _elapsed += GetCurrentTicks() - _startTick;
                _isRunning = false;
                if (_stopAction != null)
                {
                    _stopAction(this);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the instance is currently recording.
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
        }

        /// <summary>
        /// Gets the Ellapsed time as a Timespan.
        /// </summary>
        public TimeSpan Ellapsed
        {
            get { return TimeSpan.FromMilliseconds(EllapsedMilliseconds); }
        }

        /// <summary>
        /// Gets the Ellapsed time as the total number of milliseconds.
        /// </summary>
        public long EllapsedMilliseconds
        {
            get { return GetCurrentElapsedTicks() / TimeSpan.TicksPerMillisecond; }
        }

        /// <summary>
        /// Gets the Ellapsed time as the total number of ticks (which is faked
        /// as Silverlight doesn't have a way to get at the actual "Ticks")
        /// </summary>
        /// <returns>Number of ticks</returns>
        public long EllapsedTicks
        {
            get { return GetCurrentElapsedTicks(); }
        }

        /// <summary>
        /// Gets the Ellapsed time as the total number of ticks (which is faked
        /// as Silverlight doesn't have a way to get at the actual "Ticks")
        /// </summary>
        /// <returns>Number of ticks</returns>
        private long GetCurrentElapsedTicks()
        {
            return (long)(this._elapsed + (IsRunning ? (GetCurrentTicks() - _startTick) : 0));
        }

        private long GetCurrentTicks()
        {
            // TickCount: Gets the number of milliseconds elapsed since the system started.
            return Environment.TickCount * TimeSpan.TicksPerMillisecond;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        #endregion

        private static void WriteStart(StopwatchPlus sw)
        {
            WriteStartInternal(sw);
        }

        // This is not called in a Release build
        [Conditional("DEBUG")]
        private static void WriteStartInternal(StopwatchPlus sw)
        {
            Debug.WriteLine("BEGIN\t{0}", sw._name);

        }

        private static void WriteResults(StopwatchPlus sw)
        {
            WriteResultsInternal(sw);
        }

        // This is not called in a Release build
        [Conditional("DEBUG")]
        private static void WriteResultsInternal(StopwatchPlus sw)
        {
            Debug.WriteLine("END\t{0}\t{1}", sw._name, sw.EllapsedMilliseconds);
        }

        private static void WriteStep(StopwatchPlus sw)
        {
            WriteStepInternal(sw);
        }

        // This is not called in a Release build
        [Conditional("DEBUG")]
        private static void WriteStepInternal(StopwatchPlus sw)
        {
            Debug.WriteLine("BEGIN\t{0}", sw._name);

        }

 
    }
}
