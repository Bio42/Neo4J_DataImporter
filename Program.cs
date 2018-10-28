using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Neo4J_DataImporter
{
    class Program
    {
        public static string user = "neo4j";
        public static string password = "password";
        public static string uri = @"bolt://localhost:7687";

        private static readonly string CsvPath = @"C:\Users\Björn Fernkorn\Documents\Neo4j\K58_MFCS.csv";

        static void Main(string[] args)
        {
            WriteInfo("Hello WORLD!");
            WriteInfo("Gathering DATA...");

            /// Read Data
            var nodes = new List<Node>();
            Write("Reading Data from: " + CsvPath + " ...");

            List<DataRow> data = DataReader.ReadCSV(CsvPath); 
            
            WriteSuccess("Finished Reading Data!");
            WriteSuccess("Data rows: " + data.Count);
            Write();


            /// Calculate Nodes
            Write("Calculating nodes!");

            //nodes = CreateDummyData();
            nodes = DataHandler.CalculateNodes(data);

            WriteSuccess("Calculating Nodes succesfull!");
            WriteSuccess("Calculated Nodes: " + nodes.Count);
            Write();

            WriteInfo("Writing to DB...");
            
            // Write to DB
            using (Neo4JConnector connector = new Neo4JConnector(uri, user, password))
            {
                connector.CreateNodes(nodes);
                WriteSuccess("Nodes succesfully created in DB!");
            }

            // Calculate Realtionships
            Write("Calculating Realtionships...");
            var relationships = DataHandler.CalculateRelationships(nodes);
            WriteSuccess("Calculating relationships done!");

            WriteInfo("Writing to DB...");
            using (Neo4JConnector connector = new Neo4JConnector(uri, user, password))
            {
                connector.CreateRelationships(relationships);
                WriteSuccess("Relationships succesfully created in DB!");
            }

            WriteSuccess("Programm ended successfuly!");
            WriteInfo("-----------------------------------------------");
            Write("Press ENTER to EXIT...");
            Console.ReadLine();
        }

        private static List<Node> CreateDummyData()
        {
            return new List<Node> {
                new Node("TESTNODE1"){Properties = new List<Property>(){
                    new Property("NameOfTestProperty1", "ValueOfTestProperty1"),
                    new Property("NameOfTestProperty2", "ValueOfTestProperty2")} }
                
            };
        }

        #region helpers

        private static void Write(string text = "")
        {
            Console.WriteLine(text);
        }

        private static void WriteSuccess(string text)
        {
            var standartColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = standartColor;
        }

        private static void WriteFailure(string text)
        {
            var standartColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = standartColor;
        }

        private static void WriteInfo(string text)
        {
            var standartColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = standartColor;
        }

        #endregion
    }
}
