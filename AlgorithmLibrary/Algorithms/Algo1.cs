using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibrary.Algorithms
{
    public class Algo1
    {
        private int k = 10; // Number of results requested
        private double max_w; // Maximum weight of a string
        private Dictionary<string, double> weights = new Dictionary<string, double>();
        private int f = 1; // multiplication factor, may change depending on results
        private string[] R1 = new string[10]; // the range search result set
        //private string[] init_set = new string[10]; // the range search result set
        private double theta = 0.0; // Similarity threshold, initially = 0.5
        private int q_value = 2; // Denotates q value for the gram calculation in the context
        private double alpha = 1.0;
        private double beta = 1.0;
        private double last_top_k_value = 0;
        private Dictionary<string, List<int>> grams = new Dictionary<string, List<int>>();
        private double g_min = 10;
        private int theta_min = 1;
        private Dictionary<int, string> index_elements = new Dictionary<int, string>();
        private string[] query_grams = new string[20];

        // TEST VALUES
        private string query = "macho es el mejor";

        //public IDictionary Weights { get => weights; set => weights = value; }

        public string[] GetResult()
        {
            weights["mata"] = 0.1;
            weights["madero"] = 0.1;
            weights["pez"] = 0.1;
            weights["martillo"] = 0.1;
            weights["madera"] = 0.1;
            weights["madalena"] = 0.1;
            weights["mad"] = 0.1;
            weights["mada"] = 0.1;
            weights["madena"] = 0.1;
            weights["masssana"] = 0.1;
            weights["panadera"] = 0.1;
            weights["panadero"] = 0.1;

            //elemnent_to_index 
            // read the wirghts by elemnt not by pharse. 
            // Compute the overall a¡value of the phrase.
            // Keep the max  added similarity within the phrase. 
            // Complexity -- Implemntation time ... Space to storage ond compute, 
            // Accuracy, Number of results
            // Are the results relevant to the search  ??? 

            index_elements[0] = "macho es";
            index_elements[1] = "mata";
            index_elements[2] = "madero";
            index_elements[3] = "pez";
            index_elements[4] = "martillo";
            index_elements[5] = "madera";
            index_elements[6] = "madalena";
            index_elements[7] = "mad";
            index_elements[8] = "mada";
            index_elements[9] = "madena";
            index_elements[10] = "masssana";
            index_elements[11] = "panadera";
            index_elements[12] = "panadero";
            index_elements[0] = "macho el mejor de todos";

            computeGrams();


            string s_query = query;

            return run_2HP(s_query);
        }

        private void computeGrams()
        {
            for (int j = 0; j < query.Length - q_value; j++)
            {
                string g1 = query.Substring(j, q_value);
                query_grams[j] = (g1);
                grams[g1] = new List<int>();
            }

            foreach (KeyValuePair<int, string> s in index_elements)
            {
                for (int j = 0; j < s.Value.Length - q_value; j++)
                {
                    string g1 = s.Value.Substring(j, q_value);
                    if (query_grams.Contains(g1))
                    {
                        grams[g1].Add(s.Key);
                    }

                }
            }
        }

        public string[] run_2HP(string s_query)
        {
            // Read data 
            //init_set = read_data();
            query = s_query;
            R1 = IRS();
            computeFrequency(last_top_k_value);
            R1 = SPS();
            return R1;
        }

        private void computeFrequency(double k)
        {
            weights.Order();
            double n_max = weights.First().Value;

            g_min = (k - beta * n_max) / alpha;
        }

        private string[] IRS()
        {

            string[] res = ["jajajajajja"];
            // First step : compute initial candidates.
            while (res.Length < f * k)
            {
                res = computeSmilarity();

                if (res.Length < f * k)
                {
                    theta = theta - 0.01;
                }

            }

            double min_theta = 0;
            while (ComputeScore(min_theta, max_w) < ComputeScore(theta, last_top_k_value))
            {
                min_theta += 0.01;
            }

            if (min_theta < theta)
            {
                //res = MergeSkip(init_set, min_theta);
                theta = min_theta;
                res = computeSmilarity();
            }
            System.Console.WriteLine(res);
            return res;
        }


        private double ComputeScore(double jcq, double weight)
        {
            return alpha * jcq + beta * weight;
        }



        private string[] computeSmilarity()
        {
            double[] top_k = [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0];
            string[] top_k_values = ["kk1 ", "kk2 ", "kk3", "kk4", "kk5", "kk6", "kk7", "kk8", "kk9", "kk10"];
            //int count = 0;
            foreach (KeyValuePair<string, double> s in weights)
            {

                double num = 0.0;
                double den = query.Length + s.Key.Length + 0.0;
                for (int j = 0; j < query.Length - q_value; j++)
                {
                    string g2 = query.Substring(j, q_value);
                    for (int i = 0; i < s.Key.Length - q_value; i++)
                    {
                        string g1 = s.Key.Substring(i, q_value);
                        if (g1.Equals(g2)) { num += 1.0; }
                    }
                }
                double jacquard = num / (double)den;
                double weight = s.Value;


                double score = ComputeScore(jacquard, weight);

                if (score > top_k[k - 1] && score > theta)
                {
                    top_k[k - 1] = score;
                    top_k_values[k - 1] = s.Key;
                    int i = k - 2;

                    while (score > top_k[i] && i > 0)
                    {
                        top_k[i + 1] = top_k[i];
                        top_k_values[i + 1] = top_k_values[i];
                        top_k[i] = score;
                        top_k_values[i] = s.Key;
                        i -= 1;

                    }
                    if (score > top_k[0])
                    {
                        top_k[i + 1] = top_k[0];
                        top_k_values[i + 1] = top_k_values[0];
                        top_k[0] = score;
                        top_k_values[0] = s.Key;
                    }

                }


            }
            //Jaquard[0] = top_k[0].ToString();
            last_top_k_value = top_k[k - 1];
            return top_k_values;
        }

        private string[] SPS()
        {
            double n_gram = (double)grams.Count; // Number of grams in the query     
            double g = 0; // Freuency thresold.
            List<int> top_K = new List<int>();
            int top = 0;
            Queue<int> H = new Queue<int>();// The lists of ids for the query grams.
            int indx = 0;
            string[] res = ["kk1 ", "kk2 ", "kk3", "kk4", "kk5", "kk6", "kk7", "kk8", "kk9", "kk10"];

            foreach (KeyValuePair<string, List<int>> s in grams)
            {
                if (s.Value.Count > 0)
                {
                    H.Enqueue(s.Value[indx]);
                }
            }

            for (int i = 0; i < H.Count(); i++)
            {
                int T = H.Dequeue();
                top_K.Add(T);
                int num = H.Count();
                int p = 0;
                for (int j = 0; j < num; j++)
                {
                    int element = H.Dequeue();
                    if (T.Equals(element))
                    {
                        p += 1;
                    }
                    else
                    {
                        H.Enqueue(element);
                    }
                }

                if (p >= g)
                {
                    if (top < k)
                    {
                        top++;
                        top_K.Add(T);
                    }
                    else if (p < top_K.Last())
                    {
                        top_K.Remove(top_K.Last());
                        top_K.Add(p);
                    }


                    if ((theta_min * n_gram) > ((n_gram + g_min) / 1 + (1 / theta_min)))
                    {
                        g = (theta_min * n_gram);
                    }
                    else
                    {
                        g = ((n_gram + g_min) / 1.0 + (1.0 / theta_min));
                    }

                    indx += 1;

                    foreach (KeyValuePair<string, List<int>> s in grams)
                    {
                        if (indx > (s.Value).Count)
                        {
                            if(s.Value.Any())
                                H.Enqueue(s.Value[indx]);
                        }
                    }
                }
                else
                {
                    for (int j = 0; (j < g - p - 1) && (H.Count > 0); j++)
                    {
                        H.Dequeue();
                    }

                    int T_prime = top_K[0];

                    foreach (KeyValuePair<string, List<int>> s in grams)
                    {
                        int min = 100000;

                        foreach (int E in s.Value)
                        {
                            if ((E < min) || (E > T_prime))
                            {
                                min = E;
                            }
                        }


                        if (min != 100000)
                        {
                            H.Enqueue(min);
                        }
                    }
                }

            }
            int m = 0;
            foreach (int index in top_K)
            {
                res[m] = (index_elements[index]);
                //res[m] = "hhhhheeehhehehe";
                m++;
            }

            return res;
        }

    }
}

