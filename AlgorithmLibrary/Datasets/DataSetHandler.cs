using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlgorithmLibrary.Datasets
{
    public static class DataSetHandler
    {
        public static string ConvertCsvToJson(string fileName)
        {
            // Read all lines from the CSV file
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string str = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\" + fileName;
            string[] lines = File.ReadAllLines(str);

            // Extract the headers (types)
            string[] headers = lines[0].Split(',');

            // Create a list to hold JSON objects
            List<Dictionary<string, string>> jsonObjects = new List<Dictionary<string, string>>();

            // Iterate through the remaining lines to create JSON objects
            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                Dictionary<string, string> jsonObject = new Dictionary<string, string>();

                for (int j = 0; j < headers.Length; j++)
                {
                    jsonObject[headers[j]] = values[j];
                }

                jsonObjects.Add(jsonObject);
            }

            // Serialize the list of JSON objects to JSON
            return JsonSerializer.Serialize(jsonObjects, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
