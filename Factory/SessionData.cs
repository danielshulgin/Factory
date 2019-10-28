using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Factory.Domain;


class SessionData
{
    public List<Detail> detailNames = new List<Detail>();
    public List<MachineType> machineTypes = new List<MachineType>();

    private static string fileName = "SessionDataJSON.txt";

    public void Save()
    {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        string path = GetFullPath();
        string createText = json + Environment.NewLine;
        File.WriteAllText(path, createText);
    }

    private static string GetFullPath()
    {
        string ad = Directory.GetCurrentDirectory();
        string path = System.IO.Path.Combine(ad, fileName);
        return path;
    }

    public static SessionData Load()
    {
        string path = GetFullPath();
        string readText = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<SessionData>(readText);
    }

    public void SaveDetails(List<string> detailsNames)
    {
        string json = JsonConvert.SerializeObject(detailsNames, Formatting.Indented);
        //string path = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\";
        string ad = Directory.GetCurrentDirectory();
        string path = System.IO.Path.Combine(ad, "SessionDataJSON.txt");

        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            string createText = json + Environment.NewLine;
            File.WriteAllText("SessionDataJSON.txt", createText);
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        //string appendText = "This is extra text" + Environment.NewLine;
        //File.AppendAllText(path, appendText);

        // Open the file to read from.
        //string readText = File.ReadAllText(path);
        //Console.WriteLine(readText);
    }
}

