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
    public class DataConnection
    {
        string startupPath = Application.StartupPath;

        // Data table
        public List<STT_GrName_GrLink_Model> Get_Table_Data(string categrory)
        {
            List<STT_GrName_GrLink_Model> table_data_list = new List<STT_GrName_GrLink_Model>();
            if (categrory != "Chọn danh mục")
            {
                DataTable data = new DataTable();
                DataTable data_01 = new DataTable();

                SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\TeleDatabase.db");
                connection.Open();
                //get name of table
                string query = $"SELECT ID FROM Table_Categories WHERE LinkGroup = '{categrory}'";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(data);
                string number = data.Rows[0][0].ToString();
                if (int.Parse(number) < 10)
                {
                    number = "0" + number;
                }

                //get all data from table
                string tableName = "Table_" + number;
                string query_01 = $"SELECT * FROM {tableName}";
                SQLiteCommand cmd_01 = new SQLiteCommand(query_01, connection);
                SQLiteDataAdapter adapter_01 = new SQLiteDataAdapter(cmd_01);
                adapter_01.Fill(data_01);

                connection.Close();

                for (int i = 0; i < data_01.Rows.Count; i++)
                {
                    STT_GrName_GrLink_Model table_data = new STT_GrName_GrLink_Model();
                    table_data.STT = i + 1;
                    table_data.GrName = data_01.Rows[i][0].ToString();
                    table_data.GrLink = data_01.Rows[i][1].ToString();
                    table_data_list.Add(table_data);
                }
            }

            return table_data_list;
        }
        public List<string> Get_Category_Name_List()
        {
            List<string> Category_List = new List<string>();
            
            SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\TeleDatabase.db");
            connection.Open();
            for(int i = 1; i <= 20; i++) 
            {
                DataTable data = new DataTable();
                string query = $"SELECT LinkGroup FROM Table_Categories WHERE ID = {i}";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(data);
                Category_List.Add(data.Rows[0][0].ToString());
            }
            connection.Close();
            return Category_List;
        }
        public void Clear_Table_Data(string categrory)
        {
            if (categrory != "Chọn danh mục")
            {
                DataTable data = new DataTable();

                SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\TeleDatabase.db");
                connection.Open();
                //get name of table
                string query = $"SELECT ID FROM Table_Categories WHERE LinkGroup = '{categrory}'";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(data);
                string number = data.Rows[0][0].ToString();
                if (int.Parse(number) < 10)
                {
                    number = "0" + number;
                }

                //Clear all data from table
                string tableName = "Table_" + number;
                string query_01 = $"DELETE FROM {tableName}";
                SQLiteCommand cmd_01 = new SQLiteCommand(query_01, connection);
                cmd_01.ExecuteNonQuery();

                connection.Close();
            }
        }
        public void Update_Data_Table(List<STT_GrName_GrLink_Model> listGroup , string i)
        {
            List<STT_GrName_GrLink_Model> result = new List<STT_GrName_GrLink_Model>();
            if(int.Parse(i) < 10) { i = "0" + i; }
            string table_name = "Table_" + i;
            SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\TeleDatabase.db");
            connection.Open();
            string query = $"DELETE FROM {table_name}";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery();
            foreach(var group in listGroup) 
            {
                string query_01 = $"INSERT INTO {table_name} (Name, LinkGroup)  VALUES('{group.GrName}', '{group.GrLink}')";
                SQLiteCommand cmd_01 = new SQLiteCommand(query_01, connection);
                cmd_01.ExecuteNonQuery();
            }
            connection.Close();
            MessageBox.Show("Done!", "Thông báo");
        }
        public void Update_Category_Name(List<string> categoryList)
        {
            SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\TeleDatabase.db");
            connection.Open();
            for (int i = 1; i <= categoryList.Count; i++)
            {
                string query = $"UPDATE Table_Categories SET LinkGroup = '{categoryList[i - 1]}' WHERE ID = {i}";
                SQLiteCommand cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            connection.Close();
        }

        //Group test
        public string Get_Test_Group_Link()
        {
            DataTable data = new DataTable();
            SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\TeleDatabase.db");
            connection.Open();
            string query = "SELECT GrTest FROM Table_GroupTest WHERE ID = 1";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery();
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
            adapter.Fill(data);
            connection.Close();
            return data.Rows[0][0].ToString();
        }
        public void Past_Test_Group_Link(string link)
        {
            SQLiteConnection connection = new SQLiteConnection("data source=" + startupPath + "\\TeleDatabase.db");
            connection.Open();
            string query = $"UPDATE Table_GroupTest SET GrTest = '{link}' WHERE ID = 1";
            SQLiteCommand cmd = new SQLiteCommand(query, connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("DONE!", "Kết quả");
        }
    }
}
