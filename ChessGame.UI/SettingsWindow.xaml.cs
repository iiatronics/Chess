using System.Windows;
using ChessGame.UI.Services; 

namespace ChessGame.UI;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        LoadSettingsToUI();
    }

    
    private void LoadSettingsToUI()
    {
        var settings = AppSettings.Current;

        ChkDarkTheme.IsChecked = settings.IsDarkTheme;
        ChkHints.IsChecked = settings.ShowHints;
        ChkSound.IsChecked = settings.IsSoundOn;
        SldVolume.Value = settings.Volume;
    }

    // збереження змін
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var settings = AppSettings.Current;

        settings.IsDarkTheme = ChkDarkTheme.IsChecked == true;
        settings.ShowHints = ChkHints.IsChecked == true;
        settings.IsSoundOn = ChkSound.IsChecked == true;
        settings.Volume = SldVolume.Value;

        AppSettings.Save();

        MessageBox.Show("Changes are saved");
        this.Close();
    }
}