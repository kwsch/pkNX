using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;
using pkNX.WinForms.Properties;

namespace pkNX.WinForms
{
    public static class FlatBufferConverter
    {
        static FlatBufferConverter()
        {
            // Clean FlatBuffer folder the first time the program needs the deserialize content.
            // Future program updates may revise the fbs definitions, and we only write from resources if it doesn't exist.
            if (!Directory.Exists(FlatPath))
                return;
            var files = Directory.GetFiles(FlatPath);
            foreach (var f in files)
                File.Delete(f);
        }

        public static T[] DeserializeFrom<T>(string[] files)
        {
            var result = new T[files.Length];
            for (int i = 0; i < result.Length; i++)
            {
                var file = files[i];
                result[i] = DeserializeFrom<T>(file);
            }
            return result;
        }

        public static byte[][] SerializeFrom<T>(T[] obj)
        {
            var result = new byte[obj.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                var file = obj[i];
                result[i] = SerializeFrom(file);
            }
            return result;
        }

        public static T DeserializeFrom<T>(string file)
        {
            GenerateJsonFromFile<T>(file);
            var text = ReadDeleteJsonFromFolder();
            var obj = JsonConvert.DeserializeObject<T>(text);
            Debug.Assert(obj != null);
            Debug.WriteLine($"Deserialized {Path.GetFileName(file)}");
            return obj;
        }

        public static T DeserializeFrom<T>(byte[] data)
        {
            var path = Path.GetTempFileName();
            File.WriteAllBytes(path, data);
            var ret = DeserializeFrom<T>(path);
            File.Delete(path);
            return ret;
        }

        public static byte[] SerializeFrom<T>(T obj)
        {
            GenerateBinFrom(obj);
            var fileName = typeof(T).Name + ".bin";
            var data = ReadDelete(fileName);
            Debug.Assert(data.Length != 0);
            Debug.WriteLine($"Serialized to {fileName}");
            return data;
        }

        private static string ReadDeleteJsonFromFolder()
        {
            var jsonPath = Directory.GetFiles(FlatPath, "*.json")[0];
            var text = File.ReadAllText(jsonPath);
            File.Delete(jsonPath);
            return text;
        }

        private static byte[] ReadDelete(string fileName)
        {
            var filePath = Path.Combine(FlatPath, fileName);
            var data = File.ReadAllBytes(filePath);
            File.Delete(filePath);
            return data;
        }

        private static void GenerateJsonFromFile<T>(string file)
        {
            var name = typeof(T).Name;
            var fbsName = name + ".fbs";
            var fbsPath = Path.Combine(FlatPath, fbsName);
            Directory.CreateDirectory(FlatPath);
            if (!File.Exists(fbsPath))
                File.WriteAllBytes(fbsPath, GetSchema(name));

            var fileName = Path.GetFileName(file);
            var filePath = Path.Combine(FlatPath, fileName);
            File.Copy(file, filePath, true);

            var args = GetArgumentsDeserialize(fileName, fbsName);
            RunFlatC(args);
            File.Delete(filePath);
        }

        private static void GenerateBinFrom<T>(T obj)
        {
            var fbsName = typeof(T).Name + ".fbs";
            var jsonName = typeof(T).Name + ".json";
            var text = WriteJson(obj);

            var jsonPath = Path.Combine(FlatPath, jsonName);
            File.WriteAllText(jsonPath, text);
            var args = GetArgumentsSerialize(jsonName, fbsName);
            RunFlatC(args);
            File.Delete(jsonPath);
        }

        private static void RunFlatC(string args)
        {
            var fcp = Path.Combine(FlatPath, "flatc.exe");
            if (!File.Exists(fcp))
                File.WriteAllBytes(fcp, Resources.flatc);

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = FlatPath,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe",
                    Arguments = $"/C flatc {args} & exit",
                }
            };
            process.Start();
            process.WaitForExit();
        }

        public static string WriteJson<T>(T obj)
        {
            var serializer = new JsonSerializer();
            using var stringWriter = new StringWriter();
            using var writer = new JsonTextWriter(stringWriter)
            {
                QuoteName = false,
                Formatting = Formatting.Indented,
                IndentChar = ' ',
                Indentation = 2
            };
            serializer.Serialize(writer, obj);
            return stringWriter.ToString();
        }

        public static readonly string WorkingDirectory = Application.StartupPath;
        public static readonly string FlatPath = Path.Combine(WorkingDirectory, "flatbuffers");

        public static byte[] GetSchema(string name)
        {
            var obj = Resources.ResourceManager.GetObject(name);
            if (!(obj is byte[] b))
                throw new FileNotFoundException(nameof(name));
            return b;
        }

        private static string GetArgumentsDeserialize(string file, string fbs) =>
            $"-t {fbs} -- {file} --defaults-json --raw-binary";

        private static string GetArgumentsSerialize(string file, string fbs) =>
            $"-b {fbs} {file} --defaults-json";
    }
}
