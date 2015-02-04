using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            var outputFilePath = string.Format("{0}Output/{1}{2}", currentDirectory, fileName, extension);

            var enricherFilePaths = Directory.GetFiles(currentDirectory + "Input/");


            using (var stream = File.OpenRead(lemmatizerFilePath))
            {
                // create base lemmatizer with data in the base source file
                var lemmatizer = new Lemmatizer(stream);

                // then, enrich lemmatizer with every other files
                foreach (var filePath in enricherFilePaths)
                {
                    EnrichLemmatizerWithDataFile(lemmatizer, filePath);
                }

                // persist lemmatizer in output file
                Console.WriteLine("Writing output file...");
                using (var oStream = File.Create(outputFilePath))
                {
                    lemmatizer.Serialize(oStream, true, Lemmatizer.Compression.Lzma, true);
                }
                Console.WriteLine("Outuput file written at {0}", outputFilePath);
            }

            Console.WriteLine("OK");
            Console.ReadKey();
        }


        private static void EnrichLemmatizerWithDataFile(Lemmatizer lemmatizer, string enricherFilePath)
        {
            var fileReader = new EnricherFileReader(enricherFilePath);
            var newLemmas = fileReader.ReadAllLemmaEntries();
            
            EnrichLemmatizerWithExamples(lemmatizer, newLemmas);
        }
        
        private static void EnrichLemmatizerWithExamples(Lemmatizer lemmatizer, IEnumerable<Tuple<string, string, int>> wordsAndLemmaToAdd)
        {
            // add new words and lemma
            foreach (var wordAndLemma in wordsAndLemmaToAdd)
            {
                AddExampleOrException(lemmatizer, wordAndLemma.Item1, wordAndLemma.Item2);
            }
        }

        private static void AddExampleOrException(Lemmatizer lemmatizer, string word, string lemma)
        {
            // compute the lemma of this example
            var computedLemma = lemmatizer.Lemmatize(word);

            if(computedLemma != lemma)
            {
                // if the computed lemma is different from what we expect, 
                // add this example to lemmatizer (lemmatizer can then deduce a new rule and succeed, or still fail)
                lemmatizer.AddExample(word, lemma);

                // if still doesn't work --> add exception
                var computedLemma2 = lemmatizer.Lemmatize(word);
                if (computedLemma2 != lemma)
                {
                    Console.WriteLine("Added lemma exception: {0} -> {1}", word, lemma);
                    lemmatizer.AddException(word, lemma);
                }
            }
        }
    }
}

