using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using pkNX.Randomization;
using pkNX.Structures;

namespace pkNX.WinForms
{
    [Serializable]
    public class SharedSettings
    {
        public PersonalRandSettings Personal { get; set; } = new();
        public SpeciesSettings Species { get; set; } = new();
        public TrainerRandSettings Trainer { get; set; } = new();
        public MovesetRandSettings Move { get; set; } = new();
        public LearnSettings Learn { get; set; } = new();
    }

    public static class EditUtil
    {
        public static SharedSettings Settings { get; set; }

        public static void LoadSettings(GameVersion game)
        {
            string path = GetSettingsFileName(game);
            if (!File.Exists(path))
            {
                Settings = new SharedSettings();
                return;
            }

            using var file = File.OpenRead(path);
            var reader = new XmlSerializer(typeof(SharedSettings));
            try
            {
                Settings = (SharedSettings) reader.Deserialize(file) ?? new SharedSettings();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void SaveSettings(GameVersion game)
        {
            string path = GetSettingsFileName(game);
            using var file = File.Create(path);
            var writer = new XmlSerializer(typeof(SharedSettings));
            try
            {
                writer.Serialize(file, Settings);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                if (!File.Exists(path))
                    return;
                file.Close();
                File.Delete(path);
            }
        }

        private static string GetSettingsFileName(GameVersion game)
        {
            var path = Path.GetDirectoryName(Application.ExecutablePath);
            return Path.Combine(path, $"randsetting{game}.xml");
        }
    }
}
