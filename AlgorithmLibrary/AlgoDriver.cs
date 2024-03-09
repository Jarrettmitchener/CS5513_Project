using AlgorithmLibrary.Algorithms;

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
