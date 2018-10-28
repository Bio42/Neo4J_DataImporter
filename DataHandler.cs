using System.Collections.Generic;

namespace Neo4J_DataImporter
{
    public class DataHandler
    {
        public static List<Node> CalculateNodes(List<DataRow> data)
        {
            var nodes = new List<Node>();

            foreach (DataRow row in data)
            {
                // Create all the Bitches / Batches
                var batchNode = new Node("Batch");
                batchNode.Properties = new List<Property>() { new Property("BatchId", row.BatchId) };

                if (null == nodes.Find(x=>x.Name==batchNode.Name))
                {
                    nodes.Add(batchNode);
                }

                // Create "Sample" Nodes
                var sampleNode = new Node("Sample");
                // Assing Properties
                sampleNode.Properties = new List<Property>() {
                    new Property("SampleId", row.SampleId),
                    new Property("ProcessTime", row.ProcessTime),
                    new Property("AIRFL_Value", row.AIRFL_Value),
                    new Property("CO2_Value", row.CO2_Value),
                    new Property("JTEMP_Value", row.JTEMP_Value),
                    new Property("O2FL_Value", row.O2FL_Value),
                    new Property("ph_Value", row.ph_Value),
                    new Property("pO2_Setpoint", row.pO2_Setpoint),
                    new Property("PRESS_Value", row.PRESS_Value),
                    new Property("STIRR_value", row.STIRR_value),
                    new Property("SUBAT_Value", row.SUBAT_Value),
                    new Property("SUBSA_Value", row.SUBSA_Value),
                    new Property("TEMP_Value", row.TEMP_Value),
                    new Property("VWGHT_Value", row.VWGHT_Value),
                    new Property("EXHCO2_Value", row.EXHCO2_Value),
                    new Property("EXHO2_Value", row.EXHO2_Value),
                    new Property("Glc_Value", row.Glc_Value),
                    new Property("Lac_Value", row.Lac_Value),
                    new Property("CAP_Value", row.CAP_Value),
                    new Property("COND_Value", row.COND_Value),
                    new Property("VCD_CAP_Value", row.VCD_CAP_Value),
                    new Property("USA_CENTEC_Value", row.USA_CENTEC_Value),
                    new Property("USATT_CENTEC_Value", row.USATT_CENTEC_Value),
                    new Property("USV_CENTEC_Value", row.USV_CENTEC_Value),
                    new Property("USVGA_CENTEC_Value", row.USVGA_CENTEC_Value),
                    new Property("EXCO2_VARY_Value", row.EXCO2_VARY_Value),
                    new Property("EXO2_VARY_Value", row.EXO2_VARY_Value)
                    };
                nodes.Add(sampleNode);
            }

            return nodes;
        }

        public static List<Relationship> CalculateRelationships(List<Node> nodes)
        {
            var ships = new List<Relationship>();
            int currentBatchNodeId=-1;
            foreach (Node node in nodes)
            {
                if (node.Properties.Count == 1)
                {
                    // Batch
                    currentBatchNodeId = node.ID;
                    continue;
                }

                ships.Add(new Relationship() { fromId = node.ID, toID = currentBatchNodeId, Name = "FROM" });
            }

            return ships;
        }
    }

}
