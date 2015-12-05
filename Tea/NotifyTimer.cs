using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Tea {
    class NotifyTimer {
        private Timer timer;
        private Stopwatch stopWatch;

        public int Interval {
            get {
                return timer.Interval;
            }
            set {
                timer.Interval = value;
            }
        }
        public event EventHandler Elapsed {
            add {
                timer.Tick += value;
            }
            remove {
                timer.Tick -= value;
            }
        }
        public int Remained {
            get {
                return timer.Interval - (int)stopWatch.ElapsedMilliseconds;
            }
        }

        public NotifyTimer() {
            timer = new Timer();
            stopWatch = new Stopwatch();
        }

        public void Start() {
            timer.Start();
            stopWatch.Start();
        }

        public void Stop() {
            timer.Stop();
            stopWatch.Stop();
        }
    }
}
