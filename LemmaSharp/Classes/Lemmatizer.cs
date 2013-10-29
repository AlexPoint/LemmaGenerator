using System;
using System.IO;
using System.Runtime.Serialization;
using System.IO.Compression;
using SevenZip;

namespace LemmaSharp.Classes {

    [Serializable]
    public class Lemmatizer : ITrainableLemmatizer 
        #if LATINO
        , Latino.ISerializable 
        #endif 
        {

        // Private Variables -------------------

        protected LemmatizerSettings Lsett;
        protected ExampleList ElExamples;
        protected LemmaTreeNode LtnRootNode;
        protected LemmaTreeNode LtnRootNodeFront;
        

        // Constructor(s) & Destructor(s) ------

        public Lemmatizer() : this(new LemmatizerSettings()) { }
        public Lemmatizer(LemmatizerSettings lsett) { 
            this.Lsett = lsett;
            this.ElExamples = new ExampleList(lsett);
            this.LtnRootNode = null;
            this.LtnRootNodeFront = null;
        } 

        public Lemmatizer(StreamReader srIn, string sFormat, LemmatizerSettings lsett): this(lsett) {
            AddMultextFile(srIn, sFormat);
        }
        

        // Private Properties -----------------

        private LemmaTreeNode LtrRootNodeSafe {
            get {
                if (LtnRootNode == null) BuildModel();
                return LtnRootNode;
            }
        }
        private LemmaTreeNode LtrRootNodeFrontSafe {
            get {
                if (LtnRootNodeFront == null && Lsett.bBuildFrontLemmatizer) BuildModel();
                return LtnRootNodeFront;
            }
        }

        
        // Public Properties ------------------

        public LemmatizerSettings Settings{
            get{
                return Lsett.CloneDeep();
            }
        }
        public ExampleList Examples {
            get {
                return ElExamples;
            }
        }
        public RuleList Rules {
            get {
                return ElExamples.Rules;
            }
        }
        public LemmaTreeNode RootNode {
            get {
                return LtrRootNodeSafe;
            }
        }
        public LemmaTreeNode RootNodeFront {
            get {
                return LtrRootNodeFrontSafe;
            }
        }
        public ILemmatizerModel Model {
            get {
                return LtrRootNodeSafe;
            }
        }

        
        // Essential Class Functions (adding examples to repository) ----------

        public void AddMultextFile(StreamReader srIn, string sFormat) {
            this.ElExamples.AddMultextFile(srIn, sFormat);
            LtnRootNode = null;
        }
        public void AddExample(string sWord, string sLemma) {
            AddExample(sWord, sLemma, 1, null);
        }
        public void AddExample(string sWord, string sLemma, double dWeight) {
            AddExample(sWord, sLemma, dWeight, null);
        }
        public void AddExample(string sWord, string sLemma, double dWeight, string sMsd) {
            ElExamples.AddExample(sWord, sLemma, dWeight, sMsd);
            LtnRootNode = null;
        }

        public void DropExamples() {
            ElExamples.DropExamples();
        }
        public void FinalizeAdditions() {
            ElExamples.FinalizeAdditions();
        }

        
        // Essential Class Functions (building model & lemmatizing) ----------

        public void BuildModel() {
            if (LtnRootNode != null) return;

            if (!Lsett.bBuildFrontLemmatizer) {
                //TODO remove: elExamples.FinalizeAdditions();
                ElExamples.FinalizeAdditions();
                LtnRootNode = new LemmaTreeNode(Lsett, ElExamples);
            }
            else {
                LtnRootNode = new LemmaTreeNode(Lsett, ElExamples.GetFrontRearExampleList(false));
                LtnRootNodeFront = new LemmaTreeNode(Lsett, ElExamples.GetFrontRearExampleList(true));   
            }
        }

        public string Lemmatize(string sWord) {
            if (!Lsett.bBuildFrontLemmatizer)
                return LtrRootNodeSafe.Lemmatize(sWord);
            else {
                string sWordFront = LemmaExample.StringReverse(sWord);
                string sLemmaFront = LtrRootNodeFrontSafe.Lemmatize(sWordFront);
                string sWordRear = LemmaExample.StringReverse(sLemmaFront);
                return LtrRootNodeSafe.Lemmatize(sWordRear);
            }
        }
        

        // Serialization Functions (ISerializable) ---------------------------

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("lsett", Lsett);
            info.AddValue("elExamples", ElExamples);
        }
        public Lemmatizer(SerializationInfo info, StreamingContext context): this() {
            Lsett = (LemmatizerSettings)info.GetValue("lsett", typeof(LemmatizerSettings));
            ElExamples = (ExampleList)info.GetValue("elExamples", typeof(ExampleList));
            this.BuildModel();
        }

        
        // Serialization Functions (Binary) ------------

        public void Serialize(BinaryWriter binWrt, bool bSerializeExamples) {
            Lsett.Serialize(binWrt);

            binWrt.Write(bSerializeExamples);
            ElExamples.Serialize(binWrt, bSerializeExamples, false);

            if (!bSerializeExamples) {
                ElExamples.GetFrontRearExampleList(false).Serialize(binWrt, bSerializeExamples, false);
                ElExamples.GetFrontRearExampleList(true).Serialize(binWrt, bSerializeExamples, false);
            }

            LtnRootNode.Serialize(binWrt);
            if (Lsett.bBuildFrontLemmatizer)
                LtnRootNodeFront.Serialize(binWrt);
        }
        public void Deserialize(BinaryReader binRead) {
            Lsett = new LemmatizerSettings(binRead);

            bool bSerializeExamples = binRead.ReadBoolean();
            ElExamples = new ExampleList(binRead, Lsett);

            ExampleList elExamplesRear;
            ExampleList elExamplesFront;

            if (bSerializeExamples) {
                elExamplesRear = ElExamples.GetFrontRearExampleList(false);
                elExamplesFront = ElExamples.GetFrontRearExampleList(true);
            }
            else {
                elExamplesRear = new ExampleList(binRead, Lsett);
                elExamplesFront = new ExampleList(binRead, Lsett);
            }                

            if (!Lsett.bBuildFrontLemmatizer) {
                LtnRootNode = new LemmaTreeNode(binRead, Lsett, ElExamples, null);
            }
            else {
                LtnRootNode = new LemmaTreeNode(binRead, Lsett,  elExamplesRear, null);
                LtnRootNodeFront = new LemmaTreeNode(binRead, Lsett, elExamplesFront, null);
            }
        }

        //Do not change the order!!! (If new compression algorithms are added, otherwise you will not be able to load old files.)
        public enum Compression {
            None,
            Deflate,
            Lzma
        }

        public Lemmatizer(BinaryReader binRead) {
            var compr = (Compression)binRead.ReadByte();
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
                case Compression.Lzma:
                    SerializeLzma(streamOut, bSerializeExamples);
                    break;
                default:
                    break;
            }
        }

        private void SerializeNone(Stream streamOut, bool bSerializeExamples) {
            var binWrt = new BinaryWriter(streamOut);
            this.Serialize(binWrt, bSerializeExamples);
        }
        private void SerializeDeflate(Stream streamOut, bool bSerializeExamples) {
            Stream streamOutNew = new DeflateStream(streamOut, CompressionMode.Compress, true);
            var binWrt = new BinaryWriter(streamOutNew);
            this.Serialize(binWrt, bSerializeExamples);
            binWrt.Flush();
            binWrt.Close();
        }
        private void SerializeLzma(Stream streamOut, bool bSerializeExamples) {
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

            const int dictionary = 1 << 23;
            const int posStateBits = 2;
            const int litContextBits = 3; // for normal files
            // UInt32 litContextBits = 0; // for 32-bit data
            const int litPosBits = 0;
            // UInt32 litPosBits = 2; // for 32-bit data
            const int algorithm = 2;
            const int numFastBytes = 128;
            const string mf = "bt4";
            const bool eos = false;

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

            var msTemp = new MemoryStream();
            var binWrtTemp = new BinaryWriter(msTemp);
            this.Serialize(binWrtTemp, bSerializeExamples);
            msTemp.Position = 0;

            var encoder = new SevenZip.Compression.LZMA.Encoder();
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
            var compr = (Compression)streamIn.ReadByte();
            Stream streamInNew = Decompress(streamIn, compr);
            var br = new BinaryReader(streamInNew);
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
                case Compression.Lzma:
                    streamInNew = DecompressLZMA(streamIn);
                    break;
            }
            return streamInNew;
        }
        private Stream DecompressLZMA(Stream streamIn) {
            var properties = new byte[5];
            if (streamIn.Read(properties, 0, 5) != 5)
                throw (new Exception("input .lzma is too short"));
            var decoder = new SevenZip.Compression.LZMA.Decoder();
            decoder.SetDecoderProperties(properties);

            long outSize = 0;
            for (int i = 0; i < 8; i++) {
                int v = streamIn.ReadByte();
                if (v < 0)
                    throw (new Exception("Can't Read 1"));
                outSize |= ((long)(byte)v) << (8 * i);
            }
            long compressedSize = streamIn.Length - streamIn.Position;

            var outStream = new MemoryStream();
            decoder.Code(streamIn, outStream, compressedSize, outSize, null);
            outStream.Seek(0, 0);
            return outStream;
        }

        
        // Serialization Functions (Latino) -------------------
        
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

        
    }
}
