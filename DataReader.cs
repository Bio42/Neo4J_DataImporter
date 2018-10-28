using System.Collections.Generic;
using System.IO;

namespace Neo4J_DataImporter
{
    public static class DataReader
    {
        public static List<DataRow> ReadCSV(string csvPath)
        {
            List<DataRow> rows = new List<DataRow>();
            using (var reader = new StreamReader(csvPath))
            {
                while (!reader.EndOfStream)
                {                   

                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    var row = new DataRow
                    {
                        BatchId = values[0],
                        SampleId = values[1],
                        ProcessTime = values[2],
                        AIRFL_Value = values[3],
                        CO2_Value = values[4],
                        JTEMP_Value = values[5],
                        O2FL_Value = values[6],
                        ph_Value = values[7],
                        pO2_Setpoint = values[8],
                        PRESS_Value = values[9],
                        STIRR_value = values[10],
                        SUBAT_Value = values[11],
                        SUBSA_Value = values[12],
                        TEMP_Value = values[13],
                        VWGHT_Value = values[14],
                        EXHCO2_Value = values[15],
                        EXHO2_Value = values[16],
                        Glc_Value = values[17],
                        Lac_Value = values[18],
                        CAP_Value = values[19],
                        COND_Value = values[20],
                        VCD_CAP_Value = values[21],
                        USA_CENTEC_Value = values[22],
                        USATT_CENTEC_Value = values[23],
                        USV_CENTEC_Value = values[24],
                        USVGA_CENTEC_Value = values[25],
                        EXCO2_VARY_Value = values[26],
                        EXO2_VARY_Value = values[27]
                    };
                    rows.Add(row);
                }
            }
            // Throw titles away
            rows.RemoveAt(0);

            return rows;
        }
    }

}
