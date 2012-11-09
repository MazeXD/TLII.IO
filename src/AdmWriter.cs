using System.Collections.Generic;
using System.IO;
using System.Text;
using TLII.IO.Exceptions;
using TLII.IO.Formats.Adm;
using TLII.IO.Formats.Adm.Fields;
using TLII.IO.Utilities;

namespace TLII.IO
{
    /// <summary>
    /// Class for writing Adm files for Torchlight II
    /// </summary>
    public class AdmWriter
    {
        #region FIELDS

        private MemoryStream _stream;
        private BinaryWriter _writer;
        private Encoding _encoding = Encoding.GetEncoding(1200); // UTF16-LE
        private AdmData _file;

        #endregion

        #region CONSTRUCTOR

        public AdmWriter(AdmData file)
        {
            _file = file;
        }

        #endregion

        #region METHODS

        public void Write(string fileName)
        {
            Write(fileName, false);
        }

        public void Write(string fileName, bool overwrite)
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);

            if (File.Exists(fileName) && !overwrite)
            {
                return;
            }
            
            _writer.Write(_file.Version);
            WriteStringTable();

            WriteBlock(_file.MainBlock);

            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllBytes(fileName, _stream.ToArray());

            _writer.Close();
            _stream.Close();
            _writer.Dispose();
            _stream.Dispose();
        }

        #endregion

        #region PRIVATE METHODS

        private void WriteStringTable()
        {
            Dictionary<int, string> stringTable = GenerateStringTable();

            _writer.Write(stringTable.Count);
            foreach(var entry in stringTable)
            {
                _writer.Write(entry.Key);
                WriteString(entry.Value);
            }
        }

        private Dictionary<int, string> GenerateStringTable()
        {
            var result = new Dictionary<int,string>();

            GetStringsByBlock(ref result, _file.MainBlock);

            return result;
        }

        private void GetStringsByBlock(ref Dictionary<int, string> result, AdmBlock block)
        {
            foreach(var field in block.Fields)
            {
                GetString(ref result, field);
            }

            foreach(var bl in block.Childs)
            {
                GetStringsByBlock(ref result, bl);
            }
        }

        private void GetString(ref Dictionary<int,string> result, AdmField field)
        {
            if(field is AdmStringField || field is AdmTranslateField)
            {
                int hash = HashUtility.GenerateHash((string)field.Value);

                if(!result.ContainsKey(hash))
                {
                    result.Add(hash, (string)field.Value);
                }
            }
        }

        private void WriteBlock(AdmBlock block)
        {
            _writer.Write(block.NameHash);
            
            _writer.Write(block.Fields.Count);
            foreach(var field in block.Fields)
            {
                WriteField(field);
            }

            _writer.Write(block.Childs.Count);
            foreach(var child in block.Childs)
            {
                WriteBlock(child);
            }
        }

        private void WriteField(AdmField field)
        {
            _writer.Write(field.NameHash);
            _writer.Write((int)field.Type);
            
            switch(field.Type)
            {
                case AdmFieldType.Integer:
                    WriteIntegerField((AdmIntegerField)field);
                    break;
                case AdmFieldType.Float:
                    WriteFloatField((AdmFloatField)field);
                    break;
                case AdmFieldType.Double:
                    WriteDoubleField((AdmDoubleField)field);
                    break;
                case AdmFieldType.UnsignedInteger:
                    WriteUnsignedIntegerField((AdmUnsignedIntegerField)field);
                    break;
                case AdmFieldType.String:
                    WriteStringField((AdmStringField)field);
                    break;
                case AdmFieldType.Boolean:
                    WriteBooleanField((AdmBooleanField)field);
                    break;
                case AdmFieldType.Integer64:
                    WriteInteger64Field((AdmInteger64Field)field);
                    break;
                case AdmFieldType.Translate:
                    WriteTranslateField((AdmTranslateField)field);
                    break;
                default:
                    throw new AdmWriterException("Unknown field type");
            }
        }
        
        private void WriteIntegerField(AdmIntegerField field)
        {
 	        _writer.Write((int)field.Value);
        }

        private void WriteFloatField(AdmFloatField field)
        {
 	        _writer.Write((float)field.Value);
        }

        private void WriteDoubleField(AdmDoubleField field)
        {
 	        _writer.Write((double)field.Value);
        }

        private void WriteUnsignedIntegerField(AdmUnsignedIntegerField field)
        {
 	        _writer.Write((uint)field.Value);
        }

        private void WriteStringField(AdmStringField field)
        {
 	        _writer.Write(HashUtility.GenerateHash((string)field.Value));
        }

        private void WriteBooleanField(AdmBooleanField field)
        {
            _writer.Write((int)(((bool)field.Value) ? 1 : 0));
        }
        
        private void WriteInteger64Field(AdmInteger64Field field)
        {
 	        _writer.Write((long)field.Value);
        }

        private void WriteTranslateField(AdmTranslateField field)
        {
            _writer.Write(HashUtility.GenerateHash((string)field.Value));
        }
        
        private void WriteString(string str)
        {
            _writer.Write((short)str.Length);
            _writer.Write(_encoding.GetBytes(str));
        }

        #endregion
    }
}
