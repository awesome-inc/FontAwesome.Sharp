using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using Icon = System.Drawing.Icon;

namespace TestForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Icon = Icon.FromHandle(IconChar.Flag.ToBitmap(Color.Black).GetHicon());

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
            var icons = Enum.GetValues(typeof(IconChar))
                .OfType<IconChar>()
                .Skip(1)
                .Take(10)
                .ToArray();

            imageList1.AddIcons(Color.Black, IconHelper.DefaultSize, icons);

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

        private void iconButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You clicked me");
            timer1.Enabled = !timer1.Enabled;
            boxTwitter.Flip = NextFlip(boxTwitter.Flip);
        }

        private void _refreshMenuItem_Click(object sender, EventArgs e)
        {
            _rotateMenuItem.Rotation += 22.5;
            _rotateMenuItem.Text = $"Rotate ({_rotateMenuItem.Rotation}°)";
        }

        private void _flipMenuItem_Click(object sender, EventArgs e)
        {
            _flipMenuItem.Flip = NextFlip(_flipMenuItem.Flip);
            _flipMenuItem.Text = $"Flip ({_flipMenuItem.Flip})";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 180°/s -> 180 * interval / 1000.0
            var delta = 180.0 * timer1.Interval / 1000.0;
            boxTwitter.Rotation += delta;
        }

        private FlipOrientation NextFlip(FlipOrientation current)
        {
            var flip = (int)current + 1;
            flip %= Enum.GetValues(typeof(FlipOrientation)).Length;
            return (FlipOrientation)flip;
        }
    }
}
