using System.IO;
using Newtonsoft.Json;

namespace ChessGame.UI.Services;

// які є налаштування
public class AppSettingsData
{
    public bool IsSoundOn { get; set; } = true;
    public bool IsDarkTheme { get; set; } = false;
    public bool ShowHints { get; set; } = true;
    public double Volume { get; set; } = 50;
}

//  збереження та завантаження
public static class AppSettings
{
    private static readonly string FilePath = "settings.json";
    
    //актуальні налаштування
    public static AppSettingsData Current { get; private set; }
 
    static AppSettings()
    {
        Load();
    }

    public static void Save()
    {
        string json = JsonConvert.SerializeObject(Current, Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }

    public static void Load()
    {
        if (File.Exists(FilePath))
        {
            try 
            {
                string json = File.ReadAllText(FilePath);
                Current = JsonConvert.DeserializeObject<AppSettingsData>(json) ?? new AppSettingsData();
            }
            catch 
            {
                Current = new AppSettingsData(); 
            }
        }
        else
        {
            Current = new AppSettingsData(); 
        }
    }
}