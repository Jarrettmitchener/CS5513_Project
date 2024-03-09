using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLibrary.Algorithms
{
    public class BoyerMooreAdvanced
    {
        // generated list with 20 sentences all relating to school
        public List<string> GetRawList()
        {
            List<string> schoolSentences = new List<string>
            {
                "School is a place where students learn and grow.",
                "school, school, school, and school. I hate school.",
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

       

        public List<string> Search(List<string> keywords)
        {
            List<string> texts = GetRawList();

            List<string> results = new List<string>();
            List<searchResult> searchResults = new List<searchResult>();

            // pre processes the text and keyword list to be lowercase
            for (int i = 0; i < texts.Count; i++)
            {
                texts[i] = texts[i].ToLower();
            }
            for (int i = 0; i < keywords.Count; i++)
            {
                keywords[i] = keywords[i].ToLower();
            }

            // actually begins search
            //loops through each text entry
            foreach (var text in texts)
            {
                int occurences = 0;
                bool found = false;
                //loops for each keyword
                foreach (var keyword in keywords)
                {
                    searchResult foundResult = BoyerMooreSearch(text, keyword);

                    if (foundResult.resultFound == true)
                    {
                        found = true;
                        occurences += foundResult.foundCount;
                    }
                }
                //if the serach result is found, add it to the new entry list
                if(found == true)
                {
                    searchResult newEntry = new searchResult();
                    newEntry.text = text;
                    newEntry.foundCount = occurences;

                    searchResults.Add(newEntry);
                }
            }
            //sorts the list based on the number of found patterns
            var sortedSearchResults = searchResults.OrderByDescending(r => r.foundCount).ToList();

            //converts the searchResults list to a string list
            results = sortedSearchResults.Select(r =>r.text).ToList();

            return results;
        }
        //returns true if the keyword is found
        //private class that will return information relavent to the boyer moore search
        private class searchResult
        {
            public bool resultFound { get; set; }
            public int foundCount { get; set; }
            public string text { get; set; }

            public searchResult()
            {
                resultFound = false;
                foundCount = 0;
                text = string.Empty;
            }
        }

        private searchResult BoyerMooreSearch(string text, string pattern)
        {
            searchResult result = new searchResult();
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
                    result.resultFound = true;
                    result.foundCount++;
                    s += (s + m < n) ? m - badChar[text[s + m]] : 1;
                }
                else
                {
                    // Shift the pattern to align the bad character in the text
                    // with the last occurrence of it in the pattern. The max
                    // function is used to make sure that we get a positive shift.
                    s += Math.Max(1, j - badChar[text[s + j]]);
                }
            }

            return result;
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
