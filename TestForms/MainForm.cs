using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace TestFontForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Icon = System.Drawing.Icon.FromHandle(IconChar.Flag.ToBitmap(16, Color.Black).GetHicon());
        }

        private void iconButton1_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("You clicked me");
        }
    }
}
