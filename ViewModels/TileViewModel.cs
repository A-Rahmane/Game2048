using System.Windows.Media;
using Game2048.Models;

namespace Game2048.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        private Tile _tile;

        public int Value => _tile.Value;
        public int Row => _tile.Row;
        public int Column => _tile.Column;
        public bool IsNew => _tile.IsNew;
        public bool IsMerged => _tile.IsMerged;

        public string DisplayText => Value > 0 ? Value.ToString() : string.Empty;

        public Brush Background => Value switch
        {
            0 => new SolidColorBrush(Color.FromRgb(204, 192, 179)),
            2 => new SolidColorBrush(Color.FromRgb(238, 228, 218)),
            4 => new SolidColorBrush(Color.FromRgb(237, 224, 200)),
            8 => new SolidColorBrush(Color.FromRgb(242, 177, 121)),
            16 => new SolidColorBrush(Color.FromRgb(245, 149, 99)),
            32 => new SolidColorBrush(Color.FromRgb(246, 124, 95)),
            64 => new SolidColorBrush(Color.FromRgb(246, 94, 59)),
            128 => new SolidColorBrush(Color.FromRgb(237, 207, 114)),
            256 => new SolidColorBrush(Color.FromRgb(237, 204, 97)),
            512 => new SolidColorBrush(Color.FromRgb(237, 200, 80)),
            1024 => new SolidColorBrush(Color.FromRgb(237, 197, 63)),
            2048 => new SolidColorBrush(Color.FromRgb(237, 194, 46)),
            _ => new SolidColorBrush(Color.FromRgb(60, 58, 50))
        };

        public Brush Foreground => Value switch
        {
            2 or 4 => Brushes.DarkSlateGray,
            _ => Brushes.White
        };

        public TileViewModel(Tile tile)
        {
            _tile = tile;
        }

        public void UpdateTile(Tile tile)
        {
            _tile = tile;
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(DisplayText));
            OnPropertyChanged(nameof(Background));
            OnPropertyChanged(nameof(Foreground));
            OnPropertyChanged(nameof(IsNew));
            OnPropertyChanged(nameof(IsMerged));
        }
    }
}
