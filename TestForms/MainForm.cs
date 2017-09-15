using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace TestForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Icon = System.Drawing.Icon.FromHandle(IconChar.Flag.ToBitmap(16, Color.Black).GetHicon());

            InitTreeIcons();

            boxOctocat.MouseHover += BoxOctocat_MouseHover;
            boxOctocat.MouseLeave += BoxOctocat_MouseLeave;
        }

        private void BoxOctocat_MouseLeave(object sender, EventArgs e)
        {
            boxOctocat.ForeColor = Color.Black;
        }

        private void BoxOctocat_MouseHover(object sender, EventArgs e)
        {
            boxOctocat.ForeColor = Color.Aquamarine;
        }

        private void InitTreeIcons()
        {
            var icons = Enum.GetValues(typeof (IconChar))
                .OfType<IconChar>()
                .Skip(1)
                .Take(10)
                .ToArray();

            imageList1.AddIcons(16, Color.Black, icons);

            var count = 0;
            SetTreeViewIcons(ref count, treeView1.Nodes);
        }

        private void SetTreeViewIcons(ref int i, IEnumerable nodes)
        {
            foreach (TreeNode node in nodes)
            {
                var imageKey = imageList1.Images.Keys[i % imageList1.Images.Count];
                node.ImageKey = node.SelectedImageKey = imageKey;
                i++;
                SetTreeViewIcons(ref i, node.Nodes);
            }
        }

        private void iconButton1_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("You clicked me");
        }
    }
}
