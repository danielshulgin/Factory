using System;
using System.Collections.Generic;
using System.IO;
using Factory.Domain;
using Newtonsoft.Json;

namespace Factory.DAL
{
    public class Database
    {
        public string file_name = @"db.json";

        private string GetFullPath(string fileName)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }
        
        public void Save(SessionDataViewModel sessionData)
        {
            string fullPath = GetFullPath(file_name);
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            string sessionDataJson = JsonConvert.SerializeObject(sessionData, settings);
            if (System.IO.File.Exists(fullPath) == true)
            {
                FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.Write(sessionDataJson);
                objWrite.Close();
            }
            else
            {
                FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                StreamWriter objWrite = new StreamWriter(fs);
                objWrite.Write(sessionDataJson);
                objWrite.Close();
            }
        }

        public SessionDataViewModel Load()
        {
            var fullPath = GetFullPath(file_name);
            string transportersJson = "";
            if (System.IO.File.Exists(fullPath) == true)
            {
                System.IO.StreamReader objReader;
                objReader = new StreamReader(fullPath);
                transportersJson = objReader.ReadToEnd();
                objReader.Close();
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var sessionData = JsonConvert.DeserializeObject<SessionDataViewModel>(transportersJson, settings);
            return sessionData;
        }
    }
}
