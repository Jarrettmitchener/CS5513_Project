using AlgorithmLibrary.Algorithms;
using AlgorithmLibrary.Datasets;
using System.Diagnostics;

namespace AlgorithmLibrary
{
    //query parameters class to be passed to the algorithm driver
    //may add more to this later in development
    public class QueryParameters
    {
        public List<string> keywords = new List<string>();
        public bool Dataset1 { get; set; }
        public bool Dataset2 { get; set; }
        public bool Dataset3 { get; set; }
        public bool Dataset4 { get; set; }
        public bool Dataset5 { get; set; }


    }
    public class textResult
    {
        public string text;
        public string dataset;
    }
    public class singleQueryResult
    {
        public List<textResult> foundResults { get; set; }
        public int numOfResults { get; set; }
        public double searchtime { get; set; }

        public singleQueryResult()
        {
            numOfResults = 0;

        }
    }

    public class AlgoDriver
    {
        //main file that calls the other 3 algorithm files
        //this way each algorithm file is seperated

        DataSetHandler _dtHandler;
        public AlgoDriver() 
        {
            //creates dataset handler upon driver creation
            _dtHandler = new DataSetHandler();
        }


        public string Algorithm1()
        {

            Algo1 algo = new Algo1();
            string[] res = algo.GetResult();
            return res[0];
        }
        public singleQueryResult Algorithm2(QueryParameters parameters)
        {
            Stopwatch sw = new Stopwatch();
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            sw.Start();
            Algo2 algo = new Algo2();
            var res = algo.GetResult(parameters.keywords, dataSetList);
            sw.Stop();

            singleQueryResult singleQueryResult = new singleQueryResult();
            singleQueryResult.foundResults = res;
            singleQueryResult.numOfResults = res.Count();
            singleQueryResult.searchtime = sw.Elapsed.TotalSeconds;
            return singleQueryResult;
        }
        public singleQueryResult BoyerMooreAlgorithm(QueryParameters parameters)
        {
            Stopwatch sw = new Stopwatch();
            
            //builds the dataset bade on which datasets the user wants
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            //starts the stopwatch
            sw.Start();
            BoyerMoore boyerMoore = new BoyerMoore();
            var results = boyerMoore.Search(parameters.keywords, dataSetList);
            //stops the stopwatch
            sw.Stop();

            singleQueryResult singleQueryResult = new singleQueryResult();
            singleQueryResult.foundResults = results;
            singleQueryResult.numOfResults = results.Count();
            singleQueryResult.searchtime = sw.Elapsed.TotalSeconds;

            return singleQueryResult;
        }
        public singleQueryResult BoyerMooreAdvancedAlgorithm(QueryParameters parameters)
        {
            Stopwatch sw = new Stopwatch();

            //builds the dataset bade on which datasets the user wants
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            sw.Start();
            BoyerMooreAdvanced boyerMooreAdvanced = new BoyerMooreAdvanced();
            var results = boyerMooreAdvanced.Search(parameters.keywords, dataSetList);
            sw.Stop();

            singleQueryResult singleQueryResult = new singleQueryResult();
            singleQueryResult.foundResults = results;
            singleQueryResult.numOfResults = results.Count();
            singleQueryResult.searchtime = sw.Elapsed.TotalSeconds;

            return singleQueryResult;
        }
    }
}
