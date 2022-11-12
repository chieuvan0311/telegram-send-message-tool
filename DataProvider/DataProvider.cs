using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleMessenger.Model;

namespace TeleMessenger.DataProvider
{
    public class DataProvider
    {
        string startupPath = Application.StartupPath;
        public List<Table_Model> GetAllSheetName () 
        {
            List<Table_Model> table_data_list = new List<Table_Model>();
            DataTable data = new DataTable();

            SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\Database.db");
            connection.Open();
            string query = "SELECT * FROM GoogleSheet_Name";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(data);
            connection.Close();
            for(int i = 0; i < data.Rows.Count; i++) 
            {
                Table_Model table_data = new Table_Model();
                table_data.STT = i + 1;
                table_data.Sheet_name = data.Rows[i][0].ToString();
            }


            return table_data_list;
        }

        public void AddSheetName(string sheet_name)
        {
            SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\Database.db");
            connection.Open();
            string query = $"INSERT INTO GoogleSheet_Name (SheetName)  VALUES (sheet_name)";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
