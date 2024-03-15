using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlgorithmLibrary.Datasets;

namespace AlgorithmLibrary.Algorithms
{
    public class BoyerMooreListObj
    {
        public string originalString { get; set; }
        public string searchString { get; set; }
        public string dataSet { get; set; }

    }

    public class BoyerMoore
    {


        // generated list with 20 sentences all relating to school
        public List<string> GetRawList()
        {
            List<string> schoolSentences = new List<string>
            {
                "School is a place where students learn and grow.",
                "Teachers play a vital role in shaping young minds in school.",
                "Students eagerly anticipate the end of the school day to engage in extracurricular activities.",
                "The school cafeteria offers a variety of meals to cater to diverse tastes.",
                "Education is the cornerstone of success, and school is where it begins.",
                "School libraries are treasure troves of knowledge waiting to be explored.",
                "Every morning, students gather in the schoolyard, excitedly chatting with friends.",
                "School assemblies foster a sense of community and belonging among students.",
                "The sound of the school bell signals the start and end of each lesson.",
                "School trips provide students with hands-on learning experiences outside the classroom.",
                "Homework assignments are designed to reinforce concepts taught in school.",
                "Exams can be stressful, but they are a necessary part of assessing students' understanding.",
                "School projects encourage creativity and teamwork among students.",
                "Sports teams represent the school in competitions, fostering teamwork and sportsmanship.",
                "School counselors provide support and guidance to students facing personal challenges.",
                "Parent-teacher meetings are opportunities for collaboration in a student's education.",
                "School uniforms promote a sense of equality and unity among students.",
                "The school year is filled with exciting events like prom, graduation, and field trips.",
                "Volunteering at school events strengthens community ties and fosters a sense of pride.",
                "Teachers often use multimedia tools to make lessons engaging and interactive.",
            };


            return schoolSentences;
        }

        public List<textResult> Search(List<string> keywords, List<DatasetObject> dataSetList)
        {
            //starts the stopwatch time for the algorithm
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //used for testing/debugging
            //List<string> texts = GetRawList();
            List<BoyerMooreListObj> boyerMooreListObjs = new List<BoyerMooreListObj>();

            List<textResult> results = new List<textResult>();


            // pre processes the text and keyword list to be lowercase
            for(int i = 0; i < dataSetList.Count; i++)
            {
                //Assigns the string, a lowered search string, and the dataset to the boyereMoore Lists
                boyerMooreListObjs.Add(new BoyerMooreListObj
                {
                    originalString = dataSetList[i].Text,
                    searchString = dataSetList[i].Text.ToLower(),
                    dataSet = dataSetList[i].Dataset
                });
            }
            for (int i = 0; i < keywords.Count; i++)
            {
                keywords[i] = keywords[i].ToLower();
            }

            // actually begins search
            foreach (var obj in boyerMooreListObjs)
            {
                foreach (var keyword in keywords)
                {
                    if (BoyerMooreSearch(obj.searchString, keyword))
                    {
                        //adds the original string and not the lowered search string
                        results.Add(new textResult 
                        { 
                            text = obj.originalString,
                            dataset= obj.dataSet
                        });
                        break;
                    }
                }
            }
            stopwatch.Stop();
            double elapsedTimeSeconds = stopwatch.Elapsed.TotalSeconds;
            return results;
        } 
        //returns true if the keyword is found
        private bool BoyerMooreSearch(string text, string pattern)
        {
            int m = pattern.Length;
            int n = text.Length;
            int[] badChar = new int[256];

            // Preprocess the bad character heuristic
            badCharHeuristic(pattern, m, badChar);

            int s = 0;
            while (s <= n - m)
            {
                int j = m - 1;

                // keep reducing index j of the pattern while characters of
                // pattern and text are matching at this shift s
                while (j >= 0 && pattern[j] == text[s + j])
                    j--;

                // If the pattern is present at the current shift
                if (j < 0)
                {
                    return true;
                }
                else
                {
                    // Shift the pattern to align the bad character in the text
                    // with the last occurrence of it in the pattern. The max
                    // function is used to make sure that we get a positive shift.
                    char c = text[s + j];
                    //this protects against weird characters that are not standard ASCII value
                    if (c > 256)
                    {
                        c = ' ';
                    }

                    s += Math.Max(1, j - badChar[c]);
                }
            }

            return false;
        }

        private void badCharHeuristic(string str, int size, int[] badChar)
        {
            // Initializing all occurrences as -1
            for (int i = 0; i < 256; i++)
            {
                badChar[i] = -1;
            }
            // Fill the actual value of last occurrence of a char
            for (int i = 0; i < size; i++)
            {
                badChar[str[i]] = i;
            }
        }
    }
}
