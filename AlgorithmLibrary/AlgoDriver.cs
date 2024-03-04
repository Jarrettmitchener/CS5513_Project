﻿using AlgorithmLibrary.Algorithms;

namespace AlgorithmLibrary
{
    public class AlgoDriver
    {
        //main file that calls the other 3 algorithm files
        //this way each algorithm file is seperated

        public string Algorithm1()
        {
            Algo1 algo = new Algo1();
            return algo.GetResult();
        }
        public string Algorithm2()
        {
            Algo2 algo = new Algo2();
            return algo.GetResult();
        }
        public List<string> Algorithm3(List<string> keywords)
        {
            BoyerMoore algo = new BoyerMoore();
            return algo.Search(keywords);
        }
    }
}
