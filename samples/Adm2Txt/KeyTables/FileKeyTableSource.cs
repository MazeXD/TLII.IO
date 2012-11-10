using System.IO;

namespace Adm2Txt.KeyTables
{
    class FileKeyTableSource : KeyTableSource
    {
        private string _fileName;

        public FileKeyTableSource(string fileName)
        {
            _fileName = fileName;

            Load();
        }

        public override void Load()
        {
            if (!File.Exists(_fileName)) return;

            string[] lines = File.ReadAllLines(_fileName);

            foreach (var line in lines)
            {
                AddEntry(line.Trim());
            }
        }
    }
}
