using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LemmaSharp.Classes;
using SourceFileBuilder.Classes;

namespace SourceFileBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentDirectory = Environment.CurrentDirectory + "/../../";
            var lemmatizerFilePath = currentDirectory + "../Test/Data/full7z-mlteast-en.lem";

            var fileName = Path.GetFileNameWithoutExtension(lemmatizerFilePath) + "-modified";
            var extension = Path.GetExtension(lemmatizerFilePath);
            var outputFilePath = string.Format("{0}Output/{1}.{2}", currentDirectory, fileName, extension);

            var enricherFilePath = currentDirectory + "Input/english-lemma-enricher.txt";

            EnrichFile(lemmatizerFilePath, outputFilePath, enricherFilePath, true);


            /*var irregularVerbForms = IrregularVerbs.GetIrregularVerbsFormsAndAssociatedLemma();
            EnrichFile(lemmatizerFilePath, outputFilePath, irregularVerbForms);

            Console.WriteLine("Output written at '{0}'", outputFilePath);

            Console.WriteLine("Try to read newly created file");
            using(var testStream = File.OpenRead(outputFilePath))
            {
                var testLemmatizer = new Lemmatizer(testStream);
            }*/

            Console.WriteLine("OK");
            Console.ReadKey();
        }

        private static void EnrichFile(string lemmatizerFilePath, string outputFilePath, string enricherFilePath, bool checkResults)
        {
            var fileReader = new EnricherFileReader(enricherFilePath);
            var allNewLemmas = fileReader.ReadAllLemmaEntries();

            EnrichFile(lemmatizerFilePath, outputFilePath, allNewLemmas, checkResults);
        }

        private static void EnrichFile(string lemmatizerFilePath, string outputFilePath,
            IEnumerable<Tuple<string, string, int>> wordsAndLemmaToAdd, bool checkResults)
        {
            var stream = File.OpenRead(lemmatizerFilePath);
            var lemmatizer = new Lemmatizer(stream);

            // add new words and lemma
            foreach (var wordAndLemma in wordsAndLemmaToAdd)
            {
                lemmatizer.AddExample(wordAndLemma.Item1, wordAndLemma.Item2, wordAndLemma.Item3);

                if (checkResults)
                {
                    var computedLemma = lemmatizer.Lemmatize(wordAndLemma.Item1);
                    Console.WriteLine("{0}: {1} -> {2}", computedLemma == wordAndLemma.Item2 ? "OK": "ERROR", wordAndLemma.Item1, 
                        computedLemma == wordAndLemma.Item2 ? computedLemma : string.Format("{0} (inst. of {1})", computedLemma, wordAndLemma.Item2));
                }
            }
            
            /*var words = new[] { "going", "working", "finding", "got", "found", "heard", 
                "told", "said", "thought", "met", "saw", "left", "took", 
                "was", "were", "lost", "felt", "fell", "brought", "spoke", 
                "making", "thinking", "meeting", "running", "staring", "seeing",
                // other examples to check
                "net", "nets", "belt", "smell"};
            foreach (var word in words)
            {
                var lemma = lemmatizer.Lemmatize(word);
                Console.WriteLine(word + " --> " + lemma);
            }*/

            // write output file
            using (var oStream = File.Create(outputFilePath))
            {
                lemmatizer.Serialize(oStream, true, Lemmatizer.Compression.Lzma, true);
            }
        }
    }
}
