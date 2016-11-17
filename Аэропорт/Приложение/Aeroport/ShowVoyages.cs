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
    public partial class ShowVoyages : Form
    {
        public ShowVoyages()
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

        private void ShowVoyages_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=aeroport;Integrated Security=True;");
            conn.Open();
            DateTime arrive, destination;
            TimeSpan dif;
            int i = 0;
            int countPlace, countBusyPlace;
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
                    countPlace = System.Convert.ToInt32(dr.GetValue(11).ToString().Trim());
                    countBusyPlace = System.Convert.ToInt32(dr.GetValue(7).ToString().Trim());
                    dataGridView1[8, i].Value = countPlace - countBusyPlace;
                    i++;
                }
            }
        }
    }
}
