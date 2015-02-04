using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LemmaSharp.Classes;
using Test.Classes;

namespace Test
{
    class Program
    {

        static void Main(string[] args)
        {
            // Create readable file
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataFilePath = string.Format("{0}/{1}/{2}", currentDirectory, "../../Data/Custom", "full7z-mlteast-en-modified.lem");

            using (var fstream = File.OpenRead(dataFilePath))
            {
                var lemmatizer = new Lemmatizer(fstream);

                // add examples
                var examples = new List<Tuple<string, string>>()
                {
                    /*new Tuple<string,string>("acting","act"),
                    new Tuple<string,string>("balled","ball"),
                    new Tuple<string,string>("balled","ball"),
                    new Tuple<string,string>("ballsed","balls"),
                    new Tuple<string,string>("bogged","bog"),
                    new Tuple<string,string>("bottomed","bottom"),
                    new Tuple<string,string>("bounced","bounce"),
                    new Tuple<string,string>("boxed","box"),
                    new Tuple<string,string>("brought","bring"),
                    new Tuple<string,string>("cashed","cash"),
                    new Tuple<string,string>("clouded","cloud"),
                    new Tuple<string,string>("cozied","cozy"),
                    new Tuple<string,string>("divided","divide"),
                    new Tuple<string,string>("felt","feel"),
                    new Tuple<string,string>("fiddling","fiddle"),
                    new Tuple<string,string>("fishing","fish"),
                    new Tuple<string,string>("fleshed","flesh"),
                    new Tuple<string,string>("fobbed","fob"),
                    new Tuple<string,string>("following","follow"),
                    new Tuple<string,string>("homing","home"),
                    new Tuple<string,string>("hunkered","hunker"),
                    new Tuple<string,string>("leveled","level"),
                    new Tuple<string,string>("laid","lay"),
                    new Tuple<string,string>("limbered","limber"),
                    new Tuple<string,string>("livened","liven"),
                    new Tuple<string,string>("livened","liven"),
                    new Tuple<string,string>("loaded","load"),
                    new Tuple<string,string>("magicked","magic"),
                    new Tuple<string,string>("messing","mess"),
                    new Tuple<string,string>("meted","mete"),
                    new Tuple<string,string>("mouthing","mouth"),
                    new Tuple<string,string>("perked","perk"),
                    new Tuple<string,string>("pootling","pootle"),
                    new Tuple<string,string>("sacked","sack"),
                    new Tuple<string,string>("screwing","screw"),
                    new Tuple<string,string>("sexed","sex"),
                    new Tuple<string,string>("shacked","shack"),
                    new Tuple<string,string>("speeded","speed"),
                    new Tuple<string,string>("spirited","spirit"),
                    new Tuple<string,string>("started","start"),
                    new Tuple<string,string>("stove","stave"),
                    new Tuple<string,string>("swung","swing"),
                    new Tuple<string,string>("teed","tee"),
                    new Tuple<string,string>("tired","tire"),
                    new Tuple<string,string>("used","use"),
                    new Tuple<string,string>("vacuumed","vacuum"),
                    new Tuple<string,string>("whiled","while"),
                    new Tuple<string,string>("wigged","wig"),
                    new Tuple<string,string>("zoned","zone"),*/
                    new Tuple<string,string>("don't","do"),
                    new Tuple<string,string>("doesn't","do"),
                    new Tuple<string,string>("didn't","did"),
                    new Tuple<string,string>("won't","will"),
                    new Tuple<string,string>("shan't","shall"),
                    new Tuple<string,string>("can't","can"),
                    new Tuple<string,string>("couldn't","could"),
                    new Tuple<string,string>("wouldn't","would"),
                    new Tuple<string,string>("shouldn't","should"),
                    new Tuple<string,string>("mustn't","must"),
                    new Tuple<string,string>("mightn't","might"),
                    new Tuple<string,string>("oughtn't","ought"),
                    new Tuple<string,string>("needn't","need"),
                    new Tuple<string,string>("aren't","are"),
                    new Tuple<string,string>("isn't","be"),
                    new Tuple<string,string>("wasn't","be"),
                    new Tuple<string,string>("weren't","be"),
                    new Tuple<string,string>("haven't","have"),
                    new Tuple<string,string>("hasn't","have"),
                    new Tuple<string,string>("hadn't","have"),
                    new Tuple<string,string>("'s", "'s"),
                    new Tuple<string,string>("'ve", "have"),
                    new Tuple<string,string>("'m", "be"),
                    new Tuple<string,string>("'re", "be"),
                    new Tuple<string,string>("'ll", "will"),
                };
                foreach (var example in examples)
                {
                    var lemma = lemmatizer.Lemmatize(example.Item1);
                    Console.WriteLine("{0} --> {1} {2}", example.Item1, lemma, lemma != example.Item2 ? ("!= " + example.Item2):"");
                }
            }

            
            Console.WriteLine("==========");
            Console.WriteLine("OK");
            Console.ReadLine();
        }

        private static Lemmatizer CreatePreBuiltLemmatizer()
        {
            var lemmatizer = new LemmatizerPrebuiltFull(LanguagePrebuilt.English);
            return lemmatizer;
        }

        private static Lemmatizer CreateLemmatizerFromFile()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dataFilePath = string.Format("{0}/{1}/{2}", currentDirectory, "../../Data/Custom", "english.lem");
            using (var stream = File.OpenRead(dataFilePath))
            {
                var lemmatizer = new Lemmatizer(stream);
                return lemmatizer;
            }
        }
    }
}
