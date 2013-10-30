using System;
using System.IO;
using System.Reflection;

namespace Test.Classes
{
    [Serializable]
    public class LemmatizerPrebuiltFull : LemmatizerPrebuilt
    {
        public const string Filemask = "full7z-{0}.lem";

        // Constructor(s) & Destructor(s) ---------------------

        public LemmatizerPrebuiltFull(LanguagePrebuilt lang): base(lang)
        {
            Stream stream = GetResourceStream(GetResourceFileName(Filemask));
            this.Deserialize(stream);
            stream.Close();
        }


        // Resource Management Functions ----------------------

        protected override Assembly GetExecutingAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }


    }
}
