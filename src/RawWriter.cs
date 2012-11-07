using System.IO;
using System.Text;
using TLII.IO.Formats.Raw;
using TLII.IO.Formats.Raw.Data;

namespace TLII.IO
{
    /// <summary>
    /// Class for writing Raw files for Torchlight II
    /// </summary>
    public class RawWriter
    {
        #region FIELDS
        
        private MemoryStream _stream;
        private BinaryWriter _writer;
        private Encoding _encoding = Encoding.GetEncoding(1200); // UTF16-LE
        private RawData _file;

        #endregion

        #region CONSTRUCTOR

        public RawWriter(RawData file)
        {
            _file = file;
        }

        #endregion

        #region METHODS

        public void Write(string path)
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);

            string name = null;

            switch (_file.Type)
            {
                case RawType.UnitData:
                    name = "UNITDATA.RAW";
                    WriteUnitDataFile((UnitData)_file);
                    break;
                case RawType.SkillData:
                    name = "SKILLS.RAW";
                    WriteSkillFile((SkillData)_file);
                    break;
                case RawType.AffixData:
                    name = "AFFIXES.RAW";
                    WriteAffixDataFile((AffixData)_file);
                    break;
                case RawType.MissileData:
                    name = "MISSILES.RAW";
                    WriteMissileDataFile((MissileData)_file);
                    break;
                case RawType.RoomPieceData:
                    name = "ROOMPIECES.RAW";
                    WriteRoomPieceDataFile((RoomPieceData)_file);
                    break;
                case RawType.TriggerableData:
                    name = "TRIGGERABLES.RAW";
                    WriteTriggerableDataFile((TriggerableData)_file);
                    break;
                case RawType.UserInterfaceData:
                    name = "UI.RAW";
                    WriteUserInterfaceDataFile((UserInterfaceData)_file);
                    break;
                default:
                    return;
            }

            string fileName = Path.Combine(path, name);

            File.WriteAllBytes(fileName, _stream.ToArray());

            _writer.Close();
            _stream.Close();
            _writer.Dispose();
            _stream.Dispose();
        }

        #endregion

        #region PRIVATE METHODS

        private void WriteUserInterfaceDataFile(UserInterfaceData userInterfaceData)
        {
            _writer.Write(userInterfaceData.UserInterfaces.Count);
            foreach (var userInterface in userInterfaceData.UserInterfaces)
            {
                WriteUserInterface(userInterface);
            }
        }

        private void WriteUserInterface(UserInterface userInterface)
        {
            WriteString(userInterface.Name);
            WriteString(userInterface.File);

            _writer.Write(userInterface.Unknown);
            _writer.Write(userInterface.Unknown2);
            _writer.Write(userInterface.Unknown3);
            _writer.Write(userInterface.Unknown4.Value);
            WriteString(userInterface.Unknown5);
        }

        private void WriteTriggerableDataFile(TriggerableData triggerableData)
        {
            _writer.Write((short)triggerableData.Triggerables.Count);
            foreach (var triggerable in triggerableData.Triggerables)
            {
                WriteTriggerable(triggerable);
            }
        }

        private void WriteTriggerable(Triggerable triggerable)
        {
            WriteString(triggerable.File);
            WriteString(triggerable.Name);
        }

        private void WriteRoomPieceDataFile(RoomPieceData roomPieceData)
        {
            _writer.Write(roomPieceData.LevelSets.Count);
            foreach (var levelset in roomPieceData.LevelSets)
            {
                WriteLevelset(levelset);
            }

            for (int i = 0; i < roomPieceData.LevelSets.Count; i++)
            {
                _writer.Write(roomPieceData.LevelSets[i].GUIDS.Count);
                foreach(var guid in roomPieceData.LevelSets[i].GUIDS)
                {
                    _writer.Write(guid); ;
                }
            }
        }

        private void WriteLevelset(LevelSet levelset)
        {
            WriteString(levelset.File);
        }

        private void WriteMissileDataFile(MissileData missileData)
        {
            _writer.Write((short)missileData.Missiles.Count);
            foreach (var missile in missileData.Missiles)
            {
                WriteMissile(missile);
            }
        }

        private void WriteMissile(Missile missile)
        {
            WriteString(missile.File);

            _writer.Write((byte)missile.Names.Count);
            foreach (var name in missile.Names)
            {
                WriteString(name);
            }
        }

        private void WriteAffixDataFile(AffixData affixData)
        {
            _writer.Write((short)affixData.Affixes.Count);
            foreach (var affix in affixData.Affixes)
            {
                WriteAffix(affix);
            }
        }

        private void WriteAffix(Affix affix)
        {
            WriteString(affix.File);
            WriteString(affix.Name);

            _writer.Write(affix.MinSpawnRange);
            _writer.Write(affix.MaxSpawnRange);

            _writer.Write(affix.Weight);
            _writer.Write(affix.DifficultiesAllowed);

            _writer.Write((byte)affix.UnitTypes.Count);
            foreach (var unitType in affix.UnitTypes)
            {
                WriteString(unitType);
            }

            _writer.Write((byte)affix.NotUnitTypes.Count);
            foreach (var unitType in affix.NotUnitTypes)
            {
                WriteString(unitType);
            }
        }

        private void WriteSkillFile(SkillData skillData)
        {
            _writer.Write(skillData.Skills.Count);
            foreach (var skill in skillData.Skills)
            {
                WriteSkill(skill);
            }
        }

        private void WriteSkill(Skill skill)
        {
            WriteString(skill.Name);
            WriteString(skill.File);

            _writer.Write(skill.GUID);
        }

        private void WriteUnitDataFile(UnitData unitData)
        {
            _writer.Write(unitData.Items.Count);
            foreach (var unit in unitData.Items)
            {
                WriteUnit(unit);
            }

            _writer.Write(unitData.Monsters.Count);
            foreach (var unit in unitData.Monsters)
            {
                WriteUnit(unit);
            }

            _writer.Write(unitData.Players.Count);
            foreach (var unit in unitData.Players)
            {
                WriteUnit(unit);
            }

            _writer.Write(unitData.Props.Count);
            foreach (var unit in unitData.Props)
            {
                WriteUnit(unit);
            }
        }

        private void WriteUnit(Unit unit)
        {
            _writer.Write(unit.GUID);

            WriteString(unit.Name);

            WriteString(unit.File);

            _writer.Write(unit.Unknown);

            _writer.Write(unit.Level);
            _writer.Write(unit.MinLevel);
            _writer.Write(unit.MaxLevel);

            _writer.Write(unit.Rarity);
            _writer.Write(unit.RarityHC);

            WriteString(unit.Type);
        }

        private void WriteString(string str)
        {
            _writer.Write((short)str.Length);
            _writer.Write(_encoding.GetBytes(str));
        }
        #endregion
    }
}
