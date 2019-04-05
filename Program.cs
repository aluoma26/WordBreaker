using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordBreaker
{
    class Program : Form
    {
        static void Main(string[] args)
        {
            Dictionary<string, Tuple<string, string>> allRoots = new Dictionary<string, Tuple<string, string>>();
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory + "\\PrefixSuffix.xlsx";
            string excl_connection_string = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + @";Extended Properties=""Excel 12.0 Xml;HDR=YES""";

            string sql = "SELECT * FROM [Sheet1$]";

            OleDbConnection con = new OleDbConnection(excl_connection_string);
            OleDbCommand cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string root = reader.GetString(0);
                    if (root.Contains(","))
                    {
                        Tuple<string, string> details = new Tuple<string, string>(reader.GetString(1), reader.GetString(2));

                        string[] values = root.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();

                            try
                            {
                                allRoots.Add(values[i], details);
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message == "An item with the same key has already been added.")
                                {
                                    Tuple<string, string> v = allRoots[values[i]];
                                    allRoots[values[i]] = new Tuple<string, string>(v.Item1 + " Alternate Meaning: "+ details.Item1, v.Item2 + ", " + details.Item2);
                                }

                            }
                        }
                    }
                    else
                    {
                        Tuple<string, string> details = new Tuple<string, string>(reader.GetString(1), reader.GetString(2));
                        try
                        {
                            allRoots.Add(reader.GetString(0), details);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message == "An item with the same key has already been added.")
                            {
                                Tuple<string, string> v = allRoots[reader.GetString(0)];
                                allRoots[reader.GetString(0)] = new Tuple<string, string>(v.Item1 + " Alternate Meaning: " + details.Item1, v.Item2 + ", " + details.Item2);
                            }

                        }
                    }
                }


                WordBreaker gui = new WordBreaker(allRoots);
                System.Windows.Forms.Application.Run(gui);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Dispose();
            }
        }
    }
}
