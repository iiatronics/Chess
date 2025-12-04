using System.Windows;

namespace ChessGame.UI;

public partial class MainMenuWindow : Window
{
    public MainMenuWindow() => InitializeComponent();

    private void Start_Click(object sender, RoutedEventArgs e)
    {
        new MainWindow().Show(); // Відкриваємо гру
        this.Close();            // Закриваємо меню
    }

    private void Settings_Click(object sender, RoutedEventArgs e) => new SettingsWindow().ShowDialog();
    private void About_Click(object sender, RoutedEventArgs e) => new AboutWindow().ShowDialog();
    private void Exit_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
}