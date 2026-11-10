
using System;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace PracticaSQL_Lite
{
    public partial class Form1 : Form
    {
        // Ruta de la base de datos
        string path = Path.Combine(Application.StartupPath, "data_table.db");
        string cs = "Data Source=" + Path.Combine(
            Application.StartupPath, "data_table.db"
        ) + ";Version=3;";
        public Form1()
        {
            InitializeComponent();

            cs = "Data Source=" + path + ";Version=3;";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Create_db();
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Nombre";
            dataGridView1.Columns[1].Name = "ID";

            data_show();
        }
        // CREAR BASE DE DATOS
        private void Create_db()
        {
            try
            {
                using (SQLiteConnection con = new SQLiteConnection(cs))
                {
                    con.Open();
                    string sql = @"
                CREATE TABLE IF NOT EXISTS test (
                    name TEXT,
                    id TEXT PRIMARY KEY
                )";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear la tabla: " + ex.Message);
            }
        }
        // MOSTRAR DATOS
        private void data_show()
        {
            dataGridView1.Rows.Clear();

            using (SQLiteConnection con = new SQLiteConnection(cs))
            {
                con.Open();

                string stm = "SELECT name, id FROM test";

                using (SQLiteCommand cmd = new SQLiteCommand(stm, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(
                            dr["name"].ToString(),
                            dr["id"].ToString()
                        );
                    }
                }
            }
        }
        // INSERTAR
        private void btnInsertar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "" || txtID.Text.Trim() == "")
            {
                MessageBox.Show("Ingresa el nombre y el ID");
                return;
            }

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(cs))
                {
                    con.Open();

                    string sql = "INSERT INTO test(name, id) VALUES(@name, @id)";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@name", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Registro insertado correctamente");

                data_show();

                txtNombre.Clear();
                txtID.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo insertar la información: " + ex.Message);
            }
        }
        // ACTUALIZAR
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "" || txtID.Text.Trim() == "")
            {
                MessageBox.Show("Ingresa el nombre y el ID");
                return;
            }

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(cs))
                {
                    con.Open();

                    string sql = "UPDATE test SET name=@name WHERE id=@id";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@name", txtNombre.Text.Trim());
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim());

                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0)
                            MessageBox.Show("Registro actualizado correctamente");
                        else
                            MessageBox.Show("No se encontró el ID indicado");
                    }
                }

                data_show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actualizar la información: " + ex.Message);
            }
        }

        // ELIMINAR
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Trim() == "")
            {
                MessageBox.Show("Ingresa el ID que deseas eliminar");
                return;
            }

            try
            {
                using (SQLiteConnection con = new SQLiteConnection(cs))
                {
                    con.Open();

                    string sql = "DELETE FROM test WHERE id=@id";

                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@id", txtID.Text.Trim());

                        int filas = cmd.ExecuteNonQuery();

                        if (filas > 0)
                            MessageBox.Show("Registro eliminado correctamente");
                        else
                            MessageBox.Show("No se encontró el ID indicado");
                    }
                }
                data_show();
                txtNombre.Clear();
                txtID.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar la información: " + ex.Message);
            }
        }
    }
}
