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
    public partial class Kassir : Form
    {
        int idVoyage;
        SqlConnection conn = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=aeroport;Integrated Security=True;");
        public Kassir()
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
        public void UpdateTable()
        {
            DateTime arrive, destination;
            TimeSpan dif;
            string line;
            int i = 0;
            int countPlace, countBusyPlace;
            dataGridView1.Rows.Clear();
            comboBox1.Items.Clear();
            SqlCommand cmd = new SqlCommand("Select *,Plane.name From Voyage join Plane on Plane.id = Voyage.planeNumber", conn); //создаем команду
            using (SqlDataReader dr = cmd.ExecuteReader()) //выполняем
            {
                while (dr.Read()) //читаем результат
                {
                    dataGridView1.Rows.Add();
                    line = dr.GetValue(0).ToString().Trim();
                    line += ". " + dr.GetValue(1).ToString().Trim();
                    line += ". " + dr.GetValue(2).ToString().Trim();
                    comboBox1.Items.Add(line);
                    dataGridView1[0, i].Value = dr.GetValue(0).ToString().Trim();
                    dataGridView1[1, i].Value = dr.GetValue(1).ToString().Trim();
                    dataGridView1[2, i].Value = dr.GetValue(2).ToString().Trim();
                    arrive = System.Convert.ToDateTime(dr.GetValue(3).ToString());
                    dataGridView1[3, i].Value = stringToDate(arrive);
                    dataGridView1[4, i].Value = stringToTime(arrive);
                    destination = System.Convert.ToDateTime(dr.GetValue(4).ToString());
                    dataGridView1[5, i].Value = stringToTime(destination);
                    dif = destination - arrive;
                    dataGridView1[6, i].Value = dif;
                    dataGridView1[7, i].Value = dr.GetValue(9).ToString().Trim();
                    dataGridView1[8, i].Value = dr.GetValue(5).ToString().Trim();
                    countPlace = System.Convert.ToInt32(dr.GetValue(11).ToString().Trim());
                    countBusyPlace = System.Convert.ToInt32(dr.GetValue(7).ToString().Trim());
                    dataGridView1[9, i].Value = countPlace - countBusyPlace;
                    i++;
                }
            }
        }
        private void Kassir_Load(object sender, EventArgs e)
        {       
            conn.Open();
            UpdateTable();
        }
        public int getId()
        {
            int id;
            SqlCommand cmd = new SqlCommand("Select max(id) From Ticket", conn); //создаем команду
            using (SqlDataReader dr = cmd.ExecuteReader()) //выполняем
            {
                dr.Read();
                id = System.Convert.ToInt32(dr.GetValue(0).ToString().Trim());
            }
            return id + 1;
        }
        public int CountBusyPlace()
        {
            int res;
            SqlCommand cmd = new SqlCommand("Select CountFreePlace From Voyage where idVoyage = "+(comboBox1.SelectedIndex+1).ToString(), conn); //создаем команду
            using (SqlDataReader dr = cmd.ExecuteReader()) //выполняем
            {
                dr.Read();
                res = System.Convert.ToInt32(dr.GetValue(0).ToString().Trim());
            }
            return res+1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int id = getId();
            SqlCommand cmd = new SqlCommand("Insert into Ticket (id,Surname,Name,MiddleName,VoyageNumber) Values (" + id.ToString() + ",'" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "',"+comboBox1.SelectedIndex+")", conn); //создаем команду
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("Update Voyage set CountFreePlace = " + CountBusyPlace().ToString() + " where idVoyage = " + (comboBox1.SelectedIndex + 1).ToString(), conn); //создаем команду
            cmd.ExecuteNonQuery();
            UpdateTable();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowTickets ticket = new ShowTickets();
            ticket.idVoyage = idVoyage;
            ticket.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idVoyage = System.Convert.ToInt32(dataGridView1[0, e.RowIndex].Value);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            idVoyage = System.Convert.ToInt32(dataGridView1[0, e.RowIndex].Value);
        }
    }
}
