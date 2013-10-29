using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;

namespace LemmaSharp
{
    [Serializable()]
    public class LemmatizerPrebuiltFull : LemmatizerPrebuilt
    {
        public const string FILEMASK = "full7z-{0}.lem";

        #region Constructor(s) & Destructor(s)

        public LemmatizerPrebuiltFull(LanguagePrebuilt lang)
            : base(lang)
        {
            Stream stream = GetResourceStream(GetResourceFileName(FILEMASK));
            this.Deserialize(stream);
            stream.Close();
        }

        #endregion

        #region Resource Management Functions

        protected override Assembly GetExecutingAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        #endregion

    }
}
