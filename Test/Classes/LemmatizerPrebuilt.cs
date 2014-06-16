using System;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using LemmaSharp.Classes;

namespace Test.Classes {

    public enum LanguagePrebuilt {
        //from Multext-East v4 lexicons
        Bulgarian,
        Czech,
        English,
        Estonian,
        Persian,
        French,
        Hungarian,
        Macedonian,
        Polish,
        Romanian,
        Russian,
        Slovak,
        Slovene,
        Serbian,
        Ukrainian,
        //from Multext lexicons
        EnglishMt, 
        FrenchMt, 	
        German, 	
        Italian, 
        Spanish,
    }

    public enum LexiconPrebuilt
    {
        MltEast,
        Multext
    }

    [Serializable]
    public abstract class LemmatizerPrebuilt : Lemmatizer {

        // Private Variables --------------------------------

        private static readonly string[] AsLangMapping =
        {
            "bg", "mlteast",
            "cs", "mlteast",
            "en", "mlteast",
            "et", "mlteast",
            "fa", "mlteast",
            "fr", "mlteast",
            "hu", "mlteast",
            "mk", "mlteast",
            "pl", "mlteast",
            "ro", "mlteast",
            "ru", "mlteast",
            "sk", "mlteast",
            "sl", "mlteast",
            "sr", "mlteast",
            "uk", "mlteast",
            "en", "multext",
            "fr", "multext",
            "ge", "multext",
            "it", "multext",
            "sp", "multext",
        };

        private readonly LanguagePrebuilt lang;
        

        // Constructor(s) & Destructor(s) ----------------------

        public LemmatizerPrebuilt(LanguagePrebuilt lang)
        {
            this.lang = lang;
        }

        public LemmatizerPrebuilt(LanguagePrebuilt lang, LemmatizerSettings lsett): base(lsett) {
            this.lang = lang;
        }

        
        // Private Properties Helping Functions ---------------

        protected string GetResourceFileName(string sFileMask) {
            return GetResourceFileName(sFileMask, lang);
        }

        public static string GetResourceFileName(string sFileMask, LanguagePrebuilt lang) {
            string langFileName = AsLangMapping[(int)lang * 2 + 1] + '-' +AsLangMapping[(int)lang * 2];
            return String.Format(sFileMask, langFileName);
        }
        

        // Public Properties ----------------------------------

        public LanguagePrebuilt Language {
            get{
                return lang;
            }
        }
        public LexiconPrebuilt Lexicon
        {
            get
            {
                return GetLexicon(lang);
            }
        }
        

        // Public Properties ---------------------------------

        public static LexiconPrebuilt GetLexicon(LanguagePrebuilt lang)
        {
            return (LexiconPrebuilt)Enum.Parse(typeof(LexiconPrebuilt), AsLangMapping[((int)lang) * 2 + 1], true);
        }

        
        // Resource Management Functions --------------------

        protected abstract Assembly GetExecutingAssembly();

        protected Stream GetResourceStream(string sResourceShortName) {
            Assembly assembly = GetExecutingAssembly();

            string sResourceName = null;
            foreach (string sResource in assembly.GetManifestResourceNames())
                if (sResource.EndsWith(sResourceShortName)) {
                    sResourceName = sResource;
                    break;
                }

            if (String.IsNullOrEmpty(sResourceName)) return null;

            return assembly.GetManifestResourceStream(sResourceName);
        }
        

        // Serialization Functions -------------------------

        public LemmatizerPrebuilt(SerializationInfo info, StreamingContext context): base(info, context) {
        }

        

    }

}
