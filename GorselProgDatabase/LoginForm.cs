using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using GorselProgFramework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using GorselProgDatabase;

namespace ChatClient
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private SqlConnection connection = new SqlConnection("Data Source=DESKTOP-12NGJ7T;Initial Catalog=GorselProg;Integrated Security=True");


        private void button1_Click(object sender, EventArgs e)
        {
            AppMain.User = new UserClass();
            UserClass user = new UserClass()
            {
                Username = textBox1.Text.Trim(),
                Password = textBox2.Text.Trim(),
            };
            AppMain.User.Username = textBox1.Text;
            connection.Open();
            string loginquery = ("select password from [user] where username=@username");
            SqlCommand logincmd = new SqlCommand(loginquery, connection);
            logincmd.Connection = connection;
            logincmd.Parameters.AddWithValue("@username", user.Username);
            string doesPasswordExist = (string)logincmd.ExecuteScalar();
            connection.Close();
            //eğer boş döndüyse 
            if (doesPasswordExist == null)
            {
                MessageBox.Show("Boyle bir kullanici yok!");
                //giriş formunu tekrardan göster
                return;
            }
            //eğer kullanıcı varsa giriş formunu kapat
            this.Close();
        }
        //kayıt etme
        private void button2_Click(object sender, EventArgs e)
        {
            UserClass user = new UserClass()
            {
                Username = textBox1.Text.Trim(),
                Password = textBox2.Text.Trim(),
            };
            connection.Open();
            //kullanıcı zaten var mı yok mu diye baktırıyorum
            string checkifuserexistsquery = ("select count(*) username from [user] where username=@username");
            SqlCommand checkifuserexists = new SqlCommand(checkifuserexistsquery, connection);
            checkifuserexists.Connection = connection;
            checkifuserexists.Parameters.AddWithValue("@username", user.Username);
            int a = (int)checkifuserexists.ExecuteScalar();
            connection.Close();
            if (a > 0)
            {
                MessageBox.Show("Bu kullanici zaten kayitli!");
            }
            else if (a == 0 && textBox1.Text != null && textBox2.Text != null)
            {
                connection.Open();
                //kullanıcı veri tabanına kaydediliyor
                string kayit = "insert into [user] (username,password) values (@username,@password)";
                SqlCommand cmd = new SqlCommand(kayit, connection);
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@username", user.Username);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Bos Birakilamaz!");
            }

        }
    }
}
