using FontAwesome.Sharp;

namespace TestWpf
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel {Icon = IconChar.Apple};
        }
    }
}