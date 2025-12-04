using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using Newtonsoft.Json; 

namespace ChessGame.UI;

public partial class MainWindow : Window
{
    private readonly Button[,] _buttons = new Button[8, 8];

    public MainWindow()
    {
        InitializeComponent();
        DrawBoard();
    }

    private void DrawBoard()
    {
        ChessBoardGrid.Children.Clear();
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Button btn = new Button
                {
                    FontSize = 32,
                    Tag = (r, c),
                    BorderThickness = new Thickness(0)
                };
                
                bool isWhite = (r + c) % 2 == 0;
                btn.Background = isWhite ? Brushes.NavajoWhite : Brushes.SaddleBrown;
                btn.Foreground = isWhite ? Brushes.Black : Brushes.White;
                
                btn.Click += OnCellClick; // Підписка на подію

                // Тестові фігури
                if(r == 1) btn.Content = "♟";
                if(r == 6) btn.Content = "♙";

                _buttons[r, c] = btn;
                ChessBoardGrid.Children.Add(btn);
            }
        }
    }

    private void OnCellClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is (int r, int c))
        {
            string notation = $"{(char)('A' + c)}{8 - r}";
            MoveHistoryList.Items.Add($"Move: {notation}");
            MoveHistoryList.ScrollIntoView(MoveHistoryList.Items[^1]);
        }
    }

    private void SaveGame_Click(object sender, RoutedEventArgs e)
    {
        var data = new { History = MoveHistoryList.Items, Date = System.DateTime.Now };
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText("chess_save.json", json);
        MessageBox.Show("Game saved");
    }

    private void LoadGame_Click(object sender, RoutedEventArgs e)
    {
        if (File.Exists("chess_save.json"))
            MessageBox.Show($"File found\n{File.ReadAllText("chess_save.json")}");
        else
            MessageBox.Show("File not found");
    }

    private void BackToMenu_Click(object sender, RoutedEventArgs e)
    {
        new MainMenuWindow().Show();
        this.Close();
    }
}