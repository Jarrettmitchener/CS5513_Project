using AlgorithmLibrary.Algorithms;
using AlgorithmLibrary.Datasets;

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
        public string Algorithm2()
        {
            Algo2 algo = new Algo2();
            return algo.GetResult();
        }
        public string Algorithm3(QueryParameters parameters)
        {
            //builds the dataset bade on which datasets the user wants
            var dataSetList = _dtHandler.buildDatasetList(parameters.Dataset1, parameters.Dataset2, parameters.Dataset3, parameters.Dataset4, parameters.Dataset5);

            Algo3 algo = new Algo3();
            return algo.GetResult();
            //BoyerMoore algo = new BoyerMoore();
            BoyerMooreAdvanced algo = new BoyerMooreAdvanced();
            return algo.Search(keywords);
        }
    }
}
