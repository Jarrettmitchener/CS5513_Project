using Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AlgorithmLibrary.Datasets
{
    //object that will return which dataset and the raw text from each dataset
    public class DatasetObject
    {
        public string Text { get; set; }
        public string Dataset { get; set; }
    }
    public class DataSetHandler
    {
        //inner function that returns the disneyLand reviews in DatasetObject list format
        private List<DatasetObject> getDisneyLandReviews(string filePath)
        {
            //reads text from file
            var csv = File.ReadAllText(filePath);

            List<DatasetObject> DisneyLandReviews = new List<DatasetObject>();
            //iterates each line and adds the csv data to the list
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                DisneyLandReviews.Add(new DatasetObject
                {
                    Text = line["Review_Text"],
                    Dataset = "Disney Land Reviews"
                }) ;
            }

            return DisneyLandReviews;
        }

        //inner function that returns the spotyify oldiess in DatasetObject list format
        private List<DatasetObject> getSpotifyOldies(string filePath)
        {
            //reads text from file
            var csv = File.ReadAllText(filePath);

            List<DatasetObject> spotifyOldies = new List<DatasetObject>();
            //iterates each line and adds the csv data to the list
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                spotifyOldies.Add(new DatasetObject
                {
                    Text = line["Track Name"],
                    Dataset = "Spotify oldies"
                });
            }

            return spotifyOldies;
        }

        //inner function that returns the whatsapp app store reviewss in DatasetObject list format
        private List<DatasetObject> getHumorDetection(string filePath)
        {
            //reads text from file
            var csv = File.ReadAllText(filePath);

            List<DatasetObject> dune2Reviews = new List<DatasetObject>();
            //iterates each line and adds the csv data to the list
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                dune2Reviews.Add(new DatasetObject
                {
                    Text = line["text"],
                    Dataset = "Humor Detection"
                });
            }

            return dune2Reviews;
        }

        //inner function that returns the BBC full text in DatasetObject list format
        private List<DatasetObject> getBBCtext(string filePath)
        {
            //reads text from file
            var csv = File.ReadAllText(filePath);

            List<DatasetObject> bbcTexts = new List<DatasetObject>();
            //iterates each line and adds the csv data to the list
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                bbcTexts.Add(new DatasetObject
                {
                    Text = line["DocText"],
                    Dataset = "BBC Texts"
                });
            }

            return bbcTexts;
        }

        //inner function that returns the news article summary in DatasetObject list format
        private List<DatasetObject> getNewsArticleText(string filePath)
        {
            //reads text from file
            var csv = File.ReadAllText(filePath);

            List<DatasetObject> articles = new List<DatasetObject>();
            //iterates each line and adds the csv data to the list
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                articles.Add(new DatasetObject
                {
                    Text = line["text"],
                    Dataset = "News Article Summaries"
                });
            }

            return articles;
        }




        public List<DatasetObject> getSpotifyOldiesDataset(string fileName)
        {
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\" + fileName;

            return getSpotifyOldies(filePath);
        }
        public List<DatasetObject> getHumorDetectionDataset(string fileName)
        {
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\" + fileName;

            return getHumorDetection(filePath);
        }

        public List<DatasetObject> getBBCTextsDataset(string fileName)
        {
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\" + fileName;

            return getBBCtext(filePath);
        }

        public List<DatasetObject> getNewsArticleSummariesDataset(string fileName)
        {
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\" + fileName;

            return getNewsArticleText(filePath);
        }

        public string ConvertCsvToJson(string fileName)
        {
            // Read all lines from the CSV file
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\" + fileName;
            string[] lines = File.ReadAllLines(filePath);

            //begining of the csv stuff
            var csv = File.ReadAllText(filePath);

            var test = getDisneyLandReviews(filePath);

            foreach(var line in CsvReader.ReadFromText(csv))
            {
                var firstCell = line[0];
                var byName = line["Review_Text"];
            }

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
