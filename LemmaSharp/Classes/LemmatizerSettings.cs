using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;

namespace LemmaSharp {
    /// <summary>
    /// These are the lemmagen algorithm settings that affect speed/power of the learning and lemmatizing algorithm.
    /// TODO this class will be probbably removed in the future.
    /// </summary>
    [Serializable()]
    public class LemmatizerSettings : ISerializable {
        #region Constructor(s) & Destructor(s)

        public LemmatizerSettings() { } 

        #endregion

        #region Sub-Structures

        /// <summary>
        /// How algorithm considers msd tags.
        /// </summary>
        public enum MsdConsideration {
            /// <summary>
            /// Completely ignores mds tags (join examples with different tags and sum their weihgts).
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

        #endregion

        #region Public Variables

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

        #endregion

        #region Cloneable functions

        public LemmatizerSettings CloneDeep() {
            return new LemmatizerSettings() {
                bUseFromInRules = this.bUseFromInRules,
                eMsdConsider = this.eMsdConsider,
                iMaxRulesPerNode = this.iMaxRulesPerNode,
                bBuildFrontLemmatizer = this.bBuildFrontLemmatizer                
            };            
        }

        #endregion

        #region Serialization Functions (ISerializable)

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

        #endregion
        #region Serialization Functions (Binary)

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
        public LemmatizerSettings(System.IO.BinaryReader binRead) {
            this.Deserialize(binRead);
        }

        #endregion
        #region Serialization Functions (Latino)
        
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

        #endregion

    }
}
