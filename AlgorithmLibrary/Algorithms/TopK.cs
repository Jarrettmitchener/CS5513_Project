﻿using AlgorithmLibrary.Datasets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AlgorithmLibrary.Algorithms
{
    public class TopK
    {
        private int k = 10; // Number of results requested
        private double max_w; // Maximum weight of a string
        private Dictionary<string, double> weights = new Dictionary<string, double>();
        private int f = 1; // multiplication factor, may change depending on results
        private string[] R1 = new string[10]; // the range search result set
        List<textResult> results = new List<textResult>();
        //private string[] init_set = new string[10]; // the range search result set
        private double theta = 0.0; // Similarity threshold, initially = 0.5
        //private int q_value = 2; // Denotates q value for the gram calculation in the context
        private double alpha = 1.0; // Similarity weight
        private double beta = 1.0; // Relevance weight 
        private double last_top_k_value = 0; // Stores the similarity value of the last result. 
        private Dictionary<string, List<int>> grams = new Dictionary<string, List<int>>(); // Stores the dataset grams 
        private double g_min = 100000; // What is the minimum similarity.
        private double theta_min = 1;  // The similarity initial threshold. 
        private Dictionary<int, string> index_elements = new Dictionary<int, string>(); // Stores the inverted grams list. 
        private Dictionary<string, string> datasets = new Dictionary<string, string>(); // Stores the inverted grams list. 
        private string[] query_grams; // Stores the grams of the query. 

        private string query;

        //Function called from the algorithm driver. 
        public  List<textResult> GetResult(List<String> keywords, List<DatasetObject> dataSetList, int q_value)
        {
            int count = 0;

            // Create the list of indexes and weights for the  selected dataset.
            foreach(DatasetObject s in  dataSetList)
            { 
                index_elements[count] = s.Text;
                weights[s.Text] = 0.1;
                datasets[s.Text] = s.Dataset;
                count++;
            }

            int s_count = 0;
            string s_query = "";

            // The way that the algo driver works it splits the words, because for the other 
            // two alforithms in necessary, But in this case we put toghether the phrase again. 
            foreach ( String s in keywords)
            {
                // If we have added more than one key word, then we need to add a space.
                if ( s_count  > 0)
                {
                    s_query = s_query + " " +  s;
                }
                else // Start the query phrase. 
                {
                    s_query = s; 
                }
            }
            query_grams = new string[s_query.Length - q_value + 1];
            
            // Call the ficntion that will execute the algorithm.
            return run_2HP(s_query, q_value);
        }

        private void computeGrams(int q_value)
        {
            // First start all of the lists, with the quey grams that will be used 
            // as the keys to retirve the indexes. 
            // This iterates over the query creating q_value length grams, that are subsrtirngs.
            for (int j = 0; j < query.Length - q_value + 1; j++)
            {
                string g1 = query.Substring(j, q_value); // compute gram
                query_grams[j] = (g1); // intitialize hash table
                grams[g1] = new List<int>(); // Add the lists to each hash value. 
            }

            // iterate over the dataset tuples. Looking for matches
            foreach (KeyValuePair<int, string> s in index_elements)
            {
                double count = 0;
                // Compute each tuple grams.
                for (int j = 0; j < s.Value.Length - q_value; j++)
                {
                    
                    string g1 = s.Value.Substring(j, q_value);
                    count++;
                    // if the query grams list contains that gram.
                    // then add the tuple index to the grams list.
                    if (query_grams.Contains(g1))
                    {
                        
                        if (!grams[g1].Contains(s.Key))
                        {
                            grams[g1].Add(s.Key);
                            grams[g1].Sort();
                        }
                    }

                    if (count < g_min)
                    {
                        g_min = count;
                    }

                }

                
            }
        }

        public List<textResult> run_2HP(string s_query, int q_value)
        {
            // Read data 
            //init_set = read_data();
            query = s_query;

            // Compute the inverted index list that matches the query grams with the dataset tuples index that
            // conains that  grams.
            computeGrams(q_value);
            R1 = IRS(q_value); // itereative search algorithm
            computeFrequency(last_top_k_value); // Compute threshold with IRS values.
            
            List<string> R = new List<string>();
            R = SPS(); // Single pass search 
            foreach (string s in R)
            {
                results.Add(new textResult { text = s, dataset = datasets[s] });
            }
            return results ;
        }

        private void computeFrequency(double k)
        {
            weights.Order(); // order the list by weight 
            double n_max = weights.First().Value; // get the highest value

            theta_min = (k - beta * n_max) / alpha; // Compute using the threshold fomula.
        }

        private string[] IRS(int q_value)
        {

            string[] res = [""];
            // First step : compute initial candidates. And the  adjusted threshold
            while (res.Length < f * k)
            {
                // Compute the top k results using the  initial similarity threshold
                res = computeSmilarity(q_value);

                // if the size of the result set is less than the frequency and 
               // multiplied by the k, you need to decrement the similarity threshold 
               // to find at least k candidates.
                if (res.Length < f * k)
                {
                    theta = theta - 0.01;
                }

            }

            // Compute the scores for  elements in res and keep the first k
            double min_theta = 0; // minimum similarity  

            // Adjust the minimun similarity score, based on  last element on the
            // results set.
            while (ComputeScore(min_theta, max_w) < ComputeScore(theta, last_top_k_value))
            {
                min_theta += 0.01;
            }

            // Then of the new theta is smaller,  rerun the top-k results. 
            if (min_theta < theta)
            {
                theta = min_theta;
                res = computeSmilarity(q_value);
            }
      
            return res; // return the resulting set
        }


        // Compute the similarity score, based on the jacquard distance and the 
        // weight of each string. 
        private double ComputeScore(double jcq, double weight)
        {
            return alpha * jcq + beta * weight;
        }


        // Approximate search algorithm, finds the top k results given a 
        // query string and dataset.
        private string[] computeSmilarity(int q_value)
        {
            // Initialize all necessary values ,
            double[] top_k = [0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0];
            string[] top_k_values = ["None", "None", "None", "None", "None", "None", "None", "None", "None", "None"];
            
            // For all the strings in the dataset.
            foreach (KeyValuePair<string, double> s in weights)
            {
                
                double num = 0.0; 
                double den = query.Length + s.Key.Length + 0.0;  

                // calculate the number of matching grams for the query and 
                // each strings. 
                for (int j = 0; j < query.Length - q_value; j++)
                {
                    string g2 = query.Substring(j, q_value);
                    for (int i = 0; i < s.Key.Length - q_value; i++)
                    {
                        // Look for grams matchs, andn increments the number of
                        // matches if any. 
                        string g1 = s.Key.Substring(i, q_value);
                        if (g1.Equals(g2)) { num += 1.0; }
                    }
                }
                double jacquard = num / (double)den;
                double weight = s.Value;

                // Compute the similarity score.
                double score = ComputeScore(jacquard, weight);

                // Look for all the top k values

                // Check if the number score is higher that the last top k 
                // value, and grater than the threshold 
                if (score > top_k[k - 1] && score > theta)
                {
                    // Then add the new value to the last position.
                    // forgetting the last element
                    top_k[k - 1] = score;
                    top_k_values[k - 1] = s.Key;
                    int i = k - 2;

                    // iterate over all elements shifting the values to the 
                    // following top k position according to the  score of the 
                    // string. 
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
                        // In this case you have to be careful with indexes.
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

        // Single pass algorithm, second stage, computes the top k results using the 
        // grams and indexes to the elements. 
        private List<string> SPS()
        {
            double n_gram = (double)grams.Count; // Number of grams in the query     
            double g = g_min; // Freuency thresold.
            List<int> top_K = new List<int>(); // list with top k elements,
            double top_k_last = 0;
            int top = 0; // Number of added elemnts. 
            Queue<int> H = new Queue<int>();// The lists of ids for the query grams.
            int indx = 0; // Index for retrieving the values
            List<string> res = new List<string>(); // result string

            // insert the top element of each list to a heap
            foreach (KeyValuePair<string, List<int>> s in grams)
            {
                if (s.Value.Count > 0 && s.Value.Count > indx)
                {
                    H.Enqueue(s.Value[indx]);
                }
            }

            // For all of the elemnts in the heap. 
            while( H.Any())
            {
                int T = H.Dequeue(); // take first element
                int num = H.Count(); // Reminder of the total of elements before popping elements
                int p = 1; // Number of common grams with the query.
                for (int j = 0; j < num - 1; j++)
                {
                    int element = H.Dequeue();
                    // For all of the grams, if they are equal , add one to the count
                    if (T == element)
                    {
                        p += 1;
                    }
                    else
                    {
                        H.Enqueue(element);
                    }
                }

                // If the number of equal grams is higher or equal than the threshold
                if (p >= g)
                {
                    // For the first k elements you just add them to the result.
                    if (top < k)
                    {
                        top++;
                        if (!top_K.Contains(T))
                        {
                            top_K.Add(T); // add it to the top k list 
                            top_k_last = p;
                        }
                    }
                    else if ( p < top_k_last) // For the rest check  whether  it is more
                                             // similar wiht the last element and add it
                    {
                        top_K.RemoveAt(top_K.Count() - 1);
                        if (!top_K.Contains(T))
                        {
                            top_K.Add(T); // add it to the top k list
                            top_k_last = p;
                        }
                    }

                    // Recompute the threshold. 
                    if ((theta_min * n_gram) > ((n_gram + g_min) / ( 1 + (1 / theta_min))))
                    {
                        g = (theta_min * n_gram);
                    }
                    else
                    {
                        g = ((n_gram + g_min) / (1.0 + (1.0 / theta_min)));
                    }

                    if( g >= n_gram)
                    {
                        break;
                    }

                    indx += 1;

                    // Push next element( if any) of each popped list 
                    // to H 
                    foreach (KeyValuePair<string, List<int>> s in grams)
                    {
                        if (indx < (s.Value).Count)
                        {
                            if (s.Value.Any())
                                H.Enqueue(s.Value[indx]);
                        }
                    }
                }
                else // pop additional g - p - 1 elements from the heap
                {
                    for (int j = 0; (j < g - p - 1) && (H.Count > 0); j++)
                    {
                        H.Dequeue();
                    }
                    if (top_K.Count > 0)
                    {
                        int T_prime = top_K.Last(); // current  last top element
                        int count = 0; 
                        // For each of the g - q popped lists 
                        foreach (KeyValuePair<string, List<int>> s in grams)
                        {
                            if (count > g - 1)
                                break; 
                            int min = 100000;

                            // Locate its smallest element 
                            foreach (int E in s.Value)
                            {
                                if ((E < min) && (E >T_prime))
                                {
                                    min = E;
                                }
                            }

                            // If there is a minimum add it to the heap.
                            if (min != 100000)
                            {
                                H.Enqueue(min);
                            }
                        }
                    }
                }
                
            }
            //int m = 0;
            foreach (int index in top_K)
            {
                // Retrieve the  entries that correspond with the gram  list index. 
                res.Add(index_elements[index]);  
            }

            return res; // Return the top k results.,
        }

        
    }
}
