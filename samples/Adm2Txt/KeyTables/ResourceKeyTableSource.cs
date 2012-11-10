using System;
using System.IO;
using System.Reflection;

namespace Adm2Txt.KeyTables
{
    class ResourceKeyTableSource : KeyTableSource
    {
        string data = "";

        public ResourceKeyTableSource(string resourceName)
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            string nameSpace = assembly.GetName().Name + ".Resources.";

            Stream stream = assembly.GetManifestResourceStream(nameSpace + resourceName);
            if (stream == null)
            {
                return;
            }

            StreamReader reader = new StreamReader(stream);
            data = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();

            stream.Close();
            stream.Dispose();

            Load();
        }

        public override void Load()
        {
            if (data == "") return;

            string[] lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                AddEntry(line.Trim());
            }
        }
    }
}
