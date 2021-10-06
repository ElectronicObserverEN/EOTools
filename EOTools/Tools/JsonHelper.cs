using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace EOTools.Tools
{
    /// <summary>
    /// Helper to write and read json stuff
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Write object to a json file
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_data"></param>
        public static void WriteJson(string _path, object _data)
        {
            using (var _fileStream = File.Create(_path))
            using (var _streamWriter = new StreamWriter(_fileStream))
            using (var _jsonTextWriter = new JsonTextWriter(_streamWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 2,
                IndentChar = '\t'
            })
            {
                JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();
                _jsonSerializer.Serialize(_jsonTextWriter, _data);
            }

        }

        /// <summary>
        /// Read json file and deserialize it
        /// </summary>
        /// <param name="_path"></param>
        public static JObject ReadJson(string _path)
        {
            try
            {
                return (JObject)JsonConvert.DeserializeObject(File.ReadAllText(_path));
            }
            catch (Exception _ex) when (_ex is FileNotFoundException || _ex is DirectoryNotFoundException)
            {
                return null;
            }
        }
    }
}
