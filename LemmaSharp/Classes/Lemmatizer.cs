using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.IO.Compression;
using SevenZip;

namespace LemmaSharp {
    [Serializable()]
    public class Lemmatizer : ITrainableLemmatizer 
        #if LATINO
        , Latino.ISerializable 
        #endif 
        {

        #region Private Variables

        protected LemmatizerSettings lsett;
        protected ExampleList elExamples;
        protected LemmaTreeNode ltnRootNode;
        protected LemmaTreeNode ltnRootNodeFront;

        #endregion

        #region Constructor(s) & Destructor(s)

        public Lemmatizer() : this(new LemmatizerSettings()) { }
        public Lemmatizer(LemmatizerSettings lsett) { 
            this.lsett = lsett;
            this.elExamples = new ExampleList(lsett);
            this.ltnRootNode = null;
            this.ltnRootNodeFront = null;
        } 

        public Lemmatizer(StreamReader srIn, string sFormat, LemmatizerSettings lsett): this(lsett) {
            AddMultextFile(srIn, sFormat);
        }

        #endregion

        #region Private Properties

        private LemmaTreeNode ltrRootNodeSafe {
            get {
                if (ltnRootNode == null) BuildModel();
                return ltnRootNode;
            }
        }
        private LemmaTreeNode ltrRootNodeFrontSafe {
            get {
                if (ltnRootNodeFront == null && lsett.bBuildFrontLemmatizer) BuildModel();
                return ltnRootNodeFront;
            }
        }

        #endregion
        #region Public Properties

        public LemmatizerSettings Settings{
            get{
                return lsett.CloneDeep();
            }
        }
        public ExampleList Examples {
            get {
                return elExamples;
            }
        }
        public RuleList Rules {
            get {
                return elExamples.Rules;
            }
        }
        public LemmaTreeNode RootNode {
            get {
                return ltrRootNodeSafe;
            }
        }
        public LemmaTreeNode RootNodeFront {
            get {
                return ltrRootNodeFrontSafe;
            }
        }
        public ILemmatizerModel Model {
            get {
                return ltrRootNodeSafe;
            }
        }

        #endregion

        #region Essential Class Functions (adding examples to repository)

        public void AddMultextFile(StreamReader srIn, string sFormat) {
            this.elExamples.AddMultextFile(srIn, sFormat);
            ltnRootNode = null;
        }
        public void AddExample(string sWord, string sLemma) {
            AddExample(sWord, sLemma, 1, null);
        }
        public void AddExample(string sWord, string sLemma, double dWeight) {
            AddExample(sWord, sLemma, dWeight, null);
        }
        public void AddExample(string sWord, string sLemma, double dWeight, string sMsd) {
            elExamples.AddExample(sWord, sLemma, dWeight, sMsd);
            ltnRootNode = null;
        }

        public void DropExamples() {
            elExamples.DropExamples();
        }
        public void FinalizeAdditions() {
            elExamples.FinalizeAdditions();
        }

        #endregion
        #region Essential Class Functions (building model & lemmatizing)

        public void BuildModel() {
            if (ltnRootNode != null) return;

            if (!lsett.bBuildFrontLemmatizer) {
                //TODO remove: elExamples.FinalizeAdditions();
                elExamples.FinalizeAdditions();
                ltnRootNode = new LemmaTreeNode(lsett, elExamples);
            }
            else {
                ltnRootNode = new LemmaTreeNode(lsett, elExamples.GetFrontRearExampleList(false));
                ltnRootNodeFront = new LemmaTreeNode(lsett, elExamples.GetFrontRearExampleList(true));   
            }
        }

        public string Lemmatize(string sWord) {
            if (!lsett.bBuildFrontLemmatizer)
                return ltrRootNodeSafe.Lemmatize(sWord);
            else {
                string sWordFront = LemmaExample.StringReverse(sWord);
                string sLemmaFront = ltrRootNodeFrontSafe.Lemmatize(sWordFront);
                string sWordRear = LemmaExample.StringReverse(sLemmaFront);
                return ltrRootNodeSafe.Lemmatize(sWordRear);
            }
        }

        #endregion

        #region Serialization Functions (ISerializable)

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("lsett", lsett);
            info.AddValue("elExamples", elExamples);
        }
        public Lemmatizer(SerializationInfo info, StreamingContext context): this() {
            lsett = (LemmatizerSettings)info.GetValue("lsett", typeof(LemmatizerSettings));
            elExamples = (ExampleList)info.GetValue("elExamples", typeof(ExampleList));
            this.BuildModel();
        }

        #endregion
        #region Serialization Functions (Binary)

        public void Serialize(BinaryWriter binWrt, bool bSerializeExamples) {
            lsett.Serialize(binWrt);

            binWrt.Write(bSerializeExamples);
            elExamples.Serialize(binWrt, bSerializeExamples, false);

            if (!bSerializeExamples) {
                elExamples.GetFrontRearExampleList(false).Serialize(binWrt, bSerializeExamples, false);
                elExamples.GetFrontRearExampleList(true).Serialize(binWrt, bSerializeExamples, false);
            }

            ltnRootNode.Serialize(binWrt);
            if (lsett.bBuildFrontLemmatizer)
                ltnRootNodeFront.Serialize(binWrt);
        }
        public void Deserialize(BinaryReader binRead) {
            lsett = new LemmatizerSettings(binRead);

            bool bSerializeExamples = binRead.ReadBoolean();
            elExamples = new ExampleList(binRead, lsett);

            ExampleList elExamplesRear;
            ExampleList elExamplesFront;

            if (bSerializeExamples) {
                elExamplesRear = elExamples.GetFrontRearExampleList(false);
                elExamplesFront = elExamples.GetFrontRearExampleList(true);
            }
            else {
                elExamplesRear = new ExampleList(binRead, lsett);
                elExamplesFront = new ExampleList(binRead, lsett);
            }                

            if (!lsett.bBuildFrontLemmatizer) {
                ltnRootNode = new LemmaTreeNode(binRead, lsett, elExamples, null);
            }
            else {
                ltnRootNode = new LemmaTreeNode(binRead, lsett,  elExamplesRear, null);
                ltnRootNodeFront = new LemmaTreeNode(binRead, lsett, elExamplesFront, null);
            }
        }

        //Do not change the order!!! (If new compression algorithms are added, otherwise you will not be able to load old files.)
        public enum Compression {
            None,
            Deflate,
            LZMA
        }

        public Lemmatizer(BinaryReader binRead) {
            Compression compr = (Compression)binRead.ReadByte();
            if (compr == Compression.None)
                Deserialize(binRead);
            else
                throw new Exception("Loading lemmatizer with binary reader on uncompressed stream is not supported.");
        }
        public Lemmatizer(Stream streamIn){
            Deserialize(streamIn);
        }

        public void Serialize(Stream streamOut) {
            Serialize(streamOut, true, Compression.None);
        }
        public void Serialize(Stream streamOut, bool bSerializeExamples) {
            Serialize(streamOut, bSerializeExamples, Compression.None);
        }
        public void Serialize(Stream streamOut, bool bSerializeExamples, Compression compress) {
            streamOut.WriteByte((byte)compress);
            switch (compress)
            {   case Compression.None:
                    SerializeNone(streamOut, bSerializeExamples);
                    break;
                case Compression.Deflate:
                    SerializeDeflate(streamOut, bSerializeExamples);
                    break;
                case Compression.LZMA:
                    SerializeLZMA(streamOut, bSerializeExamples);
                    break;
                default:
                    break;
            }
        }

        private void SerializeNone(Stream streamOut, bool bSerializeExamples) {
            BinaryWriter binWrt = new BinaryWriter(streamOut);
            this.Serialize(binWrt, bSerializeExamples);
        }
        private void SerializeDeflate(Stream streamOut, bool bSerializeExamples) {
            Stream streamOutNew = new DeflateStream(streamOut, CompressionMode.Compress, true);
            BinaryWriter binWrt = new BinaryWriter(streamOutNew);
            this.Serialize(binWrt, bSerializeExamples);
            binWrt.Flush();
            binWrt.Close();
        }
        private void SerializeLZMA(Stream streamOut, bool bSerializeExamples) {
            CoderPropID[] propIDs = 
				{
					CoderPropID.DictionarySize,
					CoderPropID.PosStateBits,
					CoderPropID.LitContextBits,
					CoderPropID.LitPosBits,
					CoderPropID.Algorithm,
					CoderPropID.NumFastBytes,
					CoderPropID.MatchFinder,
					CoderPropID.EndMarker
				};

            Int32 dictionary = 1 << 23;
            Int32 posStateBits = 2;
            Int32 litContextBits = 3; // for normal files
            // UInt32 litContextBits = 0; // for 32-bit data
            Int32 litPosBits = 0;
            // UInt32 litPosBits = 2; // for 32-bit data
            Int32 algorithm = 2;
            Int32 numFastBytes = 128;
            string mf = "bt4";
            bool eos = false;

            object[] properties = 
				{
					(Int32)(dictionary),
					(Int32)(posStateBits),
					(Int32)(litContextBits),
					(Int32)(litPosBits),
					(Int32)(algorithm),
					(Int32)(numFastBytes),
					mf,
					eos
				};

            MemoryStream msTemp = new MemoryStream();
            BinaryWriter binWrtTemp = new BinaryWriter(msTemp);
            this.Serialize(binWrtTemp, bSerializeExamples);
            msTemp.Position = 0;

            SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(streamOut);
            Int64 fileSize = msTemp.Length;
            for (int i = 0; i < 8; i++)
                streamOut.WriteByte((Byte)(fileSize >> (8 * i)));
            encoder.Code(msTemp, streamOut, -1, -1, null);
            binWrtTemp.Close();
            msTemp.Close();
        }

        public void Deserialize(Stream streamIn) {
            Compression compr = (Compression)streamIn.ReadByte();
            Stream streamInNew = Decompress(streamIn, compr);
            BinaryReader br = new BinaryReader(streamInNew);
            Deserialize(br);
        }
        private Stream Decompress(Stream streamIn, Compression compress) {
            Stream streamInNew;
            switch (compress) {
                case Compression.None:
                default:
                    streamInNew = streamIn;
                    break;
                case Compression.Deflate:
                    streamInNew = new DeflateStream(streamIn, CompressionMode.Decompress);
                    break;
                case Compression.LZMA:
                    streamInNew = DecompressLZMA(streamIn);
                    break;
            }
            return streamInNew;
        }
        private Stream DecompressLZMA(Stream streamIn) {
            byte[] properties = new byte[5];
            if (streamIn.Read(properties, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
            decoder.SetDecoderProperties(properties);

            long outSize = 0;
            for (int i = 0; i < 8; i++) {
                int v = streamIn.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            long compressedSize = streamIn.Length - streamIn.Position;

            MemoryStream outStream = new MemoryStream();
            decoder.Code(streamIn, outStream, compressedSize, outSize, null);
            outStream.Seek(0, 0);
            return outStream;
        }

        #endregion

        #region Serialization Functions (Latino)
        
        #if LATINO

        public void Save(Latino.BinarySerializer binWrt) {
            lsett.Save(binWrt);
            
            elExamples.Save(binWrt, true, false);

            ltnRootNode.Save(binWrt);
            if (lsett.bBuildFrontLemmatizer)
                ltnRootNodeFront.Save(binWrt);
        }

        public void Load(Latino.BinarySerializer binRead) {
            lsett = new LemmatizerSettings(binRead);
            elExamples = new ExampleList(binRead, lsett);
            if (!lsett.bBuildFrontLemmatizer) {
                ltnRootNode = new LemmaTreeNode(binRead, lsett, elExamples, null);
            }
            else {
                ltnRootNode = new LemmaTreeNode(binRead, lsett, elExamples.GetFrontRearExampleList(false) , null);
                ltnRootNodeFront = new LemmaTreeNode(binRead, lsett, elExamples.GetFrontRearExampleList(true), null);
            }               
        }

        public Lemmatizer(Latino.BinarySerializer binRead) {
            Load(binRead);
        }

        public void Save(Stream streamOut) {
            Latino.BinarySerializer binWrt = new Latino.BinarySerializer(streamOut);
            this.Save(binWrt);
            binWrt.Close();
        }
        public void Load(Stream streamIn) {
            Latino.BinarySerializer binRead = new Latino.BinarySerializer(streamIn);
            Load(binRead);
            binRead.Close();
        }

        public Lemmatizer(Stream streamIn, string sDummy) {
            Load(streamIn);
        }

#endif

        #endregion
    }
}
