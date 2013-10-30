using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Test.Classes;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // use a pre-built lemmatizer
            var lemmatizer = new LemmatizerPrebuiltFull(LanguagePrebuilt.English);

            // a bunch of words to lemmatize
            const string input = "Stanford University is located in California. It is a great university.";
            var words = Regex.Split(input, @"\W+").Where(w => !string.IsNullOrEmpty(w));

            // for each word, print it's lemma
            foreach (var word in words)
            {
                var lemma = lemmatizer.Lemmatize(word);
                Console.WriteLine(word + " --> " + lemma);
            }

            Console.ReadLine();
        }
    }
}
