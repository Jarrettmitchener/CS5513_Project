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
        //aux function that just counts the average number of words in a document
        private int getAverageWords(string filePath, string header)
        {
            //reads text from file
            var csv = File.ReadAllText(filePath);
            int recordCount = 0;
            int numOfWords = 0;
            List<DatasetObject> articles = new List<DatasetObject>();
            //iterates each line and adds the csv data to the list
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                //gets the string from the line
                string str = line[header];
                string[] words = str.Split(new char[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                numOfWords += words.Length;
                recordCount++;
            }

            int averageWords = numOfWords / recordCount;
            return averageWords;
        }
        //inner function that returns the csv text as a list of entries where entries are the DatasetObject
        //filepath is filepath
        //header is the header used in the csv file where we want to extract
        //datasetName is the name we want to associate with this pull
        private List<DatasetObject> extractTextFromDataset(string filePath, string header, string datasetName)
        {
            //reads text from file
            var csv = File.ReadAllText(filePath);

            List<DatasetObject> articles = new List<DatasetObject>();
            //iterates each line and adds the csv data to the list
            foreach (var line in CsvReader.ReadFromText(csv))
            {
                //adds the object
                articles.Add(new DatasetObject
                {
                    Text = line[header],
                    Dataset = datasetName
                });
            }

            return articles;
        }

        //gets the average amount of words from a dataset
        //pretty much just made this for the quick dataset information on the WebUI, isn't present in the execution of the algorithms
        public int getAverageWordsFromDataset(string fileName, string header)
        {
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\" + fileName;

            return getAverageWords(filePath, header);
        }
        //builds the dataset return list based on which datasets the user wants to pull from
        //ds1 - disneyland
        //ds2 - BBC text documents
        //ds3 - humor texts
        //ds4 - news article summaries
        //ds5 - spotify oldies
        public List<DatasetObject> buildDatasetList(bool ds1,  bool ds2, bool ds3, bool ds4, bool ds5)
        {
            //path variables, this is needed because even though this file is in the same directory as the datasets, it 
            //sets the execution as the root, which will give us filepath errors. this fixes it
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string partialPath = solutiondir + "\\" + "CS5513_Project\\AlgorithmLibrary" + "\\Datasets\\";

            List<DatasetObject> datasetList = new List<DatasetObject>();
            //if statement case that builds the list based on what datasets the user wanted
            if(ds1 == true)
            {
                //combines the filepath with the associated csv file
                string filepath = partialPath + "DisneylandReviews.csv";
                //adds the list to the existing list
                datasetList.AddRange(extractTextFromDataset(filepath, "Review_Text", "Disney Land Reviews"));
            }
            if (ds2 == true)
            {
                string filepath = partialPath + "docs_stage_2_parsed_text.csv";
                datasetList.AddRange(extractTextFromDataset(filepath, "DocText", "BBC Text Documents"));
            }
            if (ds3 == true)
            {
                string filepath = partialPath + "humor_detection.csv";
                datasetList.AddRange(extractTextFromDataset(filepath, "text", "Humor Texts"));
            }
            if (ds4 == true)
            {
                string filepath = partialPath + "news_summary_more.csv";
                datasetList.AddRange(extractTextFromDataset(filepath, "text", "News Article Summaries"));
            }
            if (ds5 == true)
            {
                string filepath = partialPath + "oldies_60s_top_artists_tracks.csv";
                datasetList.AddRange(extractTextFromDataset(filepath, "Track Name", "Top Artists tracks from the 60s"));
            }
            return datasetList;
        }
        
    }
}
