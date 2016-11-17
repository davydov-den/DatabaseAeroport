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
    public partial class ShowTickets : Form
    {
        public int idVoyage;
        SqlConnection conn = new SqlConnection(@"Data Source=(local)\SQLEXPRESS;Initial Catalog=aeroport;Integrated Security=True;");
        public ShowTickets()
        {
            InitializeComponent();
        }

        private void ShowTickets_Load(object sender, EventArgs e)
        {
            conn.Open();
            int i = 0;
            dataGridView1.Rows.Clear();
            SqlCommand cmd = new SqlCommand("Select * From Ticket where VoyageNumber = "+idVoyage, conn); //создаем команду
            using (SqlDataReader dr = cmd.ExecuteReader()) //выполняем
            {
                while (dr.Read()) //читаем результат
                {
                    dataGridView1.Rows.Add();
                    dataGridView1[0, i].Value = dr.GetValue(1).ToString().Trim();
                    dataGridView1[1, i].Value = dr.GetValue(2).ToString().Trim();               
                    dataGridView1[2, i].Value = dr.GetValue(3).ToString().Trim();
                    i++;
                }
            }

        }
    }
}
