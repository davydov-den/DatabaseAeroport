using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Aeroport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login, test_login;
            string pass, test_password;
            bool check = false;
            string mode;
            login = textBox1.Text;
            pass = textBox2.Text;
            SqlConnection conn = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=aeroport;Integrated Security=True;");
            try
            {
                //пробуем подключится
                conn.Open();
                SqlCommand cmd = new SqlCommand("Select * From users", conn); //создаем команду
                using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) //выполняем
                {
                    while (dr.Read()) //читаем результат
                    {
                        string id = dr.GetValue(0).ToString().Trim();
                        test_login = dr.GetValue(1).ToString().Trim();
                        test_password = dr.GetValue(2).ToString().Trim();
                        if (login == test_login) //если логины совпадают
                        {
                            check = true;
                            if (test_password == pass) //если пароли совпадают
                            {
                                mode = dr.GetValue(3).ToString();
                                if (mode == "1") //проверка наличия прав администратора
                                {
                                    Kassir kassir = new Kassir();
                                    kassir.ShowDialog();
                                }
                                else
                                {
                                    Admin admin = new Admin();
                                    admin.ShowDialog();
                                }
                                Hide();
                            }
                            else
                            {
                                MessageBox.Show("Пароль не верный!!!");
                            }
                        }
                    }

                }
                conn.Close();
                conn.Dispose();
            }
            catch (SqlException se)
            {
                MessageBox.Show(se.Message);
            }
            if (!check)
                MessageBox.Show("Пользователь не найден!");
            //закрвываем соединение


            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowVoyages show = new ShowVoyages();
            show.ShowDialog();
        }
    }
}
