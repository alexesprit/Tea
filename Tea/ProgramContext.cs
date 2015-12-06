using System;
using System.Windows.Forms;
using Tea.Properties;

namespace Tea {
    class ProgramContext : ApplicationContext {
        private static int TITLE_UPDATE_INTERVAL = 1; // in seconds
        private static int NOTIFICATION_DELAY = 0; // default delay

        private NotifyIcon notifyIcon;
        private NotifyTimer notifyTimer;
        private Timer updateUiTimer;

        public ProgramContext() {
            SetupNotifyIcon();
            SetupNotifyTimer();

            ShowRemainedTimeMessage(Settings.Default.BoilDelay * 60);
        }

        private void SetupNotifyIcon() {
            notifyIcon = new NotifyIcon() {
                Text = Resources.AppName,
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem(Resources.MenuSetDelay, OnSetDelayClicked),
                    new MenuItem(Resources.MenuExit, OnExitClicked)
                }),
                Visible = true,
            };
        }

        private void SetupNotifyTimer() {
            notifyTimer = new NotifyTimer();
            notifyTimer.Interval = Settings.Default.BoilDelay * 60 * 1000;
            notifyTimer.Elapsed += OnTimerTick;
            notifyTimer.Start();

            updateUiTimer = new Timer();
            updateUiTimer.Interval = TITLE_UPDATE_INTERVAL * 1000;
            updateUiTimer.Tick += OnUpdateTitle;
            updateUiTimer.Start();
        }

        private void RestartNotifyTimer() {
            updateUiTimer.Stop();
            notifyTimer.Stop();

            notifyTimer.Interval = Settings.Default.BoilDelay * 60 * 1000;

            notifyTimer.Start();
            updateUiTimer.Start();
        }

        private void ShowRemainedTimeMessage(int remainedSeconds) {
            notifyIcon.ShowBalloonTip(
                NOTIFICATION_DELAY,
                Resources.AppName,
                GetRemainedTimeMessage(remainedSeconds),
                ToolTipIcon.Info
            );
        }

        private String GetRemainedTimeMessage(int remainedSeconds) {
            if (remainedSeconds >= 60) {
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

        private void OnSetDelayClicked(object sender, EventArgs e) {
            InputBox inputBox = new InputBox();
            inputBox.Value = (Settings.Default.BoilDelay).ToString();

            DialogResult dialogResult = inputBox.ShowDialog();
            if (dialogResult == DialogResult.OK) {
                int interval = int.Parse(inputBox.Value);
                if (interval != Settings.Default.BoilDelay) {
                    Settings.Default.BoilDelay = interval;
                    Settings.Default.Save();

                    RestartNotifyTimer();
                    ShowRemainedTimeMessage(interval * 60);
                }
            }
        }

        private void OnExitClicked(object sender, EventArgs e) {
            CloseTea();
        }

        private void OnTimerTick(object sender, EventArgs args) {
            notifyIcon.BalloonTipClosed += OnBallonTipClosed;
            notifyIcon.Text = Resources.KettleBoiling;

            notifyTimer.Stop();
            updateUiTimer.Stop();

            ShowRemainedTimeMessage(0);
        }

        private void OnUpdateTitle(object sender, EventArgs args) {
            int remainedSeconds = notifyTimer.Remained / 1000;
            notifyIcon.Text = GetRemainedTimeMessage(remainedSeconds);
        }

        private void OnBallonTipClosed(object sender, EventArgs args) {
            CloseTea();
        }
    }
}
