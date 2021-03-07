using FontAwesome.Sharp;

namespace TestWpf
{
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel
            {
                Icon = IconChar.Apple
            };
        }
    }
}
