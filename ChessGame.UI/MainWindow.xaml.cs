using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging; // для картинок
using System;
using System.IO;
using Newtonsoft.Json; 
using ChessGame.UI.Services;

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

        bool isDarkTheme = Services.AppSettings.Current.IsDarkTheme;

        SolidColorBrush lightColor, darkColor;

        if (isDarkTheme)
        {
            // темна тема
            lightColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#A9A9A9");
            darkColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#696969");
        }
        else
        {
            // світла тема
            lightColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#F0D9B5");
            darkColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#B58863");
        }

        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Button btn = new Button
                {
                    Tag = (r, c),
                    BorderThickness = new Thickness(0),
                    Padding = new Thickness(5),
                   
                    Background = (r + c) % 2 == 0 ? lightColor : darkColor
                };
                
                btn.Click += OnCellClick;

                // розстановка фігур 
                if (r == 1) btn.Content = GetPieceImage("pawn", false);
                if (r == 0) btn.Content = GetPieceImage(GetStartPieceName(c), false);
                if (r == 6) btn.Content = GetPieceImage("pawn", true);
                if (r == 7) btn.Content = GetPieceImage(GetStartPieceName(c), true);

                _buttons[r, c] = btn;
                ChessBoardGrid.Children.Add(btn);
            }
        }
    }
    
    private Image? GetPieceImage(string pieceName, bool isWhite)
    {
        string prefix = isWhite ? "w" : "b";
        string fileName = $"{prefix}_{pieceName}.png";

        // 1.  шлях до папки bin/Debug/..
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        
        // 2. пошук папки Images 
        string path = System.IO.Path.Combine(baseDir, "Images", fileName);

        if (!System.IO.File.Exists(path))
        {
            // Якщо не знайде, покаже, де саме шукав. Це допоможе нам зрозуміти.
            MessageBox.Show($"Файл не знайдено!\nШукав тут:\n{path}", "Помилка шляху");
            return null;
        }

        try
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path, UriKind.Absolute); 
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            Image img = new Image();
            img.Source = bitmap;
            img.Stretch = Stretch.Uniform;
            return img;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Помилка: {ex.Message}");
            return null;
        }
    }
    // допоміжний метод: повертає назву фігури 
    private string GetStartPieceName(int col)
    {
        switch (col)
        {
            case 0: case 7: return "rook";
            case 1: case 6: return "knight";
            case 2: case 5: return "bishop";
            case 3: return "queen";
            case 4: return "king";
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
            MessageBox.Show($"file found\n{File.ReadAllText("chess_save.json")}");
        else
            MessageBox.Show("File not found");
    }

    private void BackToMenu_Click(object sender, RoutedEventArgs e)
    {
        new MainMenuWindow().Show();
        this.Close();
    }
}