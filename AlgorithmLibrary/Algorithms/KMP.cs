using AlgorithmLibrary.Datasets;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibrary.Algorithms
{

    //Constructor to store in the text and the dataset of the object.
    public class KMPListObj
    {
        public string text { get; set; }

        public string lowercase { get; set; }

        public string dataSet { get; set; }

    }


    public class KMP
    {
        //Main KMP search algorithm.
        public int KMPSearch(string pat, string txt, int findings)
        {
            int M = pat.Length;
            int N = txt.Length;

            // create lps[] that will hold the longest
            // prefix suffix values for pattern
            int[] lps = new int[M];
            int j = 0; // index for pat[]

            // Preprocess the pattern (calculate lps[]
            // array)
            computeLPSArray(pat, M, lps);

            int i = 0; // index for txt[]
            while (i < N)
            {
                //a piece of character is matched. increment to see additional characters.
                if (pat[j] == txt[i])
                {
                    j++;
                    i++;
                }

                //Pat matches with a part of the txt line.
                if (j == M)
                {
                    //keep track of findings after you find a search to ensure you add the entry index into the found array.
                    findings++;
                    //reset the length of the pat[] after you find every match.
                    j = lps[j - 1];
                }

                // mismatch after j matches
                else if (i < N && pat[j] != txt[i])
                {
                    // Do not match lps[0..lps[j-1]] characters,
                    // they will match anyway
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }
            //return the number of findings into the findings array.
            return findings;
        }

        //Perform this function to compute the array pattern.
        public void computeLPSArray(string pat, int M, int[] lps)
        {
            // length of the previous longest prefix suffix
            int len = 0;
            int i = 1;
            lps[0] = 0; // lps[0] is always 0

            // the loop calculates lps[i] for i = 1 to M-1
            while (i < M)
            {
                if (pat[i] == pat[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else // if the index equals the length
                {
                    // What happens length and the index are not the same.
                    if (len != 0)
                    {
                        len = lps[len - 1];

                        // Don't increment i if len is not 0.
                    }
                    else // len is 0. Then increment i.
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
        }


        public List<textResult> GetResult(List<String> keywords, List<DatasetObject> dataSetList)
        {
            //declare list containing the found text corresponding to the dataset.
            //This list will be returned to the driver.
            List<textResult> foundResults = new List<textResult>();

            //declare list to store the dataset list.
            List<KMPListObj> kMPListObjs = new List<KMPListObj>();

            //variable object to store in the original text, lowered case text and the dataset.
            //text and pattern will all be lowered case to search regardless of casing.
            for(int i = 0; i < dataSetList.Count; i++)
            {
                kMPListObjs.Add(new KMPListObj
                {
                    text = dataSetList[i].Text,
                    lowercase = dataSetList[i].Text.ToLower(),
                    dataSet = dataSetList[i].Dataset
                }); ;
            }


            //make all the keywords lowercase.
            for (int i = 0; i < keywords.Count; i++)
            {
                keywords[i] = keywords[i].ToLower();
            }


            //variable to track the occurences to see if the keyword is found.
            int findings = 0;


            //Go through the dataset list and search for the pattern in each string set.
            foreach (var obj in kMPListObjs)
            {
                //Always reset the finds to zero before moving on to the next entry list and performing the algorithm again.
                findings = 0;

                //nested for loop to look for every keyword in the given dataset.
                foreach (var word in keywords)
                {
                    findings = KMPSearch(word, obj.lowercase, findings);
                }

                //If there are any occurences in the string, add that string.
                if (findings != 0)
                {
                    foundResults.Add(new textResult
                    {
                        text = obj.text,
                        dataset = obj.dataSet
                    });
                }
            }

            //Done with the search, now return it into the algo driver.
            return foundResults;

        }
    }
}
