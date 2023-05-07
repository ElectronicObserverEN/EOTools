using System.IO;
using System.Text.Json;

namespace EOTools.Tools.Translations;

public abstract class TranslationBase
{
    public abstract void Initialize();

    public T? Load<T>(string path)
    {
        using StreamReader sr = new StreamReader(path);
        {
            var json = JsonSerializer.Deserialize<T>(sr.ReadToEnd());
            return json;
        }
    }
}
