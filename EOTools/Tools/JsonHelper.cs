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
                Indentation = 1,
                IndentChar = '\t'
            })
            {
                JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();
                _jsonSerializer.Serialize(_jsonTextWriter, _data);
            }

        } 
        
        /// <summary>
        /// Write object to a json file
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_data"></param>
        public static void WriteJsonByOnlyIndentingOnceWidePeepoHappy(string _path, object _data)
        {
            using (var _fileStream = File.Create(_path))
            using (var _streamWriter = new StreamWriter(_fileStream))
            using (var _jsonTextWriter = new CustomIndentingJsonTextWriter(_streamWriter)
            {
                Formatting = Formatting.Indented,
                Indentation = 1,
                IndentChar = '\t',
                MaxIndentDepth = 1
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
        public static JObject ReadJsonObject(string _path)
        {
            return ReadJson(_path) as JObject;
        }

        /// <summary>
        /// Read json file and deserialize it
        /// </summary>
        /// <param name="_path"></param>
        private static JToken ReadJson(string _path)
        {
            try
            {
                string _text = File.ReadAllText(_path);
                return ReadJsonFromString(_text);
            }
            catch (Exception _ex) when (_ex is FileNotFoundException || _ex is DirectoryNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Read json file and deserialize it
        /// </summary>
        /// <param name="_path"></param>
        public static JToken ReadJsonFromString(string _text)
        {
            return (JToken)JsonConvert.DeserializeObject(_text);
        }

        /// <summary>
        /// Read json file and deserialize it
        /// </summary>
        /// <param name="_path"></param>
        public static JArray ReadJsonArray(string _path)
        {
            return ReadJson(_path) as JArray;
        }

        /// <summary>
        /// Read a Kancolle json file and deserialize it<br></br>
        /// Separated method cause need to remove the svdata=
        /// </summary>
        /// <param name="_path"></param>
        public static JObject ReadKCJson(string _path)
        {
            try
            {
                string _text = File.ReadAllText(_path);

                // --- revome svdata=
                _text = _text.Substring(7, _text.Length - 7);


                _text = _text.Substring(0, _text.Length - 6);

                return (JObject)JsonConvert.DeserializeObject(_text);
            }
            catch (Exception _ex) when (_ex is FileNotFoundException || _ex is DirectoryNotFoundException)
            {
                return null;
            }
        }
    }
}
