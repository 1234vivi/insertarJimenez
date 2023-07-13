using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoNuevo
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=192.168.1.55;Initial Catalog=DB_PRACTICAS;Persist Security Info=True;User ID=sa;Password=123*abc*456");

        public string ConnectionString { get; private set; }

        public Form1()
        {

            InitializeComponent();
            ConnectionString = "Data Source=192.168.1.55;Initial Catalog=DB_PRACTICAS;Persist Security Info=True;User ID=sa;Password=123*abc*456";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CargarData();

        }
        private void CargarData()


        {
            SqlDataAdapter da = new SqlDataAdapter("select *from pr_transacciones ",conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            this.dataGridView1.DataSource = dt;
        }






        private void InsertarDatos_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    Random random = new Random();

                    for (int i = 1; i <= 100000; i++)
                    {
                         int id= random.Next();
                        DateTime fecha = DateTime.Now.AddDays(-random.Next(365));
                        int comercio = random.Next(1000);
                        string tarjeta = GenerateRandomTarjeta();
                        decimal valor = random.Next(100, 10000) / 100.0m;
                        string tipotrx = GenerateRandomTipoTrx(connection);
                        string pr_razon = GetRandomRazonId( connection);
                        string autoriza = GenerateRandomAutoriza();
                        string id_tipo = GenerateRandomTipoTrx(connection); 
                        string id_razones = GetRandomRazonId( connection);


                        // Insertar el registro en la base de datos
                        string query = "INSERT INTO pr_transacciones (pr_id, pr_fecha, pr_comercio, pr_tarjeta, pr_valor, pr_tipotrx, pr_razon, pr_autoriza, id_tipo, id_Razones) " +
                            "VALUES (@pr_id, @pr_fecha, @pr_comercio, @pr_tarjeta, @pr_valor, @pr_tipotrx, @pr_razon, @pr_autoriza, @id_tipo, @id_Razones)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@pr_id", id);
                            command.Parameters.AddWithValue("@pr_fecha", fecha);
                            command.Parameters.AddWithValue("@pr_comercio", comercio);
                            command.Parameters.AddWithValue("@pr_tarjeta", tarjeta);
                            command.Parameters.AddWithValue("@pr_valor", valor);
                            command.Parameters.AddWithValue("@pr_tipotrx", GenerateRandomTipoTrx(connection));
                            command.Parameters.AddWithValue("@pr_razon", pr_razon);
                            command.Parameters.AddWithValue("@pr_autoriza", autoriza);
                            command.Parameters.AddWithValue("@id_tipo", id_tipo);
                            command.Parameters.AddWithValue("@id_Razones", id_razones);
                            command.ExecuteNonQuery();
                            Console.WriteLine("holakease");
                        }
                    }

                    MessageBox.Show("Se insertaron los registros de manera aleatoria.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar los registros: " + ex.Message);
            }
        }
     
        string query = "(SELECT ISNULL((SELECT TOP 1 id_razon FROM TRtrx WHERE id_tipo = " + result + " ORDER BY NEWID()), 0))";

        SqlCommand command = new SqlCommand(query, connection);
            object result = command.ExecuteScalar();





        private string GetRandomRazonId(SqlConnection connection)
        {
            string query = "(SELECT ISNULL((SELECT TOP 1 id_razon FROM TRtrx WHERE id_tipo = "+GenerateRandomTipoTrx(connection)+ " ORDER BY NEWID()), 0))";
            Console.WriteLine(query);
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                return (string)command.ExecuteScalar();
            }
        }

        private string GenerateRandomTarjeta()
        {
            Random random = new Random();
            string prefix = "XXXXX";
            string suffix = random.Next(10000, 99999).ToString();
            return prefix + suffix;
        }
        private string GenerateRandomTipoTrx(SqlConnection connection)
        {
            string query = "(SELECT TOP 1 id from tipotrx order by NEWID()) ";
            using (SqlCommand command = new SqlCommand(query, connection))
            {

                return (string)command.ExecuteScalar();
            }
        }

        private string GenerateRandomAutoriza()
        {
            Random random = new Random();
            string[] autorizas = { "Aut1", "Aut2", "Aut3", "Aut4", "Aut5" };
            int index = random.Next(autorizas.Length);
            return autorizas[index];
        }
    }
}
