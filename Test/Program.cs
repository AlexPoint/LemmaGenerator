using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LemmaSharp;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var lemmatizer = new LemmatizerPrebuiltFull(LanguagePrebuilt.English);

            var input = "Stanford University is located in California. It is a great university.";
            var words = Regex.Split(input, @"\W+").Where(w => !string.IsNullOrEmpty(w));

            foreach (var word in words)
            {
                var lemma = lemmatizer.Lemmatize(word);
                Console.WriteLine(word + " --> " + lemma);
            }

            Console.ReadLine();
        }
    }
}
