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

        //  кольори шахівниці
        var lightColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#F0D9B5");
        var darkColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B58863");

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Button btn = new Button
                {
                    FontSize = 42, 
                    Tag = (r, c),
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(0),
                    VerticalContentAlignment = VerticalAlignment.Center, 
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    FontWeight = FontWeights.Bold 
                };
                
                // Розфарбування клітинок
                bool isWhiteSquare = (r + c) % 2 == 0;
                btn.Background = isWhiteSquare ? lightColor : darkColor;

                btn.Click += OnCellClick;

                
                // Чорні фігури (Зверху, рядки 0 і 1)
                if (r <= 1) 
                {
                    btn.Foreground = Brushes.Black; // Колір тексту - Чорний
                    if (r == 1) btn.Content = "♟"; // Пішак 
                    if (r == 0) btn.Content = GetPieceSymbol(c); // Інші фігури 
                }

                // Білі фігури (Знизу, рядки 6 і 7)
                if (r >= 6)
                {
                    btn.Foreground = Brushes.White; // Колір тексту - БІЛИЙ
                    if (r == 6) btn.Content = "♟"; // Пішак
                    if (r == 7) btn.Content = GetPieceSymbol(c);
                }

                _buttons[r, c] = btn;
                ChessBoardGrid.Children.Add(btn);
            }
        }
    }

    
    private string GetPieceSymbol(int column)
    {
        switch (column)
        {
            case 0: return "♜"; // Тура
            case 1: return "♞"; // Кінь
            case 2: return "♝"; // Слон
            case 3: return "♛"; // Ферзь
            case 4: return "♚"; // Король
            case 5: return "♝";
            case 6: return "♞";
            case 7: return "♜";
            default: return "";
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