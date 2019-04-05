using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordBreaker
{
    public partial class WordBreaker : Form
    {
        private Dictionary<string, Tuple<string, string>> allRoots;
        public WordBreaker(Dictionary<string, Tuple<string, string>> allRoots)
        {
            this.allRoots = allRoots;
            InitializeComponent();
        }

        private void search_Click(object sender, EventArgs e)
        {
            try
            {
                string search = submission.Text.Trim();
                Tuple<string, string> data = allRoots[search];
                result.Rows.Clear();
                result.Columns.Clear();
                result.Refresh();
                result.Columns.Add("Search", "Prefix/Suffix");
                result.Columns.Add("Meaning", "Meaning");
                result.Columns.Add("Examples", "Examples");
                result.Rows.Add(search, data.Item1, data.Item2);
                
            }
            catch (KeyNotFoundException ex)
            {
                result.Rows.Clear();
                result.Columns.Clear();
                result.Refresh();
                result.Columns.Add("NOT FOUND", "");
                result.Rows.Add("No Results Found");
            }
        }
    }
}
