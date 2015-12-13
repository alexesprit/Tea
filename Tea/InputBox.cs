using System.Windows.Forms;
using Tea.Properties;

namespace Tea {
    public partial class InputBox : Form {
        public string Value {
            get {
                return valueTextBox.Text;
            }
            set {
                valueTextBox.Text = value;
            }
        }

        public InputBox() {
            InitializeComponent();

            messageLabel.Text = Resources.DelaySetTip;
            Text = Resources.Settings;

            okButton.DialogResult = DialogResult.OK;
        }
    }
}
