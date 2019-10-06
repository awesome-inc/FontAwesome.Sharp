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
        private int _currentIcon;

        public MainViewModel()
        {
            OpenCommand = new DelegateCommand(DoOpen);
            PlayCommand = new DelegateCommand(DoPlay);
            ToggleSpinCommand = new DelegateCommand(ToggleSpin);

            // create a new icon block to show there is no memory leak
            _timer = new Timer(IterateIcons, null, 1000, 100);
        }

        private void IterateIcons(object state)
        {
            // creating a new icon block should be on UI thread ;-)
            Application.Current.Dispatcher?.Invoke(() =>
            {
                var icon = IconHelper.Icons[_currentIcon];
                IconBlock = null;
                GC.Collect();
                IconBlock = new IconBlock { Icon = icon, Height = 32, FontSize = 24 };
                OnPropertyChanged(nameof(IconBlock));
                _currentIcon = (_currentIcon + 1) % IconHelper.Icons.Length;
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
