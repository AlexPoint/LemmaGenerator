using System;
using System.IO;

namespace LemmaSharp.Classes {
    public class LemmaRule {

        // Private Variables -----------------------

        private int iId;
        private int iFrom;
        private string sFrom;
        private string sTo;
        private string sSignature;
        private LemmatizerSettings lsett;
        

        // Constructor(s) & Destructor(s) ---------

        public LemmaRule(string sWord, string sLemma, int iId, LemmatizerSettings lsett) {
            this.lsett = lsett;
            this.iId = iId;

            int iSameStem = SameStem(sWord, sLemma);
            sTo = sLemma.Substring(iSameStem);
            iFrom = sWord.Length - iSameStem;

            if (lsett.bUseFromInRules) {
                sFrom = sWord.Substring(iSameStem);
                sSignature = "[" + sFrom + "]==>[" + sTo + "]";
            }
            else {
                sFrom = null;
                sSignature = "[#" + iFrom + "]==>[" + sTo + "]";
            }
        }
        

        // Public Properties ---------------------

        public string Signature {
            get {
                return sSignature;
            }
        }
        public int Id {
            get {
                return iId;
            }
        }
        

        // Essential Class Functions -------------

        private static int SameStem(string sStr1, string sStr2) {
            int iLen1 = sStr1.Length;
            int iLen2 = sStr2.Length;
            int iMaxLen = Math.Min(iLen1, iLen2);

            for (int iPos = 0; iPos < iMaxLen; iPos++)
                if (sStr1[iPos] != sStr2[iPos]) return iPos;

            return iMaxLen;
        }
        public bool IsApplicableToGroup(int iGroupCondLen) {
            return iGroupCondLen >= iFrom; 
        }
        public string Lemmatize(string sWord)
        {
            // if the removed part is upper, replace by an uppercase string
            var isRemovedPartUpper = IsFullyUpper(sWord.Substring(sWord.Length - iFrom, iFrom));
            return sWord.Substring(0, sWord.Length - iFrom) + (isRemovedPartUpper ? sTo.ToUpper() : sTo);
        }
        

        // Output Functions (ToString) ----------

        public override string ToString() {
            return iId + ":" + sSignature;
        }
        
        // Serialization Functions (regular) -----

        public void Serialize(StreamWriter sWrt, bool bThisTopObject)
        {
            //save metadata
            sWrt.Write(bThisTopObject); sWrt.Write(Constants.Separator);

            //save value types --------------------------------------
            sWrt.Write(iId); sWrt.Write(Constants.Separator);
            sWrt.Write(iFrom); sWrt.Write(Constants.Separator);
            if (sFrom == null)
            {
                sWrt.Write(false); sWrt.Write(Constants.Separator);
            }
            else
            {
                sWrt.Write(true); sWrt.Write(Constants.Separator);
                sWrt.Write(sFrom); sWrt.Write(Constants.Separator);
            }
            sWrt.Write(sTo); sWrt.Write(Constants.Separator);
            sWrt.Write(sSignature); sWrt.Write(Constants.Separator);

            if (bThisTopObject)
            {
                lsett.Serialize(sWrt); sWrt.Write(Constants.Separator);
            }

            sWrt.WriteLine();
        }

        // Serialization Functions (Binary) -----

        public void Serialize(BinaryWriter binWrt, bool bThisTopObject) {
            //save metadata
            binWrt.Write(bThisTopObject);            
            
            //save value types --------------------------------------
            binWrt.Write(iId);
            binWrt.Write(iFrom);
            if (sFrom == null)
            {
                binWrt.Write(false);
            }
            else
            {
                binWrt.Write(true);
                binWrt.Write(sFrom);
            }
            binWrt.Write(sTo);
            binWrt.Write(sSignature);

            if (bThisTopObject)
            {
                lsett.Serialize(binWrt);
            }
        }
        public void Deserialize(BinaryReader binRead, LemmatizerSettings lsett) {
            //load metadata
            bool bThisTopObject = binRead.ReadBoolean();    
            
            //load value types --------------------------------------
            iId = binRead.ReadInt32();
            iFrom = binRead.ReadInt32();
            if (binRead.ReadBoolean())
            {
                sFrom = binRead.ReadString();
            }
            else
            {
                sFrom = null;
            }
            sTo = binRead.ReadString();
            sSignature = binRead.ReadString();

            //load refernce types if needed -------------------------
            if (bThisTopObject)
            {
                this.lsett = new LemmatizerSettings(binRead);
            }
            else
            {
                this.lsett = lsett;
            }
        }
        public LemmaRule(BinaryReader binRead, LemmatizerSettings lsett) {
            this.Deserialize(binRead, lsett);
        }
        

        // Serialization Functions (Latino) -----
        #if LATINO

        public void Save(Latino.BinarySerializer binWrt, bool bThisTopObject) {
            //save metadata
            binWrt.WriteBool(bThisTopObject);

            //save value types --------------------------------------
            binWrt.WriteInt(iId);
            binWrt.WriteInt(iFrom);
            if (sFrom == null)
                binWrt.WriteBool(false);
            else {
                binWrt.WriteBool(true);
                binWrt.WriteString(sFrom);
            }
            binWrt.WriteString(sTo);
            binWrt.WriteString(sSignature);

            if (bThisTopObject)
                lsett.Save(binWrt);
        }
        public void Load(Latino.BinarySerializer binRead, LemmatizerSettings lsett) {
            //load metadata
            bool bThisTopObject = binRead.ReadBool();

            //load value types --------------------------------------
            iId = binRead.ReadInt();
            iFrom = binRead.ReadInt();
            if (binRead.ReadBool())
                sFrom = binRead.ReadString();
            else
                sFrom = null;
            sTo = binRead.ReadString();
            sSignature = binRead.ReadString();

            //load refernce types if needed -------------------------
            if (bThisTopObject)
                this.lsett = new LemmatizerSettings(binRead);
            else
                this.lsett = lsett;
        }
        public LemmaRule(Latino.BinarySerializer binRead, LemmatizerSettings lsett) {
            Load(binRead, lsett);
        }

        #endif
        

        // String utilities ------
        public static bool IsFullyUpper(string value)
        {
            if (string.IsNullOrEmpty(value)){ return false; }

            // Consider string to be uppercase if it has no lowercase letters.
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsLower(value[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
