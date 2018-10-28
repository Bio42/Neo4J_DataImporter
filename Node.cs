using System.Collections.Generic;

namespace Neo4J_DataImporter
{
    public class Node
    {
        public string Name;
        public int ID;
        public List<Property> Properties = new List<Property>();

        public Node(string name, int id)
        {
            Name = name;
            ID = id;            
        }

        public Node(string name)
        {
            Name = name;
            ID = (0-1);
        }
    }
}
