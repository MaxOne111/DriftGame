    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using BayatGames.SaveGameFree;
    using BayatGames.SaveGameFree.Serializers;

    public static class JsonSerializer
    {
        public static void Serialize(string _path, object _data)
        {
            using (FileStream _stream = new FileStream(_path, FileMode.OpenOrCreate))
            {
                SaveGameJsonSerializer _serializer = new SaveGameJsonSerializer();
                _serializer.Serialize(_data, _stream, Encoding.Default);
            }
        }

        public static T Deserialize<T>(string _path)
        {
            using (FileStream _stream = new FileStream(_path, FileMode.Open))
            {
                SaveGameJsonSerializer _serializer = new SaveGameJsonSerializer();
                T _data = _serializer.Deserialize<T>(_stream, Encoding.Default);
                return _data;
            }
        }
        
    }