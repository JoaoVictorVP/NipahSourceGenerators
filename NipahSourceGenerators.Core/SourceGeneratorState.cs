using Newtonsoft.Json;

namespace NipahSourceGenerators.Core;

public static class SourceGeneratorState
{
    static Dictionary<string, object> state = new Dictionary<string, object>(32);

    public static void Set(string key, object value) => state[key] = value;
    public static object Get(string key) => state[key];
    public static T Get<T>(string key) => (T)state[key];

    static string getId()
    {
        return AppDomain.CurrentDomain.FriendlyName;
    }
    static string getDir() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NIPAH-SOURCE-GENERATORS");

    public static void MakePersistent()
    {
        string json = JsonConvert.SerializeObject(state);
        string dir = getDir();
        string path = Path.Combine(dir, getId() + ".nsgDat");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        File.WriteAllText(path, json);
    }
    public static void LoadPersistentState()
    {
        string path = Path.Combine(getDir(), getId() + ".nsgDat");
        if (!File.Exists(path))
            return;
        string json = File.ReadAllText(path);

        state = JsonConvert.DeserializeObject<Dictionary<string, object>>(json)!;
    }
    static SourceGeneratorState()
    {
        var def = JsonConvert.DefaultSettings();
        if (def == null) def = new JsonSerializerSettings();
        JsonConvert.DefaultSettings = () =>
        {
            def.TypeNameHandling = TypeNameHandling.Auto;
            return def;
        };
    }
}