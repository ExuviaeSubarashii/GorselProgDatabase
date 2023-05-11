using ChatClient;
using GorselProgFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GorselProgDatabase
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SqlConnection connection = new SqlConnection("Data Source=DESKTOP-12NGJ7T;Initial Catalog=GorselProg;Integrated Security=True");
        private void Form1_Load(object sender, EventArgs e)
        {
            LoginForm LF = new LoginForm();
            LF.ShowDialog();
            if (AppMain.User != null)
            {

            }
            else
            {
                Application.Exit();
            }
            if (AppMain.User == null)
            {
                return;
            }
            label1.Text = AppMain.User.Username;
        }
        //mesajların gönderilmesi
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox1.Text != "" && channelLabel.Text != "label2")
            {
                Messages messages = new Messages()
                {
                    Message = textBox1.Text,
                    Username = AppMain.User.Username,
                    SendTime = DateTime.UtcNow,
                    Channel = channelLabel.Text
                };
                connection.Open();
                string sendmessage = "insert into [Messages] (username,message,sendtime,channel) values (@username,@message,@sendtime,@channel)";
                SqlCommand cmd = new SqlCommand(sendmessage, connection);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@username", AppMain.User.Username);
                cmd.Parameters.AddWithValue("@message", messages.Message);
                cmd.Parameters.AddWithValue("@sendtime", messages.SendTime);
                cmd.Parameters.AddWithValue("@channel", messages.Channel);
                cmd.ExecuteNonQuery();
                connection.Close();
                textBox1.Text = "";
            }
        }
        //kanal 1 için yenileme
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = "";
            channelLabel.Text = button1.Text;
            connection.Open();
            string cmdstr = "select * from Messages where channel=@channel";
            SqlCommand cmd = new SqlCommand(cmdstr, connection);
            cmd.Parameters.Add(new SqlParameter("@channel", button1.Text));
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }
        //kanal 2 için yenileme
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = "";
            channelLabel.Text = button2.Text;
            connection.Open();
            string cmdstr = "select * from Messages where channel=@channel";
            SqlCommand cmd = new SqlCommand(cmdstr, connection);
            cmd.Parameters.Add(new SqlParameter("@channel", button2.Text));
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }
        private int id;
        private string name;
        //verilerin silinmesi için satırın tamamı seçili olmalıdır.
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }
            if (name.Trim() != label1.Text.Trim())
            {
                MessageBox.Show("Bu Mesaj Size Ait Degil!");
                return;
            }
            connection.Open();
            string cmdstr = "delete from messages where ID=@ID";
            SqlCommand cmd = new SqlCommand(cmdstr, connection);
            cmd.Parameters.Add(new SqlParameter("@ID", id));
            cmd.ExecuteNonQuery();
            MessageBox.Show("Mesaj Kaldirildi");
            connection.Close();
        }
        int indexRow;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[indexRow];
            id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].FormattedValue.ToString());
            name = dataGridView1.Rows[e.RowIndex].Cells["Username"].FormattedValue.ToString();

            textBox1.Text = row.Cells[2].Value.ToString();
        }
        //mesajin duzenlenmesi
        private void button4_Click(object sender, EventArgs e)
        {
            DataGridViewRow newDataRow = dataGridView1.Rows[indexRow];
            newDataRow.Cells[0].Value.ToString();
            if (name.Trim() != label1.Text.Trim()&&textBox1.Text!=null)
            {
                MessageBox.Show("Bu Mesaj Size Ait Degil veya Mesaj Bos!");
                return;
            }
            connection.Open();
            string cmdstr = "update [messages] set Message=@message where ID=@ID";
            SqlCommand cmd = new SqlCommand(cmdstr,connection);
            cmd.Parameters.Add(new SqlParameter("@message", textBox1.Text));
            cmd.Parameters.Add(new SqlParameter("@ID", newDataRow.Cells[0].Value));
            cmd.ExecuteNonQuery();
            connection.Close();
            MessageBox.Show("Mesaj Duzenlendi");
        }
    }
}
