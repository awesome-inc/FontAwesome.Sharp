using FontAwesome.Sharp;

namespace TestFontAwesome
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
