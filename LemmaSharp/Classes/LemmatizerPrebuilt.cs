using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;

namespace LemmaSharp {

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
        EnglishMT, 
        FrenchMT, 	
        German, 	
        Italian, 
        Spanish,
    }

    public enum LexiconPrebuilt
    {
        MltEast,
        Multext
    }

    [Serializable()]
    public abstract class LemmatizerPrebuilt : Lemmatizer {

        #region Private Variables

        private static string[] asLangMapping = new string[] {
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

        private LanguagePrebuilt lang;

        #endregion

        #region Constructor(s) & Destructor(s)

        public LemmatizerPrebuilt(LanguagePrebuilt lang)
            : base() {
            this.lang = lang;
        }

        public LemmatizerPrebuilt(LanguagePrebuilt lang, LemmatizerSettings lsett)
            : base(lsett) {
            this.lang = lang;
        }

        #endregion

        #region Private Properties Helping Functions

        protected string GetResourceFileName(string sFileMask) {
            return GetResourceFileName(sFileMask, lang);
        }

        public static string GetResourceFileName(string sFileMask, LanguagePrebuilt lang) {
            string langFileName = asLangMapping[(int)lang * 2 + 1] + '-' +asLangMapping[(int)lang * 2];
            return String.Format(sFileMask, langFileName);
        }

        #endregion

        #region Public Properties

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

        #endregion

        #region Public Properties

        public static LexiconPrebuilt GetLexicon(LanguagePrebuilt lang)
        {
            return (LexiconPrebuilt)Enum.Parse(typeof(LexiconPrebuilt), asLangMapping[((int)lang) * 2 + 1], true);
        }

        #endregion

        #region Resource Management Functions

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

        #endregion

        #region Serialization Functions

        public LemmatizerPrebuilt(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        #endregion

    }

}
