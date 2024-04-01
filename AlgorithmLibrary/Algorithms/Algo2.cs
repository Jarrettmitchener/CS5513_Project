using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibrary.Algorithms
{
    public class Algo2
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


        public string[] GetResult()
        {
            //Add in arrays of strings from the text file.
            string[] txt = { "School sucks. I don't like school.", "I like college.", "Where is work?", "College is school!" };
            //add in array of word searches from a text file.
            string pat = "school";
            //any findings variable.
            int findings = 0;
            //make a generic list of all found strings.
            List<string> foundlist = new List<string>();


            //Go through the strings array and search for the word or phrase.
            for (int i = 0; i < txt.Length; i++)
            {
                findings = 0;
                findings = KMPSearch(pat, txt[i], findings);
                //If word or phrase is found add into the found index.
                if (findings != 0)
                {
                    foundlist.Add(txt[i]);
                }
            }

            //add them into the array.
            string[] foundarray = foundlist.ToArray();

            return foundarray;

        }
    }
}
