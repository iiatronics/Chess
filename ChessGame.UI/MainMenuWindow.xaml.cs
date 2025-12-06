using System.Windows;
using ChessGame.UI.Services; 

namespace ChessGame.UI;

public partial class MainMenuWindow : Window
{
    public MainMenuWindow()
    {
        InitializeComponent();
        

        AudioService.PlayMusic(); 
    }

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show(); 
        this.Close();            
    }

    
    private void Settings_Click(object sender, RoutedEventArgs e) => new SettingsWindow().ShowDialog();
    
    private void About_Click(object sender, RoutedEventArgs e) => new AboutWindow().ShowDialog();
    
    private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
}