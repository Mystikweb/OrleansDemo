using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace OrleansDemo.Server
{
    public class ClusterSetup
    {
        #region Singleton
        private static readonly Lazy<ClusterSetup> lazy = new Lazy<ClusterSetup>(() => new ClusterSetup());

        public static ClusterSetup Instance { get { return lazy.Value; } }
        #endregion

        private string _connectionString;
        private string _fileName = "OrleansConfiguration.xml";

        public string BuildClusterConfiguration()
        {
            XDocument baseConfig = LoadXmlFile("OrleansDemo.Server", "Base_ClusterConfiguration.xml");

            XElement xConfig = baseConfig.Elements().FirstOrDefault(x => x.Name.LocalName == "OrleansConfiguration");
            XElement xGlobals = xConfig.Elements().FirstOrDefault(x => x.Name.LocalName == "Globals");
            XElement xSystemStore = xGlobals.Elements().FirstOrDefault(x => x.Name.LocalName == "SystemStore");

            _connectionString = xSystemStore.Attribute("DataConnectionString").Value;

            List<string> modules = GetActiveModuleNamespaces();

            string result = Path.Combine(Directory.GetCurrentDirectory(), _fileName);
            using (var stream = new FileStream(result, FileMode.Create))
            {
                baseConfig.Save(stream);
            }

            return result;
        }

        private List<string> GetActiveModuleNamespaces()
        {
            List<string> results = new List<string>();

            return results;
        }

        private XDocument LoadXmlFile(string assemblyName, string fileName)
        {
            string fileString = GetResourceTextFile(assemblyName, fileName);

            XDocument result = XDocument.Parse(fileString);

            return result;
        }

        private string GetResourceTextFile(string assemblyName, string filename)
        {
            string result = string.Empty;

            Assembly assembly = Assembly.Load(assemblyName);

            using (Stream stream = assembly.GetManifestResourceStream($"{assemblyName}.{filename}"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }

            return result;
        }
    }
}
