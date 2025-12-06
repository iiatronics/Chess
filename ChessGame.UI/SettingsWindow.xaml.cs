using System.Windows;
using ChessGame.UI.Services; 

namespace ChessGame.UI;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        LoadSettingsToUI();

        SldVolume.ValueChanged += SldVolume_ValueChanged;
        
        ChkSound.Checked += (s, e) => AudioService.ToggleSound(true);
        ChkSound.Unchecked += (s, e) => AudioService.ToggleSound(false);
    }

    private void LoadSettingsToUI()
    {
        var settings = AppSettings.Current;

        ChkDarkTheme.IsChecked = settings.IsDarkTheme;
        ChkHints.IsChecked = settings.ShowHints;
        ChkSound.IsChecked = settings.IsSoundOn;
        SldVolume.Value = settings.Volume;


        AudioService.ToggleSound(settings.IsSoundOn);
        AudioService.SetVolume(settings.Volume / 100.0);
    }


    private void SldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {

        AudioService.SetVolume(e.NewValue / 100.0);
    }

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