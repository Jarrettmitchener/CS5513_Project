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

    public class StatSearchVM
    {
        public string disneyKeyword { get; set; }
        public string BBCKeyword { get; set; }
        public string humorKeyword { get; set; }
        public string newsKeyword { get; set; }
        public string spotifyKeyword { get; set; }

        public int q_value1 { get; set; }
        public int q_value2 { get; set; }

    }
    //result for a single comparative query
    public class comparativeQueryResult
    {
        public double bmTime { get; set; }
        public int bmTotalResults { get; set; }
        public List<textResult> bmResults { get; set; }
        public double bmaTime { get; set; }
        public int bmaTotalResults { get; set; }
        public List<textResult> bmaResults { get; set; }

        public double kmpTime { get; set; }
        public int kmpTotalResults { get; set; }
        public List<textResult> kmpResults { get; set; }
        public double kmpaTime { get; set; }
        public int kmpaTotalResults { get; set; }
        public List<textResult> kmpaResults { get; set; }


        public double topKTime1 { get; set; }
        public int topKq_value1 { get; set; }
        public List<textResult> topkResults1 { get; set; }
        public double topKTime2 { get; set; }
        public int topKq_value2 { get; set; }
        public List<textResult> topkResults2 { get; set; }

        public string keyword { get; set; }

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


        public singleQueryResult TopKAlgorithm(QueryParameters parameters, int q_value)
        {
            Stopwatch sw = new Stopwatch();
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            sw.Start();
            TopK topk = new TopK();
            var result = topk.GetResult(parameters.keywords, dataSetList, q_value);
            sw.Stop();


            singleQueryResult singleQueryResult = new singleQueryResult();
            singleQueryResult.foundResults = result;
            singleQueryResult.numOfResults = result.Count();
            singleQueryResult.searchtime = sw.Elapsed.TotalSeconds;
            return singleQueryResult;
        }

        //KMP Algorithm without advanced selected.
        public singleQueryResult KMPAlgorithm(QueryParameters parameters)
        {
            Stopwatch sw = new Stopwatch();
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            sw.Start();
            KMP kmp = new KMP();
            var res = kmp.GetResult(parameters.keywords, dataSetList);
            sw.Stop();

            singleQueryResult singleQueryResult = new singleQueryResult();
            singleQueryResult.foundResults = res;
            singleQueryResult.numOfResults = res.Count();
            singleQueryResult.searchtime = sw.Elapsed.TotalSeconds;
            return singleQueryResult;
        }

        //KMP algorithm if advanced is selected. This will sort the strings to show first the highest search occurances.
        //TODO: Implement advanced algorithm.
        public singleQueryResult KMPAlgorithmAdvanced(QueryParameters parameters)
        {
            Stopwatch sw = new Stopwatch();
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            sw.Start();
            KMPAdvanced kmpAdvanced = new KMPAdvanced();
            var res = kmpAdvanced.GetResult(parameters.keywords, dataSetList);
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

        public List<comparativeQueryResult> StatisticalAnalysis(StatSearchVM vm)
        {

            //DEBUG VARIABLES
            //var DISNEYWORD = "food";
            //var BBCWORD = "ferrari";
            //var HUMORWORD = "genetic";
            //var NEWSWORD = "Boeing";
            //var SPOTIFYWORD = "love";

            List<string> keywordList = new List<string>();
            keywordList.Add(vm.disneyKeyword);
            keywordList.Add(vm.BBCKeyword);
            keywordList.Add(vm.humorKeyword);
            keywordList.Add(vm.newsKeyword);
            keywordList.Add(vm.spotifyKeyword);
            var QVALUE1 = vm.q_value1;
            var QVALUE2 = vm.q_value2;

            //required objects for searches
            BoyerMoore boyerMoore = new BoyerMoore();
            BoyerMooreAdvanced boyerMooreAdvanced = new BoyerMooreAdvanced();

            KMP kmp = new KMP();
            KMPAdvanced kmpAdvanced = new KMPAdvanced();

            Stopwatch sw = new Stopwatch();

            //result that will be used to store all of our results
            List<comparativeQueryResult> queryResults = new List<comparativeQueryResult>();

            //assigns the keyword for the next view

            for(int i = 0; i < keywordList.Count; i++)
            {
                comparativeQueryResult result = new comparativeQueryResult();
                //this is an array of one to be compatable with our algorithms
                var keyword = new List<string>
                {
                    keywordList[i],
                };
                //gets dataset depending on iteration
                var currDataset = getDataset(i);
                result.keyword = assignKeywrod(i, vm);

                //boyer moore
                sw.Restart();
                sw.Start();
                var bm = boyerMoore.Search(keyword, currDataset);
                sw.Stop();
                result.bmTime = sw.Elapsed.TotalSeconds;
                result.bmTotalResults = bm.Count;
                result.bmResults = bm.Take(3).ToList();

                //boyer moore advanced
                sw.Restart();
                sw.Start();
                var bma = boyerMooreAdvanced.Search(keyword, currDataset);
                sw.Stop();
                result.bmaTime = sw.Elapsed.TotalSeconds;
                result.bmaTotalResults = bma.Count;
                result.bmaResults = bma.Take(3).ToList();

                //KMP 
                sw.Restart();
                sw.Start();
                var km = kmp.GetResult(keyword, currDataset);
                sw.Stop();
                result.kmpTime = sw.Elapsed.TotalSeconds;
                result.kmpTotalResults = km.Count;
                result.kmpResults = km.Take(3).ToList();

                //KMP advanced
                sw.Restart();
                sw.Start();
                var kma = kmpAdvanced.GetResult(keyword, currDataset);
                sw.Stop();
                result.kmpaTime = sw.Elapsed.TotalSeconds;
                result.kmpaTotalResults = kma.Count;
                result.kmpaResults = kma.Take(3).ToList();



                //TopK second q value
                TopK topk1 = new TopK();
                sw.Restart();
                sw.Start();
                var tk1 = topk1.GetResult(keyword, currDataset, QVALUE1);
                sw.Stop();
                result.topKTime1 = sw.Elapsed.TotalSeconds;
                result.topkResults1 = tk1.Take(3).ToList();
                result.topKq_value1 = QVALUE1;

                //TopK second q value
                TopK topk2 = new TopK();
                sw.Restart();
                sw.Start();
                var tk2 = topk2.GetResult(keyword, currDataset, QVALUE2);
                sw.Stop();
                result.topKTime2 = sw.Elapsed.TotalSeconds;
                result.topkResults2 = tk2.Take(3).ToList();
                result.topKq_value2 = QVALUE2;

                queryResults.Add(result);
            }
            return queryResults;

        }
        //builds the correct dataset depending on what stage the loop is
        private List<DatasetObject> getDataset(int i)
        {
            List<DatasetObject> datasetObjects = new List<DatasetObject>();
            switch (i)
            {
                case 0:
                    datasetObjects = _dtHandler.buildDatasetList(true, false, false, false, false);
                    break;

                case 1:
                    datasetObjects = _dtHandler.buildDatasetList(false, true, false, false, false);
                    break;

                case 2:
                    datasetObjects = _dtHandler.buildDatasetList(false, false, true, false, false);
                    break;

                case 3:
                    datasetObjects = _dtHandler.buildDatasetList(false, false, false, true, false);
                    break;

                case 4:
                    datasetObjects = _dtHandler.buildDatasetList(false, false, false, false, true);
                    break;
            }

            return datasetObjects;
        }

        private string assignKeywrod(int i, StatSearchVM vm)
        {
            string keyword = "";
            switch (i)
            {
                case 0:
                    keyword = vm.disneyKeyword;
                    break;

                case 1:
                    keyword = vm.BBCKeyword;
                    break;

                case 2:
                    keyword = vm.humorKeyword;
                    break;

                case 3:
                    keyword = vm.newsKeyword;
                    break;

                case 4:
                    keyword = vm.spotifyKeyword;
                    break;
            }

            return keyword;
        }
    }
}
