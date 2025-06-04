using Game2048.Models;
using Game2048.Services;
using Game2048.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Game2048.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();

        var gameService = new GameService();
        _viewModel = new MainViewModel(gameService);
        DataContext = _viewModel;
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        Direction? direction = e.Key switch
        {
            Key.Up => Direction.Up,
            Key.Down => Direction.Down,
            Key.Left => Direction.Left,
            Key.Right => Direction.Right,
            _ => null
        };

        if (direction.HasValue)
        {
            _viewModel.MoveCommand.Execute(direction.Value);
            e.Handled = true;
        }
    }
}