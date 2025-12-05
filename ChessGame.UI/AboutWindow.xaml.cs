using System.Windows;

namespace ChessGame.UI;

public partial class AboutWindow : Window
{
    public AboutWindow() => InitializeComponent();
    private void BtnClose_Click(object sender, RoutedEventArgs e) => this.Close();
}