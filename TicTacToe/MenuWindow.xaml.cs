using System.Windows;

namespace TicTacToe;

public partial class MenuWindow : Window
{
    public MenuWindow()
    {
        InitializeComponent();
    }

    public void Start_Game_With_Player(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow(false);
        mainWindow.Show();
        mainWindow.BlockField();
        Close();
    }
    
    public void Start_Game_With_PC(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow(true);
        mainWindow.Show();
        mainWindow.BlockField();
        Close();
    }
}