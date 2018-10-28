using System;
using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;

namespace Neo4J_DataImporter
{
    public class Neo4JConnector : IDisposable
    {
        private readonly IDriver dbDriver;

        public Neo4JConnector(string uri, string user, string password)
        {
            dbDriver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public void PrintGreeting(string message)
        {
            using (var session = dbDriver.Session())
            {
                var greeting = session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (a:Greeting) " +
                                        "SET a.message = $message " +
                                        "RETURN a.message + ', from node ' + id(a)",
                        new { message });
                    return result.Single()[0].As<string>();
                });
                Console.WriteLine(greeting);
            }
        }

        public int CreateNodes(List<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                CreateNode(node);
            }
            return 0;
        }

        public void CreateNode(Node node)
        {
            string statement = CreateCreateStatement(node);
            using (var session = dbDriver.Session())
            {
                int IdOfCreatedNode = session.WriteTransaction(tx =>
                {
                    var result = tx.Run(statement + "RETURN id(n)", new { node.Name });
                    return result.Single()[0].As<int>();
                });
                node.ID = IdOfCreatedNode;
            }
        }

        

        internal void CreateRelationship(Relationship relationship)
        {
            string statement = CreateRelationshipStatement(relationship);
            using (var session = dbDriver.Session())
            {
                int IdOfCreatedRelationship = session.WriteTransaction(tx =>
                {
                    var result = tx.Run(statement + "RETURN id(r)", new { relationship.Name });
                    return result.Single()[0].As<int>();
                });
                relationship.Id = IdOfCreatedRelationship;
            }
        }

        internal void CreateRelationships(List<Relationship> relationships)
        {
            foreach (var item in relationships)
            {
                CreateRelationship(item);
            }
        }

        private string CreateCreateStatement(Node node)
        {
            string statement = "";
            // Create (Node Name)
            statement += string.Format("CREATE (n:{0}) ", node.Name);

            // SET (Properties)
            foreach (Property prop in node.Properties)
            {
                statement += string.Format("SET n.{0} = \'{1}\' ", prop.Name, prop.Value);
            }

            return statement;
        }

        private string CreateRelationshipStatement(Relationship rel)
        {
            
            /// MATCH (a: Sample), (b: Batch)
            /// WHERE id(a) = $$$ and id(b) = $$$
            /// CREATE(a) -[r: $$$]->(b)
            string statement = "MATCH (a: Sample), (b: Batch) ";
            // Create (Node Name)
            statement += string.Format("WHERE id(a) = {0} and id(b) = {1} ", rel.fromId, rel.toID);
            statement += string.Format("CREATE (a) -[r:{0}]-> (b) ", rel.Name);

            return statement;
        }

        public void Dispose()
        {
            dbDriver?.Dispose();
        }
    }
}
