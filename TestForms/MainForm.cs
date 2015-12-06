using System.Windows.Forms;

namespace TestFontForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void iconButton1_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("You clicked me");
        }
    }
}
