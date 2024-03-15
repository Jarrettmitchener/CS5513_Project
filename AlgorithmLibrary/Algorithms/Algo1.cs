using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibrary.Algorithms
{
    public class Algo1
    {
        private int k = 10; // Number of results requested
        private double max_w; // Maximum weight of a string
        private int f = 1; // multiplication factor, may change depending on results
        private string[] R1 = new string[10]; // the range search result set
        private string[] init_set = new string[10]; // the range search result set
        private double theta = 0.5; // Similarity threshold, initially = 0.5
        private int q_value = 2; // Denotates q value for the gram calculation in the context
        private double alpha = 1.0;
        private double beta = 1.0;
        private double last_top_k_value = 0;

        // TEST VALUES
        private string query = "Madalena";

        public string[] GetResult()
        {
            init_set = [ "madero", "pez", "martillo",
                                    "madera", "MariaMadalena", "madalana",
                                    "mad", "mada", "madena", "masssana",
                                    "madera", "MariaMadalena", "madalana"];
            return run_2HP(query);
        }

        public string[] run_2HP(string s_query)
        {
            // Read data 
            //init_set = read_data();
            query = s_query;
            R1 = IRS();
            //R1 = SPS(R1, )
            return R1;
        }

        private string[] IRS()
        {

           string[] res = ["jajajajajja"];
            // First step : compute initial candidates.
            while (res.Length < f * k)
            {
                res = computeSmilarityJacquard();

                if (res.Length < f * k)
                {
                    theta = theta - 0.01;
                }

            }

            double min_theta = 0; 
            while( ComputeScore(min_theta, max_w) < ComputeScore(theta, last_top_k_value))
            {
                min_theta += 0.01; 
            }

            if( min_theta < theta)
            {
                //res = MergeSkip(init_set, min_theta);
                res = computeSmilarityJacquard();
            }
            return res;
        }


        private double ComputeScore(double jcq, double weight)
        {
            return alpha * jcq + beta * weight;
        }
        private string[] computeSmilarityJacquard()
        {
            double[] top_k = new double[k];
            string[] top_k_values = ["kk ", "kk ", "kk", "kk", "kk", "kk", "kk", "kk", "kk", "kk"];
            int count = 0;
            foreach (string s in init_set)
            {

                int num = 0;
                int den = query.Length + s.Length;
                for (int j = 0; j < query.Length - (q_value + 1); j++)
                {
                    string g2 = query.Substring(j, j + q_value - 1);
                    for (int i = 0; i < s.Length - (q_value +1) ; i++)
                    {
                        string g1 = s.Substring(i, i + q_value - 1);

                        if (g1 == g2) { num += 1; }
                    }
                }
                double jacquard = num / den; 

                if(jacquard > top_k[k-1] && jacquard > theta && count > k)
                {
                    for (int i = k - 2;i  >= 0; i-- )
                    {
                        if (jacquard > top_k[i])
                        {
                            top_k[i - 1] = top_k[i];
                            top_k_values[i - 1] = top_k_values[i]; ;
                            top_k[i] = jacquard;
                            top_k_values[i] = s;
                        }
                    }
                }
                else if(count < k && jacquard > theta)
                {

                    count += 1;
                }
                

            }
            last_top_k_value = top_k[k - 1];
            return top_k_values;
        }
    }
}
