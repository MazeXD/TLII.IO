using System;
using System.IO;
using System.Text;
using TLII.IO.Contexts;
using TLII.IO.Exceptions;
using TLII.IO.Formats.Adm;
using TLII.IO.Formats.Adm.Fields;

namespace TLII.IO
{
    public class AdmReader : IDisposable
    {
        private MemoryStream _stream;
        private BinaryReader _reader;
        private Encoding _encoding = Encoding.GetEncoding(1200);
        private bool _disposed = false;

        public AdmReader(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("The file doesn't exist!", fileName);
            }

            byte[] data = File.ReadAllBytes(fileName);

            SetupStream(data);
        }

        public AdmReader(byte[] data)
        {
            SetupStream(data);
        }

        public AdmData Read()
        {
            _stream.Seek(0, SeekOrigin.Begin);

            AdmData data = new AdmData();
            AdmReadingContext context = new AdmReadingContext();

            try
            {
                data.Version = _reader.ReadInt32();

                int stringTableCount = _reader.ReadInt32();
                for (int i = 0; i < stringTableCount; i++)
                {
                    ReadStringTableEntry(ref context);
                }

                data.MainBlock = ReadBlock(ref context);
            }
            catch (Exception e)
            {
                throw new AdmReaderException("Failed to read raw file.", e);
            }

            return data;
        }

        #region PRIVATE METHODS

        private void SetupStream(byte[] data)
        {
            _stream = new MemoryStream(data);
            _reader = new BinaryReader(_stream);
        }

        private void ReadStringTableEntry(ref AdmReadingContext context)
        {
            int key = _reader.ReadInt32();

            string value = ReadString();

            context.StringTable.Add(key, value);
        }

        private AdmBlock ReadBlock(ref AdmReadingContext context)
        {
            AdmBlock block = new AdmBlock();

            block.NameHash = _reader.ReadInt32();

            int fieldCount = _reader.ReadInt32();
            for (int i = 0; i < fieldCount; i++)
            {
                block.Fields.Add(ReadField(ref context));
            }

            int childCount = _reader.ReadInt32();
            for (int i = 0; i < childCount; i++)
            {
                block.Childs.Add(ReadBlock(ref context));
            }

            return block;
        }

        private AdmField ReadField(ref AdmReadingContext context)
        {
            AdmField field;

            int nameHash = _reader.ReadInt32();

            AdmFieldType fieldType;
            if (!Enum.TryParse(_reader.ReadInt32().ToString(), out fieldType))
            {
                throw new AdmReaderException("Unexpected field type");
            }

            switch (fieldType)
            {
                case AdmFieldType.Integer:
                    field = ReadIntegerField(ref context);
                    break;
                case AdmFieldType.Float:
                    field = ReadFloatField(ref context);
                    break;
                case AdmFieldType.Double:
                    field = ReadDoubleField(ref context);
                    break;
                case AdmFieldType.UnsignedInteger:
                    field = ReadUnsignedIntegerField(ref context);
                    break;
                case AdmFieldType.String:
                    field = ReadStringField(ref context);
                    break;
                case AdmFieldType.Boolean:
                    field = ReadBooleanField(ref context);
                    break;
                case AdmFieldType.Integer64:
                    field = ReadInteger64Field(ref context);
                    break;
                case AdmFieldType.Translate:
                    field = ReadTranslateField(ref context);
                    break;
                default:
                    throw new AdmReaderException("Unexpected field type");
            }

            field.NameHash = nameHash;

            return field;
        }

        private AdmIntegerField ReadIntegerField(ref AdmReadingContext context)
        {
            AdmIntegerField field = new AdmIntegerField();
            field.Value = _reader.ReadInt32();

            return field;
        }

        private AdmFloatField ReadFloatField(ref AdmReadingContext context)
        {
            AdmFloatField field = new AdmFloatField();
            field.Value = _reader.ReadSingle();

            return field;
        }

        private AdmDoubleField ReadDoubleField(ref AdmReadingContext context)
        {
            AdmDoubleField field = new AdmDoubleField();
            field.Value = _reader.ReadDouble();

            return field;
        }

        private AdmUnsignedIntegerField ReadUnsignedIntegerField(ref AdmReadingContext context)
        {
            AdmUnsignedIntegerField field = new AdmUnsignedIntegerField();
            field.Value = _reader.ReadUInt32();

            return field;
        }

        private AdmStringField ReadStringField(ref AdmReadingContext context)
        {
            AdmStringField field = new AdmStringField();
            field.Value = context.GetStringByKey(_reader.ReadInt32());

            return field;
        }

        private AdmBooleanField ReadBooleanField(ref AdmReadingContext context)
        {
            AdmBooleanField field = new AdmBooleanField();
            field.Value = _reader.ReadInt32() == 1;

            return field;
        }

        private AdmInteger64Field ReadInteger64Field(ref AdmReadingContext context)
        {
            AdmInteger64Field field = new AdmInteger64Field();
            field.Value = _reader.ReadInt64();

            return field;
        }

        private AdmTranslateField ReadTranslateField(ref AdmReadingContext context)
        {
            AdmTranslateField field = new AdmTranslateField();
            field.Value = context.GetStringByKey(_reader.ReadInt32());

            return field;
        }

        private string ReadString()
        {
            int length = _reader.ReadInt16() * 2;
            byte[] data = _reader.ReadBytes(length);
            return _encoding.GetString(data);
        }

        #endregion

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                _reader.Close();
                _stream.Close();
                _reader.Dispose();
                _stream.Dispose();
            }
        }
    }
}
