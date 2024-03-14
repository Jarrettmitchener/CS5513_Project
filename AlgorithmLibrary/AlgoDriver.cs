using AlgorithmLibrary.Algorithms;
using AlgorithmLibrary.Datasets;

namespace AlgorithmLibrary
{
    //query parameters class to be passed to the algorithm driver
    //This includes all what should be needed for each algorithm
    public class QueryParameters
    {
        public List<int> datasets = new List<int>();
        public List<string> keywords = new List<string>();
        public int algorithmChosen { get; set;}


    }
    public class AlgoDriver
    {
        //main file that calls the other 3 algorithm files
        //this way each algorithm file is seperated
        

        public string Algorithm1()
        {
            //testing dataset handler
            //string path = "DisneylandReviews.csv";
            //string path = "oldies_60s_top_artists_tracks.csv";
            //string path = "humor_detection.csv";
            //string path = "docs_stage_2_parsed_text.csv";
            string path = "news_summary_more.csv";
            DataSetHandler dtHandler = new DataSetHandler();
            //string json = dtHandler.ConvertCsvToJson(path);
            //var test = dtHandler.getSpotifyOldiesDataset(path);
            //var test = dtHandler.getHumorDetectionDataset(path);
            //var test = dtHandler.getBBCTextsDataset(path);
            var test = dtHandler.getNewsArticleSummariesDataset(path);

            Algo1 algo = new Algo1();
            string[] res = algo.GetResult();
            return res[0];
        }
        public string Algorithm2()
        {
            Algo2 algo = new Algo2();
            return algo.GetResult();
        }
        public string Algorithm3()
        {
            Algo3 algo = new Algo3();
            return algo.GetResult();
        }
    }
}
