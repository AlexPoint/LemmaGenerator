using System.Collections.Generic;
using System.IO;

namespace LemmaSharp.Classes {

    public class RuleList : Dictionary<string, LemmaRule> {

        // Private Variables ------------------------

        private LemmatizerSettings lsett;
        private LemmaRule lrDefaultRule;
        
        
        // Constructor(s) & Destructor(s) ------------

        public RuleList(LemmatizerSettings lsett) {
            this.lsett = lsett;
            lrDefaultRule = AddRule(new LemmaRule("", "", 0, lsett));
        }

        
        // Public Properties -----------------------

        public LemmaRule DefaultRule {
            get {
                return lrDefaultRule;
            }
        }
        

        // Essential Class Functions --------------

        public LemmaRule AddRule(LemmaExample le) {
            return AddRule(new LemmaRule(le.Word, le.Lemma, this.Count, lsett));
        }
        private LemmaRule AddRule(LemmaRule lrRuleNew) {
            LemmaRule lrRuleReturn = null;

            if (!this.TryGetValue(lrRuleNew.Signature, out lrRuleReturn)) {
                lrRuleReturn = lrRuleNew;
                this.Add(lrRuleReturn.Signature, lrRuleReturn);
            }

            return lrRuleReturn;
        }
        
        // Serialization Functions (regular) ------

        public void Serialize(StreamWriter sWrt, bool bThisTopObject)
        {
            //save metadata
            sWrt.Write(bThisTopObject); sWrt.WriteLine(Constants.Separator);

            //save value types --------------------------------------

            //save refernce types if needed -------------------------
            if (bThisTopObject)
            {
                lsett.Serialize(sWrt);
            }

            //save list items ---------------------------------------
            int iCount = this.Count;
            sWrt.WriteLine(iCount);
            foreach (KeyValuePair<string, LemmaRule> kvp in this)
            {
                sWrt.WriteLine(kvp.Key);
                kvp.Value.Serialize(sWrt, false);
            }

            //default rule is already saved in the list. Here just save its id.
            sWrt.WriteLine(lrDefaultRule.Signature);
        }
     
        // Serialization Functions (Binary) ------
        
        public void Serialize(BinaryWriter binWrt, bool bThisTopObject) {
            //save metadata
            binWrt.Write(bThisTopObject);

            //save value types --------------------------------------

            //save refernce types if needed -------------------------
            if (bThisTopObject)
            {
                lsett.Serialize(binWrt);
            }

            //save list items ---------------------------------------
            int iCount = this.Count;
            binWrt.Write(iCount);
            foreach (KeyValuePair<string, LemmaRule> kvp in this) {
                binWrt.Write(kvp.Key);
                kvp.Value.Serialize(binWrt, false);
            }

            //default rule is already saved in the list. Here just save its id.
            binWrt.Write(lrDefaultRule.Signature); 
        }
        public void Deserialize(BinaryReader binRead, LemmatizerSettings lsett) {
            //load metadata
            bool bThisTopObject = binRead.ReadBoolean();
            
            //load value types --------------------------------------

            //load refernce types if needed -------------------------
            this.lsett = bThisTopObject ? new LemmatizerSettings(binRead) : lsett;
               
            //load list items ---------------------------------------
            this.Clear();
            int iCount = binRead.ReadInt32();
            for (int iId = 0; iId < iCount; iId++) {
                string sKey = binRead.ReadString();
                var lrVal = new LemmaRule(binRead, this.lsett);
                this.Add(sKey, lrVal);
            }

            //link the default rule just Id was saved.
            lrDefaultRule = this[binRead.ReadString()];
        }
        public RuleList(BinaryReader binRead, LemmatizerSettings lsett) {
            this.Deserialize(binRead, lsett);
        }

        
        // Serialization Functions (Latino) ------
        #if LATINO

        public void Save(Latino.BinarySerializer binWrt, bool bThisTopObject) {
            //save metadata
            binWrt.WriteBool(bThisTopObject);

            //save value types --------------------------------------

            //save refernce types if needed -------------------------
            if (bThisTopObject)
                lsett.Save(binWrt);

            //save list items ---------------------------------------
            int iCount = this.Count;
            binWrt.WriteInt(iCount);
            foreach (KeyValuePair<string, LemmaRule> kvp in this) {
                binWrt.WriteString(kvp.Key);
                kvp.Value.Save(binWrt, false);
            }

            //default rule is already saved in the list. Here just save its id.
            binWrt.WriteString(lrDefaultRule.Signature); 
        }
        public void Load(Latino.BinarySerializer binRead, LemmatizerSettings lsett) {
            //load metadata
            bool bThisTopObject = binRead.ReadBool();

            //load value types --------------------------------------

            //load refernce types if needed -------------------------
            if (bThisTopObject)
                this.lsett = new LemmatizerSettings(binRead);
            else
                this.lsett = lsett;

            //load list items ---------------------------------------
            this.Clear();
            int iCount = binRead.ReadInt();
            for (int iId = 0; iId < iCount; iId++) {
                string sKey = binRead.ReadString();
                LemmaRule lrVal = new LemmaRule(binRead, this.lsett);
                this.Add(sKey, lrVal);
            }

            //link the default rule just Id was saved.
            lrDefaultRule = this[binRead.ReadString()];

        }
        public RuleList(Latino.BinarySerializer binRead, LemmatizerSettings lsett) {
            Load(binRead, lsett);
        }

        #endif
        
    }
}
