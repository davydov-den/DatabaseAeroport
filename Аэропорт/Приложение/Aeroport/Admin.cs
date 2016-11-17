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
using System.Collections;
namespace Aeroport
{
    public partial class Admin : Form
    {
        List<int> id = new List<int>();
        int selectIndex;
        SqlConnection conn = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=aeroport;Integrated Security=True;");
        public Admin()
        {
            InitializeComponent();
        }

        public string stringToDate(DateTime str)
        {
            string res = "";
            res += str.Year + "-" + str.Month.ToString("00") + "-" + str.Day.ToString("00");
            return res;
        }

        public string stringToTime(DateTime str)
        {
            string res = "";
            res += str.Hour + ":" + str.Minute.ToString("00");
            return res;
        }

        public string DateTimeToString(DateTime date, DateTime time)
        {
            string res = "";
            res = date.Year + "-" + date.Month + "-" + date.Day + " " + time.Hour + ":" + time.Minute;
            return res;
        }

        public int getId()
        {
            int id;
            SqlCommand cmd = new SqlCommand("Select max(idVoyage) From Voyage", conn); //создаем команду
            using (SqlDataReader dr = cmd.ExecuteReader()) //выполняем
            {
                dr.Read();
                id = System.Convert.ToInt32(dr.GetValue(0).ToString().Trim());
            }
            return id+1;
        }

        public void UpdateTable()
        {
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            DateTime arrive, destination;
            TimeSpan dif;
            int i = 0;
            dataGridView1.Rows.Clear();
            SqlCommand cmd = new SqlCommand("Select *,Plane.name From Voyage join Plane on Plane.id = Voyage.planeNumber", conn); //создаем команду
            using (SqlDataReader dr = cmd.ExecuteReader()) //выполняем
            {
                while (dr.Read()) //читаем результат
                {
                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = dr.GetValue(1).ToString().Trim();
                    dataGridView1[1, i].Value = dr.GetValue(2).ToString().Trim();
                    arrive = System.Convert.ToDateTime(dr.GetValue(3).ToString());
                    dataGridView1[2, i].Value = stringToDate(arrive);
                    dataGridView1[3, i].Value = stringToTime(arrive);
                    destination = System.Convert.ToDateTime(dr.GetValue(4).ToString());
                    dataGridView1[4, i].Value = stringToTime(destination);
                    dif = destination - arrive;
                    dataGridView1[5, i].Value = dif;
                    dataGridView1[6, i].Value = dr.GetValue(9).ToString().Trim();
                    dataGridView1[7, i].Value = dr.GetValue(5).ToString().Trim();
                    i++;
                }
            }

        }

        private void Admin_Load(object sender, EventArgs e)
        {    
            conn.Open();
            UpdateTable();
            SqlCommand cmd = new SqlCommand("Select * From Plane", conn);
            using (SqlDataReader dr = cmd.ExecuteReader()) //выполняем
            {
                while (dr.Read()) //читаем результат
                {
                    string line;
                    line = dr.GetValue(0).ToString().Trim();
                    line += ". " + dr.GetValue(1).ToString().Trim();
                    line += ". " + dr.GetValue(2).ToString().Trim();
                    comboBox1.Items.Add(line);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectIndex = e.RowIndex;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string arrive = DateTimeToString(dateTimePicker1.Value, dateTimePicker2.Value);
            string destination = DateTimeToString(dateTimePicker1.Value, dateTimePicker3.Value);
            int id = getId();
            SqlCommand cmd = new SqlCommand("Insert into Voyage (idVoyage,cityArrive,cityDestination,timeArrive,timeDestination,Cost,planeNumber,CountFreePlace) Values ("+id.ToString()+",'" + textBox1.Text + "','" + textBox2.Text + "','" + arrive + "','" + destination + "'," + textBox3.Text + "," + comboBox1.SelectedIndex + ",0)", conn); //создаем команду
            cmd.ExecuteNonQuery();
            UpdateTable();
        }
    }
}
