using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TeleMessenger.DataProvider;
using TeleMessenger.Model;

namespace TeleMessenger
{
    public partial class Form2 : Form
    {
        private string[] tele_group_list = null;
        List<string> _categoryList = null;
        List<STT_GrName_GrLink_Model> table_data = null;
        List<STT_GrName_GrLink_Model> send_fail = null;

        public Form2()
        {
            InitializeComponent();
            this.CenterToScreen();
            Load_Category_List();
            Load_RightMouse_Click_Menu();
            LoadForm();
           
        }

        private void LoadForm() 
        {
            if(table_data != null) 
            {
                dataGridView1.DataSource = table_data;
                textBox2.Text = table_data.Count.ToString();
            }
            else 
            {
                dataGridView1.DataSource = new List<STT_GrName_GrLink_Model>();
                textBox2.Text = "0";
            }
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[1].Width = 180;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[2].Width = 150;
        }
        private void btnChangeName_Click(object sender, EventArgs e)
        {
            fSetting form = new fSetting(_categoryList);
            form.ShowDialog();
            table_data = null;
            Load_RightMouse_Click_Menu();
            Load_Category_List();
            LoadForm();
        }
        private void Load_Category_List() 
        {           
            comboBox1.Items.Clear();
            comboBox1.Text = "Chọn danh mục";
            _categoryList = new DataConnection().Get_Category_Name_List();
            for (int i = 0; i < _categoryList.Count; i++)
            {
                comboBox1.Items.Add(_categoryList[i].ToString());
            }
        }    
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {          
            table_data = new DataConnection().Get_Table_Data(comboBox1.Text);
            LoadForm();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string startupPath = Application.StartupPath;
            string profileName = "Telegram";
            if (!Directory.Exists("Profile"))
            {
                Directory.CreateDirectory("Profile");
            }
            if (Directory.Exists("Profile"))
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("user-data-dir=" + startupPath + "\\Profile");
                options.AddArgument("profile-directory=" + profileName);
                options.AddArgument("--start-maximized");
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                ChromeDriver driver = new ChromeDriver(service, options);

                try
                {
                    driver.Url = "https://web.telegram.org/";
                    driver.Navigate();
                    Thread.Sleep(2500);

                    StringWriter strWriteLine_02 = new StringWriter();
                    strWriteLine_02.WriteLine("-Lưu ý: ");
                    strWriteLine_02.WriteLine(" ");
                    strWriteLine_02.WriteLine("1. Trình duyệt sẽ auto tắt khi click 'Ok' hoặc tắt From");
                    strWriteLine_02.WriteLine(" ");
                    strWriteLine_02.WriteLine("2. Phải click 'OK' hoặc tắt From khi đã đăng nhập tele thành công");
                    MessageBox.Show(strWriteLine_02.ToString(), "Form hướng dẫn", MessageBoxButtons.OK,
                                            MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    driver.Quit();
                    driver.Dispose();
                }
                catch
                {
                    driver.Quit();
                    driver.Dispose();
                }
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text)) 
            {
                if (table_data != null)
                {
                    string startupPath = Application.StartupPath;
                    string profileName = "Telegram";
                    
                    string filePath = startupPath + "\\Message.txt";
                    File.WriteAllText(filePath, "");
                    File.WriteAllText(filePath, textBox1.Text);
                    string[] listStr = File.ReadAllLines(filePath);

                    if (Directory.Exists("Profile"))
                    {
                        ChromeOptions options = new ChromeOptions();
                        options.AddArgument("user-data-dir=" + startupPath + "\\Profile");
                        options.AddArgument("profile-directory=" + profileName);
                        //options.AddArgument("--start-maximized");
                        options.AddArgument("--window-position=-3200,-3200");
                        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                        service.HideCommandPromptWindow = true;
                        ChromeDriver driver = new ChromeDriver(service, options);
                        Thread.Sleep(100);
                        bool stop = false;
                        int error = 0;

                        try 
                        {
                            StringWriter stringWriter1 = new StringWriter();
                            string link = table_data[0].GrLink;
                            driver.Navigate().GoToUrl(link);
                            Thread.Sleep(200);
                            driver.Navigate().GoToUrl(link);
                            Thread.Sleep(300);
                            var textbox = driver.FindElement(By.Id("editable-message-text"));
                            Thread.Sleep(100);

                            for (int j = 0; j < listStr.Length; j++)
                            {
                                try
                                {
                                    textbox.SendKeys(listStr[j]);
                                    Thread.Sleep(50);
                                    textbox.SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
                                    Thread.Sleep(50);
                                    stringWriter1.WriteLine(listStr[j]);
                                }
                                catch
                                {
                                    error += 1;
                                    stop = true;
                                    stringWriter1.WriteLine("===>" + listStr[j]);
                                }
                            }

                            new Actions(driver).KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("a").KeyUp(OpenQA.Selenium.Keys.Control).Build().Perform();
                            Thread.Sleep(100);
                            textbox.SendKeys(OpenQA.Selenium.Keys.Backspace);
                            Thread.Sleep(600);

                            if (stop == true)
                            {
                                stringWriter1.WriteLine("");
                                stringWriter1.WriteLine("");
                                stringWriter1.WriteLine("Gợi ý Icon:");
                                stringWriter1.WriteLine("▶️  ➡️  ✳️  ✅  ➖  ☎️  ❤️  ❌  ⚠️ ♏️  Ⓜ️  ⚽️  ☘️  ❄️ ✈️ ⚡️ ✨");
                                textBox1.Text = stringWriter1.ToString();

                                dataGridView1.Rows[0].Cells[2].Value = "Failed";

                                MessageBox.Show("Lỗi text " + error.ToString() + " dòng, cần thay Icon khác", "Thông báo", MessageBoxButtons.OK,
                                                MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);

                            }
                        }
                        catch { }


                        if(stop == false) 
                        {
                            try
                            {
                                send_fail = new List<STT_GrName_GrLink_Model>();
                                int f = 1;
                                for (int i = 0; i < table_data.Count; i++)
                                {
                                    try
                                    {
                                        string link = table_data[i].GrLink;
                                        driver.Navigate().GoToUrl(link);
                                        Thread.Sleep(200);
                                        driver.Navigate().GoToUrl(link);
                                        Thread.Sleep(300);
                                        var textbox = driver.FindElement(By.Id("editable-message-text"));
                                        Thread.Sleep(100);

                                        for (int j = 0; j < listStr.Length; j++)
                                        {

                                            textbox.SendKeys(listStr[j]);
                                            Thread.Sleep(50);
                                            textbox.SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
                                            Thread.Sleep(50);
                                        }

                                        Thread.Sleep(100);
                                        var sendBTN = driver.FindElement(By.XPath("//div[@id='MiddleColumn']//div[@class='messages-layout']//button[@title='Send Message']"));
                                        Thread.Sleep(400);
                                        sendBTN.Click();
                                        Thread.Sleep(600);
                                        dataGridView1.Rows[i].Cells[2].Value = "Successful";
                                    }
                                    catch
                                    {
                                        dataGridView1.Rows[i].Cells[2].Value = "Failed";
                                        STT_GrName_GrLink_Model fail = new STT_GrName_GrLink_Model();
                                        fail.STT = f;
                                        fail.GrName = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                        fail.SessionResult = "Failed";
                                        fail.GrLink = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                        send_fail.Add(fail);
                                        f = f + 1;
                                    }
                                }

                                driver.Quit();
                                driver.Dispose();
                            }
                            catch
                            {
                                driver.Quit();
                                driver.Dispose();
                            }
                        }
                    }
                    
                }
                else
                {
                    MessageBox.Show("Table chưa có data");
                }
            }
            else
            {
                MessageBox.Show("Chưa nhập tin nhắn", "Thông báo");
            }
        }

        //Nhập file text
        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            List<STT_GrName_GrLink_Model> table_data_list = new List<STT_GrName_GrLink_Model>();
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Browse Text File",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tele_group_list = File.ReadAllLines(openFileDialog.FileName);
                for (int i = 0; i < tele_group_list.Length; i++)
                {
                    STT_GrName_GrLink_Model table_data = new STT_GrName_GrLink_Model();
                    string[] group_data = tele_group_list[i].Split('|');
                    if (group_data.Length == 2)
                    {
                        table_data.STT = i + 1;
                        table_data.GrName = tele_group_list[i].Split('|')[0].ToString();
                        table_data.GrLink = tele_group_list[i].Split('|')[1].ToString();
                        table_data_list.Add(table_data);
                    }
                    else
                    {
                        table_data.STT = i + 1;
                        table_data.GrName = tele_group_list[i].ToString();
                        table_data.GrLink = tele_group_list[i].ToString();
                        table_data.SessionResult = "Sai Format";
                        table_data_list.Add(table_data);
                    }
                }
            }

            table_data = table_data_list;
            dataGridView1.DataSource = table_data;
            comboBox1.Text = "File text";
        }

        //Check tin nhắn
        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string link = new DataConnection().Get_Test_Group_Link();
                if (!String.IsNullOrEmpty(link))
                {
                    string startupPath = Application.StartupPath;
                    string profileName = "Telegram";
                    string filePath = startupPath + "\\Message.txt";

                    File.WriteAllText(filePath, "");
                    File.WriteAllText(filePath, textBox1.Text);
                    string[] listStr = File.ReadAllLines(filePath);
                    if (Directory.Exists("Profile"))
                    {
                        ChromeOptions options = new ChromeOptions();
                        options.AddArgument("user-data-dir=" + startupPath + "\\Profile");
                        options.AddArgument("profile-directory=" + profileName);
                        //options.AddArgument("--start-maximized");
                        options.AddArgument("--window-position=-3200,-3200");
                        ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                        service.HideCommandPromptWindow = true;
                        ChromeDriver driver = new ChromeDriver(service, options);
                        Thread.Sleep(100);
                        int error = 0;
                        StringWriter stringWriter1 = new StringWriter();
                        try
                        {
                            driver.Navigate().GoToUrl(link);
                            Thread.Sleep(400);
                            var textbox = driver.FindElement(By.Id("editable-message-text"));
                            Thread.Sleep(100);

                            for (int j = 0; j < listStr.Length; j++)
                            {
                                try
                                {
                                    textbox.SendKeys(listStr[j]);
                                    Thread.Sleep(50);
                                    textbox.SendKeys(OpenQA.Selenium.Keys.Shift + OpenQA.Selenium.Keys.Enter);
                                    Thread.Sleep(50);
                                    stringWriter1.WriteLine(listStr[j]);
                                }
                                catch
                                {
                                    error += 1;
                                    stringWriter1.WriteLine("===>" + listStr[j]);
                                }
                            }

                            new Actions(driver).KeyDown(OpenQA.Selenium.Keys.Control).SendKeys("a").KeyUp(OpenQA.Selenium.Keys.Control).Build().Perform();
                            Thread.Sleep(100);
                            textbox.SendKeys(OpenQA.Selenium.Keys.Backspace);



                            driver.Quit();
                            driver.Dispose();
                            this.WindowState = FormWindowState.Normal;
                        }
                        catch
                        {
                            driver.Quit();
                            driver.Dispose();
                            this.WindowState = FormWindowState.Normal;
                        }
                        if (error == 0)
                        {
                            textBox1.Text = stringWriter1.ToString();
                            MessageBox.Show("Good!", "Kết quả");
                        }
                        else
                        {
                            stringWriter1.WriteLine("");
                            stringWriter1.WriteLine("");
                            stringWriter1.WriteLine("Gợi ý Icon:");
                            stringWriter1.WriteLine("▶️  ➡️  ✳️  ✅  ➖  ☎️  ❤️  ❌  ⚠️ ♏️  Ⓜ️  ⚽️  ☘️  ❄️ ✈️ ⚡️ ✨");
                            textBox1.Text = stringWriter1.ToString();

                            MessageBox.Show("Lỗi text " + error.ToString() + " dòng, cần thay Icon khác", "Kết quả");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Profile chưa được tạo hoặc bị xóa!");
                    }
                }
                else
                {
                    MessageBox.Show("Chưa nhập link group test!");
                }
            }
            else
            {
                MessageBox.Show("Chưa nhập tin nhắn");
            }
        }

        // Chuột phải
        private void Load_RightMouse_Click_Menu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripItem past_test_gr_link = menu.Items.Add("Dán link group tele test Icon");
            past_test_gr_link.Click += new EventHandler(Past_Test_Group_Link);
            ToolStripItem copy_test_gr_link = menu.Items.Add("Copy link group tele test Icon");
            copy_test_gr_link.Click += new EventHandler(Copy_Test_Group_Link);

            _categoryList = new DataConnection().Get_Category_Name_List();
            ToolStripMenuItem save_to_category = (ToolStripMenuItem)menu.Items.Add("Lưu file text vào danh mục");
            if (_categoryList != null)
            {
                ToolStripItem save_to_category_01 = save_to_category.DropDownItems.Add(_categoryList[0]);
                save_to_category_01.Click += new EventHandler(Update_Table_01);
                ToolStripItem save_to_category_02 = save_to_category.DropDownItems.Add(_categoryList[1]);
                save_to_category_02.Click += new EventHandler(Update_Table_02);
                ToolStripItem save_to_category_03 = save_to_category.DropDownItems.Add(_categoryList[2]);
                save_to_category_03.Click += new EventHandler(Update_Table_03);
                ToolStripItem save_to_category_04 = save_to_category.DropDownItems.Add(_categoryList[3]);
                save_to_category_04.Click += new EventHandler(Update_Table_04);
                ToolStripItem save_to_category_05 = save_to_category.DropDownItems.Add(_categoryList[4]);
                save_to_category_05.Click += new EventHandler(Update_Table_05);
                ToolStripItem save_to_category_06 = save_to_category.DropDownItems.Add(_categoryList[5]);
                save_to_category_06.Click += new EventHandler(Update_Table_06);
                ToolStripItem save_to_category_07 = save_to_category.DropDownItems.Add(_categoryList[6]);
                save_to_category_07.Click += new EventHandler(Update_Table_07);
                ToolStripItem save_to_category_08 = save_to_category.DropDownItems.Add(_categoryList[7]);
                save_to_category_08.Click += new EventHandler(Update_Table_08);
                ToolStripItem save_to_category_09 = save_to_category.DropDownItems.Add(_categoryList[8]);
                save_to_category_09.Click += new EventHandler(Update_Table_09);
                ToolStripItem save_to_category_10 = save_to_category.DropDownItems.Add(_categoryList[9]);
                save_to_category_10.Click += new EventHandler(Update_Table_10);
                ToolStripItem save_to_category_11 = save_to_category.DropDownItems.Add(_categoryList[10]);
                save_to_category_11.Click += new EventHandler(Update_Table_11);
                ToolStripItem save_to_category_12 = save_to_category.DropDownItems.Add(_categoryList[11]);
                save_to_category_12.Click += new EventHandler(Update_Table_12);
                ToolStripItem save_to_category_13 = save_to_category.DropDownItems.Add(_categoryList[12]);
                save_to_category_13.Click += new EventHandler(Update_Table_13);
                ToolStripItem save_to_category_14 = save_to_category.DropDownItems.Add(_categoryList[13]);
                save_to_category_14.Click += new EventHandler(Update_Table_14);
                ToolStripItem save_to_category_15 = save_to_category.DropDownItems.Add(_categoryList[14]);
                save_to_category_15.Click += new EventHandler(Update_Table_15);
                ToolStripItem save_to_category_16 = save_to_category.DropDownItems.Add(_categoryList[15]);
                save_to_category_16.Click += new EventHandler(Update_Table_16);
                ToolStripItem save_to_category_17 = save_to_category.DropDownItems.Add(_categoryList[16]);
                save_to_category_17.Click += new EventHandler(Update_Table_17);
                ToolStripItem save_to_category_18 = save_to_category.DropDownItems.Add(_categoryList[17]);
                save_to_category_18.Click += new EventHandler(Update_Table_18);
                ToolStripItem save_to_category_19 = save_to_category.DropDownItems.Add(_categoryList[18]);
                save_to_category_19.Click += new EventHandler(Update_Table_19);
                ToolStripItem save_to_category_20 = save_to_category.DropDownItems.Add(_categoryList[19]);
                save_to_category_20.Click += new EventHandler(Update_Table_20);
            }

            ToolStripItem clear_table_data = menu.Items.Add("Xóa dữ liệu nhóm hiện tại");
            clear_table_data.Click += new EventHandler(Clear_Table_Data);

            dataGridView1.ContextMenuStrip = menu;
        }
        private void Clear_Table_Data(object sender, EventArgs e)
        {
            if (comboBox1.Text != "File text" && comboBox1.Text != "Chọn danh mục")
            {
                string message = "Bạn muốn xóa dữ liệu Nhóm: " + $"'" + comboBox1.Text + $"'";
                DialogResult dialogResult = MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    new DataConnection().Clear_Table_Data(comboBox1.Text);
                    table_data = null;
                    LoadForm();
                }
            }
        }

        private void Update_Table_01(object sender, EventArgs e)
        {
            if(comboBox1.Text == "File text") 
            {
                new DataConnection().Update_Data_Table(table_data, "1");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }  
        }
        private void Update_Table_02(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "2");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_03(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "3");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }

        }
        private void Update_Table_04(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "4");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }

        }
        private void Update_Table_05(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "5");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_06(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "6");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_07(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "7");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_08(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "8");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_09(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "9");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_10(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "10");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_11(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "11");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_12(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "12");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_13(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "13");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_14(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "14");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_15(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "15");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_16(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "16");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_17(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "17");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_18(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "18");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_19(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "19");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }
        private void Update_Table_20(object sender, EventArgs e)
        {
            if (comboBox1.Text == "File text")
            {
                new DataConnection().Update_Data_Table(table_data, "20");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập data từ file .txt");
            }
        }

        private void Past_Test_Group_Link(object sender, EventArgs e) 
        {
            IDataObject iData = Clipboard.GetDataObject();
            var copy_link = (String)iData.GetData(DataFormats.Text);
            new DataConnection().Past_Test_Group_Link(copy_link);
        }
        private void Copy_Test_Group_Link(object sender, EventArgs e)
        {
            IDataObject iData = Clipboard.GetDataObject();
            var copy_link = (String)iData.GetData(DataFormats.Text);
            var link = new DataConnection().Get_Test_Group_Link();

            if (!string.IsNullOrEmpty(link))
            {
                Clipboard.SetText(link);
            }
            else
            {
                Clipboard.SetText("Chưa dán link");
            }
        }

    }
}