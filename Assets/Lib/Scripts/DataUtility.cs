using UnityEngine;
using System.IO;

namespace Kosu.UnityLibrary
{
    public static class DataUtility
    {
        public static void SaveDataToJson<T>(T obj, string savePath)
        {
            string dirPath = Path.GetDirectoryName(savePath);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(Application.dataPath + dirPath);
            }

            string json = JsonUtility.ToJson(obj);
            FileStream fs = File.Create(savePath);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(json);
            sw.Close();
            fs.Close();
        }

        public static T LoadDataFromJson<T>(string loadPath) where T : new()
        {
            if (!File.Exists(loadPath))
            {
                return new T();
            }

            FileStream fs = File.Open(loadPath, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            var json = JsonUtility.FromJson<T>(sr.ReadToEnd());
            sr.Close();
            fs.Close();
            return json;
        }
    }
}
