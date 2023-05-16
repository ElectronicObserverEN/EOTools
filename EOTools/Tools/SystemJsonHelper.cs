using EOTools.Models.KancolleApi;
using System;
using System.IO;
using System.Text.Json;

namespace EOTools.Tools;

public static class SystemJsonHelper
{
    /// <summary>
    /// Read a Kancolle json file and deserialize it<br></br>
    /// Separated method cause need to remove the svdata=
    /// </summary>
    /// <param name="_path"></param>
    public static T? ReadKCJson<T>(string _path)
    {
        try
        {
            string _text = File.ReadAllText(_path);

            // --- revome svdata=
            _text = _text[7..];

            BaseApi<T>? api = JsonSerializer.Deserialize<BaseApi<T>>(_text);

            if (api is null) return default;
            if (api.ApiData is null) return default;

            return api.ApiData;
        }
        catch (Exception _ex) when (_ex is FileNotFoundException || _ex is DirectoryNotFoundException)
        {
            return default;
        }
    }
}
