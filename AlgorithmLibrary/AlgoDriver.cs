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


        public singleQueryResult Algorithm1(QueryParameters parameters)
        {
            Stopwatch sw = new Stopwatch();
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            sw.Start();
            Algo1 algo = new Algo1();
            var res = algo.GetResult(parameters.keywords, dataSetList);
            sw.Stop();

            List<textResult> result = new List<textResult>();

            foreach(string s in res)
            {
                result.Add(new textResult
                {
                    text = s,
                    dataset = ""
                });

            }

            singleQueryResult singleQueryResult = new singleQueryResult();
            singleQueryResult.foundResults = result;
            singleQueryResult.numOfResults = result.Count();
            singleQueryResult.searchtime = sw.Elapsed.TotalSeconds;
            return singleQueryResult;
        }
        public string Algorithm2()
        {
            Algo2 algo = new Algo2();
            return algo.GetResult();
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
