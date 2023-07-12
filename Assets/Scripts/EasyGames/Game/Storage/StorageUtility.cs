using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EasyGames
{
    public static class StorageUtility
    {
        private static string GetPath(StoragePath storagePath)
        {
            return Application.persistentDataPath + "/" + storagePath + ".EasyGames";
        }
        
        public static void Save<T>(T value, StoragePath storagePath)
        {
            var filePath = GetPath(storagePath);
            var json = JsonUtility.ToJson(value);
            File.WriteAllTextAsync(filePath, json);
        }

        public static T Load<T>(StoragePath storagePath)
        {
            if (!Exist(storagePath))
                return default;
            var filePath = GetPath(storagePath);
            var json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }

        public static bool Exist(StoragePath storagePath)
        {
            return File.Exists(GetPath(storagePath));
        }

        public static void Delete(StoragePath storagePath)
        {
            if (!Exist(storagePath))
                return;
            var filePath = GetPath(storagePath);
            File.Delete(filePath);
        }
    }
}