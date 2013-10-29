/*==========================================================================;
 *
 *  (c) 2004-08 JSI.  All rights reserved.
 *
 *  File:          BinarySerializer.cs
 *  Version:       1.0
 *  Desc:		   Binary serializer
 *  Author:        Miha Grcar
 *  Created on:    Oct-2004
 *  Last modified: May-2008
 *  Revision:      May-2008
 *
 ***************************************************************************/

//Remark: Use this file as Latino compatibility checker. When it is included in
//  the project it defines symbol LATINO, that should enable all Latino specific 
//  serialization functions. When excluded, this code will not be created and also
//  following Latino namspace will not be added to the project.


using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;

#if LATINO

namespace Latino
{
    /* .-----------------------------------------------------------------------
       |
       |  Class BinarySerializer
       |
       '-----------------------------------------------------------------------
    */
    public interface ISerializable {
        // *** note that you need to implement a constructor that loads the instance if the class implements Latino.ISerializable
        void Save(Latino.BinarySerializer writer);
    }

    public class BinarySerializer
    {
        private static Dictionary<string, string> m_full_to_short_type_name
            = new Dictionary<string, string>();
        private static Dictionary<string, string> m_short_to_full_type_name
            = new Dictionary<string, string>();
        private Stream m_stream;
        private string m_data_dir
            = ".";
        private static void RegisterTypeName(string full_type_name, string short_type_name)
        {
            m_full_to_short_type_name.Add(full_type_name, short_type_name);
            m_short_to_full_type_name.Add(short_type_name, full_type_name);
        }
        private static string GetFullTypeName(string short_type_name)
        {
            return m_short_to_full_type_name.ContainsKey(short_type_name) ? m_short_to_full_type_name[short_type_name] : short_type_name;
        }
        private static string GetShortTypeName(string full_type_name)
        {
            return m_full_to_short_type_name.ContainsKey(full_type_name) ? m_full_to_short_type_name[full_type_name] : full_type_name;
        }
        static BinarySerializer()
        {
            RegisterTypeName(typeof(bool).AssemblyQualifiedName, "b");
            RegisterTypeName(typeof(byte).AssemblyQualifiedName, "ui1");
            RegisterTypeName(typeof(sbyte).AssemblyQualifiedName, "i1");
            RegisterTypeName(typeof(char).AssemblyQualifiedName, "c");
            RegisterTypeName(typeof(double).AssemblyQualifiedName, "f8");
            RegisterTypeName(typeof(float).AssemblyQualifiedName, "f4");
            RegisterTypeName(typeof(int).AssemblyQualifiedName, "i4");
            RegisterTypeName(typeof(uint).AssemblyQualifiedName, "ui4");
            RegisterTypeName(typeof(long).AssemblyQualifiedName, "i8");
            RegisterTypeName(typeof(ulong).AssemblyQualifiedName, "ui8");
            RegisterTypeName(typeof(short).AssemblyQualifiedName, "i2");
            RegisterTypeName(typeof(ushort).AssemblyQualifiedName, "ui2");
            RegisterTypeName(typeof(string).AssemblyQualifiedName, "s");
        }
        public BinarySerializer(Stream stream)
        {
            //Utils.ThrowException(stream == null ? new ArgumentNullException("stream") : null);
            m_stream = stream;
        }
        public BinarySerializer()
        {
            m_stream = new MemoryStream();
        }
        public BinarySerializer(string file_name, FileMode file_mode)
        {
            m_stream = new FileStream(file_name, file_mode); // throws ArgumentException, NotSupportedException, ArgumentNullException, SecurityException, FileNotFoundException, IOException, DirectoryNotFoundException, PathTooLongException, ArgumentOutOfRangeException
        }
        // *** Reading ***
        private byte[] Read<T>() // Read<T>() is directly or indirectly called from several methods thus exceptions thrown here can also be thrown in all those methods
        {
            int sz = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[sz];
            int num_bytes = m_stream.Read(buffer, 0, sz); // throws IOException, NotSupportedException, ObjectDisposedException
            //Utils.ThrowException(num_bytes < sz ? new EndOfStreamException() : null);
            return buffer;
        }
        public bool ReadBool()
        {
            return ReadByte() != 0;
        }
        public byte ReadByte() // ReadByte() is directly or indirectly called from several methods thus exceptions thrown here can also be thrown in all those methods
        {
            int val = m_stream.ReadByte(); // throws NotSupportedException, ObjectDisposedException
            //Utils.ThrowException(val < 0 ? new EndOfStreamException() : null);
            return (byte)val;
        }
        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }
        private char ReadChar8()
        {
            return (char)ReadByte();
        }
        private char ReadChar16()
        {
            return BitConverter.ToChar(Read<ushort>(), 0);
        }
        public char ReadChar()
        {
            return ReadChar16();
        }
        public double ReadDouble()
        {
            return BitConverter.ToDouble(Read<double>(), 0);
        }
        public float ReadFloat()
        {
            return BitConverter.ToSingle(Read<float>(), 0);
        }
        public int ReadInt()
        {
            return BitConverter.ToInt32(Read<int>(), 0);
        }
        public uint ReadUInt()
        {
            return BitConverter.ToUInt32(Read<uint>(), 0);
        }
        public long ReadLong()
        {
            return BitConverter.ToInt64(Read<long>(), 0);
        }
        public ulong ReadULong()
        {
            return BitConverter.ToUInt64(Read<ulong>(), 0);
        }
        public short ReadShort()
        {
            return BitConverter.ToInt16(Read<short>(), 0);
        }
        public ushort ReadUShort()
        {
            return BitConverter.ToUInt16(Read<ushort>(), 0);
        }
        private string ReadString8()
        {
            int len = ReadInt();
            if (len < 0) { return null; }
            byte[] buffer = new byte[len];
            m_stream.Read(buffer, 0, len); // throws IOException, NotSupportedException, ObjectDisposedException
            return Encoding.ASCII.GetString(buffer);
        }
        private string ReadString16()
        {
            int len = ReadInt();
            if (len < 0) { return null; }
            byte[] buffer = new byte[len * 2];
            m_stream.Read(buffer, 0, len * 2); // throws IOException, NotSupportedException, ObjectDisposedException
            return Encoding.Unicode.GetString(buffer);
        }
        public string ReadString()
        {
            return ReadString16(); // throws exceptions (see ReadString16())
        }
        public Type ReadType()
        {
            string type_name = ReadString8(); // throws exceptions (see ReadString8())
            //Utils.ThrowException(type_name == null ? new InvalidDataException() : null);
            return Type.GetType(GetFullTypeName(type_name)); // throws TargetInvocationException, ArgumentException, TypeLoadException, FileNotFoundException, FileLoadException, BadImageFormatException
        }
        public ValueType ReadValue(Type type)
        {
            //Utils.ThrowException(type == null ? new ArgumentNullException("type") : null);
            //Utils.ThrowException(!type.IsValueType ? new InvalidArgumentValueException("type") : null);
            if (type == typeof(bool))
            {
                return ReadBool();
            }
            else if (type == typeof(byte))
            {
                return ReadByte();
            }
            else if (type == typeof(sbyte))
            {
                return ReadSByte();
            }
            else if (type == typeof(char))
            {
                return ReadChar();
            }
            else if (type == typeof(double))
            {
                return ReadDouble();
            }
            else if (type == typeof(float))
            {
                return ReadFloat();
            }
            else if (type == typeof(int))
            {
                return ReadInt();
            }
            else if (type == typeof(uint))
            {
                return ReadUInt();
            }
            else if (type == typeof(long))
            {
                return ReadLong();
            }
            else if (type == typeof(ulong))
            {
                return ReadULong();
            }
            else if (type == typeof(short))
            {
                return ReadShort();
            }
            else if (type == typeof(ushort))
            {
                return ReadUShort();
            }
            else if (typeof(Latino.ISerializable).IsAssignableFrom(type))
            {
                ConstructorInfo cxtor = type.GetConstructor(new Type[] { typeof(Latino.BinarySerializer) });
                //Utils.ThrowException(cxtor == null ? new ArgumentNotSupportedException("type") : null);
                return (ValueType)cxtor.Invoke(new object[] { this }); // throws MemberAccessException, MethodAccessException, TargetInvocationException, NotSupportedException, SecurityException
            }
            else
            {
                //throw new ArgumentNotSupportedException("type");
                throw new Exception("type");
            }
        }
        public T ReadValue<T>()
        {
            return (T)(object)ReadValue(typeof(T)); // throws exceptions (see ReadValue(Type type))
        }
        public object ReadObject(Type type)
        {
            //Utils.ThrowException(type == null ? new ArgumentNullException("type") : null);
            switch (ReadByte())
            {
                case 0:
                    return null;
                case 1:
                    break;
                case 2:
                    Type type_0 = ReadType(); // throws exceptions (see ReadType())
                    //Utils.ThrowException(type_0 == null ? new TypeLoadException() : null); 
                    //Utils.ThrowException(!type.IsAssignableFrom(type_0) ? new InvalidArgumentValueException("type") : null);
                    type = type_0;
                    break;
                default:
                    throw new InvalidDataException();
            }
            if (type == typeof(string))
            {
                return ReadString();
            }
            else if (typeof(Latino.ISerializable).IsAssignableFrom(type))
            {
                ConstructorInfo cxtor = type.GetConstructor(new Type[] { typeof(Latino.BinarySerializer) });
                //Utils.ThrowException(cxtor == null ? new ArgumentNotSupportedException("type") : null);
                return cxtor.Invoke(new object[] { this }); // throws MemberAccessException, MethodAccessException, TargetInvocationException, NotSupportedException, SecurityException
            }
            else if (type.IsValueType)
            {
                return ReadValue(type); // throws exceptions (see ReadValue(Type type))
            }
            else
            {
                //throw new InvalidArgumentValueException("type");
                throw new Exception("type");
            }
        }
        public T ReadObject<T>()
        {
            return (T)ReadObject(typeof(T)); // throws exceptions (see ReadObject(Type type))
        }
        public object ReadValueOrObject(Type type)
        {
            //Utils.ThrowException(type == null ? new ArgumentNullException("type") : null);
            if (type.IsValueType)
            {
                return ReadValue(type); // throws exceptions (see ReadValue(Type type))
            }
            else
            {
                return ReadObject(type); // throws exceptions (see ReadObject(Type type))
            }
        }
        public T ReadValueOrObject<T>()
        {
            return (T)ReadValueOrObject(typeof(T)); // throws exceptions (see ReadValueOrObject(Type type))
        }
        // *** Writing ***
        private void Write(byte[] data) // Write(byte[] data) is directly or indirectly called from several methods thus exceptions thrown here can also be thrown in all those methods
        {
            m_stream.Write(data, 0, data.Length); // throws IOException, NotSupportedException, ObjectDisposedException
        }
        public void WriteBool(bool val)
        {
            WriteByte(val ? (byte)1 : (byte)0);
        }
        public void WriteByte(byte val) // WriteByte(byte val) is directly or indirectly called from several methods thus exceptions thrown here can also be thrown in all those methods
        {
            m_stream.WriteByte(val); // throws IOException, NotSupportedException, ObjectDisposedException
        }
        public void WriteSByte(sbyte val)
        {
            WriteByte((byte)val);
        }
        private void WriteChar8(char val)
        {
            WriteByte(Encoding.ASCII.GetBytes(new char[] { val })[0]);
        }
        private void WriteChar16(char val)
        {
            Write(BitConverter.GetBytes((ushort)val));
        }
        public void WriteChar(char val)
        {
            WriteChar16(val);
        }
        public void WriteDouble(double val)
        {
            Write(BitConverter.GetBytes(val));
        }
        public void WriteFloat(float val)
        {
            Write(BitConverter.GetBytes(val));
        }
        public void WriteInt(int val)
        {
            Write(BitConverter.GetBytes(val));
        }
        public void WriteUInt(uint val)
        {
            Write(BitConverter.GetBytes(val));
        }
        public void WriteLong(long val)
        {
            Write(BitConverter.GetBytes(val));
        }
        public void WriteULong(ulong val)
        {
            Write(BitConverter.GetBytes(val));
        }
        public void WriteShort(short val)
        {
            Write(BitConverter.GetBytes(val));
        }
        public void WriteUShort(ushort val)
        {
            Write(BitConverter.GetBytes(val));
        }
        private void WriteString8(string val)
        {
            if (val == null) { WriteInt(-1); return; }
            WriteInt(val.Length);
            Write(Encoding.ASCII.GetBytes(val));
        }
        private void WriteString16(string val)
        {
            if (val == null) { WriteInt(-1); return; }
            WriteInt(val.Length);
            Write(Encoding.Unicode.GetBytes(val));
        }
        public void WriteString(string val)
        {
            WriteString16(val);
        }
        public void WriteValue(ValueType val)
        {
            if (val is bool)
            {
                WriteBool((bool)val);
            }
            else if (val is byte)
            {
                WriteByte((byte)val);
            }
            else if (val is sbyte)
            {
                WriteSByte((sbyte)val);
            }
            else if (val is char)
            {
                WriteChar((char)val);
            }
            else if (val is double)
            {
                WriteDouble((double)val);
            }
            else if (val is float)
            {
                WriteFloat((float)val);
            }
            else if (val is int)
            {
                WriteInt((int)val);
            }
            else if (val is uint)
            {
                WriteUInt((uint)val);
            }
            else if (val is long)
            {
                WriteLong((long)val);
            }
            else if (val is ulong)
            {
                WriteULong((ulong)val);
            }
            else if (val is short)
            {
                WriteShort((short)val);
            }
            else if (val is ushort)
            {
                WriteUShort((ushort)val);
            }
            else if (val is Latino.ISerializable)
            {
                ((Latino.ISerializable)val).Save(this); // throws serialization-related exceptions
            }
            else
            {
                //throw new ArgumentTypeException("val");
            }
        }
        public void WriteObject(Type type, object obj)
        {
            //Utils.ThrowException(type == null ? new ArgumentNullException("type") : null);
            //Utils.ThrowException((obj != null && !type.IsAssignableFrom(obj.GetType())) ? new ArgumentTypeException("obj") : null);
            if (obj == null)
            {
                WriteByte(0);
            }
            else
            {
                Type obj_type = obj.GetType();
                if (obj_type == type)
                {
                    WriteByte(1);
                }
                else
                {
                    WriteByte(2);
                    WriteType(obj_type);
                }
                if (obj is string)
                {
                    WriteString((string)obj);
                }
                else if (obj is Latino.ISerializable)
                {
                    ((Latino.ISerializable)obj).Save(this); // throws serialization-related exceptions
                }
                else if (obj is ValueType)
                {
                    WriteValue((ValueType)obj); // throws exceptions (see WriteValue(ValueType val))
                }
                else
                {
                    //throw new ArgumentTypeException("obj");
                }
            }
        }
        public void WriteObject<T>(T obj)
        {
            WriteObject(typeof(T), obj); // throws exceptions (see WriteObject(Type type, object obj))
        }
        public void WriteValueOrObject(Type type, object obj)
        {
            //Utils.ThrowException(type == null ? new ArgumentNullException("type") : null);
            //Utils.ThrowException(!type.IsAssignableFrom(obj.GetType()) ? new ArgumentTypeException("obj") : null);
            if (type.IsValueType)
            {
                WriteValue((ValueType)obj); // throws exceptions (see WriteValue(ValueType val))
            }
            else
            {
                WriteObject(type, obj); // throws exceptions (see WriteObject(Type type, object obj))
            }
        }
        public void WriteValueOrObject<T>(T obj)
        {
            WriteValueOrObject(typeof(T), obj); // throws exceptions (see WriteValueOrObject(Type type, object obj))
        }
        public void WriteType(Type type)
        {
            //Utils.ThrowException(type == null ? new ArgumentNullException("type") : null);
            WriteString8(GetShortTypeName(type.AssemblyQualifiedName)); 
        }
        // *** Data directory ***
        public string DataDir
        {
            get { return m_data_dir; }
            set 
            {
                //Utils.ThrowException(!Utils.VerifyPathName(value, /*must_exist=*/true) ? new InvalidArgumentValueException("DataDir") : null);
                m_data_dir = value;
            }
        }
        // *** Access to the associated stream ***
        public void Close()
        {
            m_stream.Close();
        }
        public void Flush()
        {
            m_stream.Flush(); // throws IOException
        }
        public Stream Stream
        {
            get { return m_stream; }
        }
    }
}

#endif
