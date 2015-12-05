using System;
using System.Windows.Forms;
using Tea.Properties;

namespace Tea {
    class ProgramContext : ApplicationContext {
        private static int KETTLE_BOIL_DELAY = 7 * 60; // in seconds
        private static int TITLE_UPDATE_INTERVAL = 1; // in seconds
        private static int NOTIFICATION_DELAY = 0; // default delay

        private NotifyIcon notifyIcon;
        private NotifyTimer notifyTimer;
        private Timer updateTitleTimer;

        public ProgramContext() {
            SetupTrayIcon();
            SetupNotifyTimer();

            ShowRemainedTimeMessage(KETTLE_BOIL_DELAY);
        }

        private void SetupTrayIcon() {
            notifyIcon = new NotifyIcon() {
                Text = Resources.AppName,
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem(Resources.MenuExit, OnExitClicked)
                }),
                Visible = true,
            };
        }

        private void SetupNotifyTimer() {
            notifyTimer = new NotifyTimer();
            notifyTimer.Interval = KETTLE_BOIL_DELAY * 1000;
            notifyTimer.Elapsed += OnTimerTick;
            notifyTimer.Start();

            updateTitleTimer = new Timer();
            updateTitleTimer.Interval = TITLE_UPDATE_INTERVAL * 1000;
            updateTitleTimer.Tick += OnUpdateTitle;
            updateTitleTimer.Start();
        }

        private void ShowRemainedTimeMessage(int remainedMinutes) {
            notifyIcon.ShowBalloonTip(
                NOTIFICATION_DELAY,
                Resources.AppName,
                GetRemainedTimeMessage(remainedMinutes),
                ToolTipIcon.Info
            );
        }

        private String GetRemainedTimeMessage(int remainedSeconds) {
            if (remainedSeconds > 60) {
                return String.Format(Resources.KettleWillBoil, remainedSeconds / 60);
            }
            else if (remainedSeconds > 0) {
                return Resources.KettleAlmostBlow;
            }
            else {
                return Resources.KettleBoiling;
            }
        }

        private void CloseTea() {
            notifyIcon.Visible = false;
            Application.Exit();
        }

        private void OnExitClicked(object sender, EventArgs e) {
            CloseTea();
        }

        private void OnTimerTick(object sender, EventArgs args) {
            notifyIcon.BalloonTipClicked += OnBallonTipClicked;
            notifyIcon.Text = Resources.KettleBoiling;

            ShowRemainedTimeMessage(0);

            notifyTimer.Stop();
            updateTitleTimer.Stop();
        }

        private void OnUpdateTitle(object sender, EventArgs args) {
            int remainedSeconds = notifyTimer.Remained / 1000;
            notifyIcon.Text = GetRemainedTimeMessage(remainedSeconds);
        }

        private void OnBallonTipClicked(object sender, EventArgs args) {
            CloseTea();
        }
    }
}
