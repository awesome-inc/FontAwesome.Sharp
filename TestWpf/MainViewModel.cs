using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;

namespace TestWpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isSpinning = true;
        // ReSharper disable once NotAccessedField.Local
        private readonly Timer _timer;
        private readonly Random _random = new Random();

        public MainViewModel()
        {
            OpenCommand = new DelegateCommand(DoOpen);
            PlayCommand = new DelegateCommand(DoPlay);
            ToggleSpinCommand = new DelegateCommand(ToggleSpin);

            // create a new icon block to show there is no memory leak
            _timer = new Timer(OnIconBlockChange, null, 1000, 200);
        }

        private void OnIconBlockChange(object state)
        {
            // creating a new icon block should be on UI thread ;)
            Application.Current.Dispatcher.Invoke(() =>
            {
                var values = Enum.GetValues(typeof(IconChar));
                var rndIcon = (IconChar)values.GetValue(_random.Next(values.Length));
                IconBlock = null;
                GC.Collect();
                IconBlock = new IconBlock { Icon = rndIcon, Height = 80, FontSize = 72 };
                OnPropertyChanged(nameof(IconBlock));
            });
        }

        public IconChar Icon { get; set; }
        public ICommand OpenCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand ToggleSpinCommand { get; }

        public IconBlock IconBlock { get; set; }

        public bool IsSpinning
        {
            get => _isSpinning;
            set
            {
                if (_isSpinning == value) return;
                _isSpinning = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void DoOpen()
        {
            MessageBox.Show("Clicked Open", "Info");
        }

        private void DoPlay()
        {
            MessageBox.Show("Clicked Play", "Info");
        }

        private void ToggleSpin()
        {
            IsSpinning = !IsSpinning;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
