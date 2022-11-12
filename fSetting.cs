using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleMessenger.DataProvider;

namespace TeleMessenger
{
    public partial class fSetting : Form
    {
        public fSetting(List<string> category_list)
        {
            InitializeComponent();
            textBox1.Text = category_list[0];
            textBox2.Text = category_list[1];
            textBox3.Text = category_list[2];
            textBox4.Text = category_list[3];
            textBox5.Text = category_list[4];
            textBox6.Text = category_list[5];
            textBox7.Text = category_list[6];
            textBox8.Text = category_list[7];
            textBox9.Text = category_list[8];
            textBox10.Text = category_list[9];
            textBox11.Text = category_list[10];
            textBox12.Text = category_list[11];
            textBox13.Text = category_list[12];
            textBox14.Text = category_list[13];
            textBox15.Text = category_list[14];
            textBox16.Text = category_list[15];
            textBox17.Text = category_list[16];
            textBox18.Text = category_list[17];
            textBox19.Text = category_list[18];
            textBox20.Text = category_list[19];
        }

        private void Save_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            list.Add(textBox1.Text);
            list.Add(textBox2.Text);
            list.Add(textBox3.Text);
            list.Add(textBox4.Text);
            list.Add(textBox5.Text);
            list.Add(textBox6.Text);
            list.Add(textBox7.Text);
            list.Add(textBox8.Text);
            list.Add(textBox9.Text);
            list.Add(textBox10.Text);
            list.Add(textBox11.Text);
            list.Add(textBox12.Text);
            list.Add(textBox13.Text);
            list.Add(textBox14.Text);
            list.Add(textBox15.Text);
            list.Add(textBox16.Text);
            list.Add(textBox17.Text);
            list.Add(textBox18.Text);
            list.Add(textBox19.Text);
            list.Add(textBox20.Text);
            new DataConnection().Update_Category_Name(list);
            this.Close();
        }
    }
}
