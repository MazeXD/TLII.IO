using System;
using System.IO;
using System.Text;
using TLII.IO.Exceptions;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO
{
    /// <summary>
    /// Class for reading Raw files of Torchlight II
    /// </summary>
    public class RawReader : IDisposable
    {
        #region FIELDS
        
        private MemoryStream _stream;
        private BinaryReader _reader;
        private Encoding _encoding = Encoding.GetEncoding(1200); // UTF16-LE
        private RawType _type;
        protected string _filename;
        private bool _disposed = false;

        #endregion

        #region CONSTRUCTOR

        public RawReader(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("The file doesn't exist!", fileName);
            }

            bool handled = true;
            string name = Path.GetFileNameWithoutExtension(fileName);
            switch (name.ToUpperInvariant())
            {
                case "UNITDATA":
                    _type = RawType.UnitData;
                    break;
                case "SKILLS":
                    _type = RawType.SkillData;
                    break;
                case "AFFIXES":
                    _type = RawType.AffixData;
                    break;
                case "MISSILES":
                    _type = RawType.MissileData;
                    break;
                case "ROOMPIECES":
                    _type = RawType.RoomPieceData;
                    break;
                case "TRIGGERABLES":
                    _type = RawType.TriggerableData;
                    break;
                case "UI":
                    _type = RawType.UserInterfaceData;
                    break;
                default:
                    handled = false;
                    break;
            }

            if (!handled) {
                if (name.StartsWith("UNITDATA")) {
                    _type = RawType.UnitData;
                    handled = true;
                } else {
                    throw new RawReaderException(String.Format("That type of raw file [{0}] is not supported.\r\nNote: for custom UNITDATA files, name has to start with 'UNITDATA'.", name));
                }
            }

            _filename = fileName;

            byte[] data = File.ReadAllBytes(fileName);

            SetupStream(data);
        }

        public RawReader(byte[] data, RawType type)
        {
            _type = type;
            SetupStream(data);
        }
        #endregion

        #region METHODS

        public RawData Read()
        {
            _stream.Seek(0, SeekOrigin.Begin);

            RawData file;

            try
            {
                switch (_type)
                {
                    case RawType.UnitData:
                        file = ReadUnitDataFile();
                        break;
                    case RawType.SkillData:
                        file = ReadSkillDataFile();
                        break;
                    case RawType.AffixData:
                        file = ReadAffixDataFile();
                        break;
                    case RawType.MissileData:
                        file = ReadMissileDataFile();
                        break;
                    case RawType.RoomPieceData:
                        file = ReadRoomPieceDataFile();
                        break;
                    case RawType.TriggerableData:
                        file = ReadTriggerableDataFile();
                        break;
                    case RawType.UserInterfaceData:
                        file = ReadUserInterfaceDataFile();
                        break;
                    default:
                        file = null;
                        break;
                }
            }
            catch(Exception e)
            {
                throw new RawReaderException("Failed to read raw file.", e);
            }

            return file;
        }

        #endregion

        #region PRIVATE METHODS

        private void SetupStream(byte[] data)
        {
            _stream = new MemoryStream(data);
            _reader = new BinaryReader(_stream);
        }

        private UserInterfaceData ReadUserInterfaceDataFile()
        {
            UserInterfaceData rawFile = new UserInterfaceData();

            int uiCount = _reader.ReadInt32();
            for (int i = 0; i < uiCount; i++)
            {
                rawFile.UserInterfaces.Add(ReadUserInterface());
            }

            return rawFile;
        }

        private UserInterface ReadUserInterface()
        {
            UserInterface userInterface = new UserInterface();

            userInterface.Name = ReadString();
            userInterface.File = ReadString();

            userInterface.Unknown = _reader.ReadInt32();
            userInterface.Unknown2 = _reader.ReadInt32();
            userInterface.Unknown3 = _reader.ReadInt16();
            userInterface.Unknown4 = _reader.ReadBoolean();
            userInterface.Unknown5 = ReadString();

            return userInterface;
        }

        private TriggerableData ReadTriggerableDataFile()
        {
            TriggerableData rawFile = new TriggerableData();

            int triggerableCount = _reader.ReadInt16();
            for (int i = 0; i < triggerableCount; i++)
            {
                rawFile.Triggerables.Add(ReadTriggerable());
            }

            return rawFile;
        }

        private Triggerable ReadTriggerable()
        {
            Triggerable triggerable = new Triggerable();

            triggerable.File = ReadString();
            triggerable.Name = ReadString();

            return triggerable;
        }

        private RoomPieceData ReadRoomPieceDataFile()
        {
            RoomPieceData rawFile = new RoomPieceData();

            int levelSetCount = _reader.ReadInt32();
            for (int i = 0; i < levelSetCount; i++)
            {
                rawFile.LevelSets.Add(ReadLevelset());
            }

            for (int i = 0; i < levelSetCount; i++)
            {
                int guidCount = _reader.ReadInt32();
                for (int j = 0; j < guidCount; j++)
                {
                    rawFile.LevelSets[i].GUIDS.Add(_reader.ReadInt64());
                }
            }

            return rawFile;
        }

        private LevelSet ReadLevelset()
        {
            LevelSet levelSet = new LevelSet();

            levelSet.File = ReadString();

            return levelSet;
        }

        private MissileData ReadMissileDataFile()
        {
            MissileData rawFile = new MissileData();

            int missileCount = _reader.ReadInt16();
            for (int i = 0; i < missileCount; i++)
            {
                rawFile.Missiles.Add(ReadMissile());
            }

            return rawFile;
        }

        private Missile ReadMissile()
        {
            Missile missile = new Missile();

            missile.File = ReadString();

            int nameCount = _reader.ReadByte();
            for (int i = 0; i < nameCount; i++)
            {
                missile.Names.Add(ReadString());
            }

            return missile;
        }

        private AffixData ReadAffixDataFile()
        {
            AffixData rawFile = new AffixData();

            int affixCount = _reader.ReadInt16();
            for (int i = 0; i < affixCount; i++)
            {
                rawFile.Affixes.Add(ReadAffix());
            }

            return rawFile;
        }

        private Affix ReadAffix()
        {
            Affix affix = new Affix();

            affix.File = ReadString();
            affix.Name = ReadString();

            affix.MinSpawnRange = _reader.ReadInt32();
            affix.MaxSpawnRange = _reader.ReadInt32();

            affix.Weight = _reader.ReadInt32();
            affix.DifficultiesAllowed = _reader.ReadInt32();

            int unitTypeCount = _reader.ReadByte();
            for (int i = 0; i < unitTypeCount; i++)
            {
                affix.UnitTypes.Add(ReadString());
            }

            int notUnitTypeCount = _reader.ReadByte();
            for (int i = 0; i < notUnitTypeCount; i++)
            {
                affix.NotUnitTypes.Add(ReadString());
            }

            return affix;
        }

        private SkillData ReadSkillDataFile()
        {
            SkillData rawFile = new SkillData();

            int skillCount = _reader.ReadInt32();
            for (int i = 0; i < skillCount; i++)
            {
                rawFile.Skills.Add(ReadSkill());
            }

            return rawFile;
        }

        private Skill ReadSkill()
        {
            Skill skill = new Skill();

            skill.Name = ReadString();
            skill.File = ReadString();

            skill.GUID = _reader.ReadInt64();

            return skill;
        }

        private UnitData ReadUnitDataFile()
        {
            UnitData rawFile = new UnitData();

            int itemCount = _reader.ReadInt32();
            for (int i = 0; i < itemCount; i++)
            {
                rawFile.Items.Add(ReadUnit());
            }

            int monsterCount = _reader.ReadInt32();
            for (int i = 0; i < monsterCount; i++)
            {
                rawFile.Monsters.Add(ReadUnit());
            }

            int playerCount = _reader.ReadInt32();
            for (int i = 0; i < playerCount; i++)
            {
                rawFile.Players.Add(ReadUnit());
            }

            int propCount = _reader.ReadInt32();
            for (int i = 0; i < propCount; i++)
            {
                rawFile.Props.Add(ReadUnit());
            }

            return rawFile;
        }

        private Unit ReadUnit()
        {
            Unit unit = new Unit();

            unit.GUID = _reader.ReadInt64();

            unit.Name = ReadString();
            unit.File = ReadString();

            unit.Unknown = _reader.ReadByte();

            unit.Level = _reader.ReadInt32();
            unit.MinLevel = _reader.ReadInt32();
            unit.MaxLevel = _reader.ReadInt32();

            unit.Rarity = _reader.ReadInt32();
            unit.RarityHC = _reader.ReadInt32();

            unit.Type = ReadString();

            return unit;
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
