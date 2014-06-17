using System;
using System.IO;
using System.Runtime.Serialization;

namespace LemmaSharp.Classes {

    /// <summary>
    /// These are the lemmagen algorithm settings that affect speed/power of the learning and lemmatizing algorithm.
    /// TODO this class will be probbably removed in the future.
    /// </summary>
    [Serializable()]
    public class LemmatizerSettings : ISerializable {
        
        // Constructor(s) & Destructor(s) -------------------

        public LemmatizerSettings() { } 

        
        // Sub-Structures ----------------------------------

        /// <summary>
        /// How algorithm considers msd tags.
        /// MSD stands for the wordform morphosyntactic description. 
        /// This is the set of all lemmas starting with "writ-", as they appear in the Multext English lexicon
        /// </summary>
        public enum MsdConsideration {
            /// <summary>
            /// Completely ignores msd tags (join examples with different tags and sum their weihgts).
            /// </summary>
            Ignore,
            /// <summary>
            /// Same examples with different msd's are not considered equal and joined.
            /// </summary>
            Distinct,
            /// <summary>
            /// Joins examples with different tags (concatenates all msd tags).
            /// </summary>
            JoinAll,
            /// <summary>
            /// Joins examples with different tags (concatenates just distinct msd tags - somehow slower).
            /// </summary>
            JoinDistinct,
            /// <summary>
            /// Joins examples with different tags (new tag is the left to right substring that all joined examples share).
            /// </summary>
            JoinSameSubstring
        }         
        

        // Public Variables --------------------------------

        /// <summary>
        /// True if from string should be included in rule identifier ([from]->[to]). False if just length of from string is used ([#len]->[to]).
        /// </summary>
        public bool bUseFromInRules = true;
        /// <summary>
        /// Specification how algorithm considers msd tags.
        /// </summary>
        public MsdConsideration eMsdConsider = MsdConsideration.Distinct;
        /// <summary>
        /// How many of the best rules are kept in memory for each node. Zero means unlimited.
        /// </summary>
        public int iMaxRulesPerNode = 0;
        /// <summary>
        /// If true, than build proccess uses few more hevristics to build first left to right lemmatizer (lemmatizes front of the word)
        /// </summary>
        public bool bBuildFrontLemmatizer = false;

        
        // Cloneable functions --------------------------------

        public LemmatizerSettings CloneDeep() {
            return new LemmatizerSettings() {
                bUseFromInRules = this.bUseFromInRules,
                eMsdConsider = this.eMsdConsider,
                iMaxRulesPerNode = this.iMaxRulesPerNode,
                bBuildFrontLemmatizer = this.bBuildFrontLemmatizer                
            };            
        }
        

        // Serialization Functions (ISerializable) -----------

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("bUseFromInRules", bUseFromInRules);
            info.AddValue("eMsdConsider", eMsdConsider);
            info.AddValue("iMaxRulesPerNode", iMaxRulesPerNode);
            info.AddValue("bBuildFrontLemmatizer", bBuildFrontLemmatizer);
        }
        public LemmatizerSettings(SerializationInfo info, StreamingContext context) {
            bUseFromInRules = info.GetBoolean("bUseFromInRules");
            eMsdConsider = (MsdConsideration)info.GetValue("eMsdConsider", typeof(MsdConsideration));
            iMaxRulesPerNode = info.GetInt32("iMaxRulesPerNode");
            bBuildFrontLemmatizer = info.GetBoolean("bBuildFrontLemmatizer");
        }

        // Serialization Functions (regular) ----------------

        public void Serialize(StreamWriter sWrt)
        {
            sWrt.Write(bUseFromInRules); sWrt.Write(Constants.Separator);
            sWrt.Write((int)eMsdConsider); sWrt.Write(Constants.Separator);
            sWrt.Write(iMaxRulesPerNode); sWrt.Write(Constants.Separator);
            sWrt.Write(bBuildFrontLemmatizer); sWrt.Write(Constants.Separator);
            sWrt.WriteLine();
        }

        
        // Serialization Functions (Binary) -----------------

        public void Serialize(BinaryWriter binWrt) {
            binWrt.Write(bUseFromInRules);
            binWrt.Write((int)eMsdConsider);
            binWrt.Write(iMaxRulesPerNode);
            binWrt.Write(bBuildFrontLemmatizer);
        }
        public void Deserialize(BinaryReader binRead) {
            bUseFromInRules = binRead.ReadBoolean();
            eMsdConsider = (MsdConsideration)binRead.ReadInt32();
            iMaxRulesPerNode = binRead.ReadInt32();
            bBuildFrontLemmatizer = binRead.ReadBoolean();
        }
        public LemmatizerSettings(BinaryReader binRead) {
            this.Deserialize(binRead);
        }

        
        // Serialization Functions (Latino) -----------------
        
        #if LATINO

        public void Save(Latino.BinarySerializer binWrt) {
            binWrt.WriteBool(bUseFromInRules);
            binWrt.WriteInt((int)eMsdConsider);
            binWrt.WriteInt(iMaxRulesPerNode);
            binWrt.WriteBool(bBuildFrontLemmatizer);
        }

        public void Load(Latino.BinarySerializer binRead) {
            bUseFromInRules = binRead.ReadBool();
            eMsdConsider = (MsdConsideration)binRead.ReadInt();
            iMaxRulesPerNode = binRead.ReadInt();
            bBuildFrontLemmatizer = binRead.ReadBool();
        }

        public LemmatizerSettings(Latino.BinarySerializer reader) {
            Load(reader);
        }

    #endif

        

    }
}
