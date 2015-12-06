using System.Windows.Forms;

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
            okButton.DialogResult = DialogResult.OK;
        }
    }
}
