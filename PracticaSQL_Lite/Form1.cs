using System.Data.SQLite;

namespace PracticaSQL_Lite
{
    public partial class Form1 : Form
    {
        //Agreagamos la base de datos
        string path = "data_table.db";
        string cs = @"URI=file" + Application.StartupPath + "\\data_table.db";
        //La base de datos se crae en la carpeta debug
        SQLiteConnection con;
        SQLiteCommand cmd;
        SQLiteDataReader dr;


        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Create_db();
            data_show();
        }
        private void data_show()
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            string stm = "SELECT * FROM test";
            var cmd = new SQLiteCommand(stm, con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dataGridView1.Rows.Insert(0, dr.GetString(0), dr.GetString(1));
            }
        }
        private void Create_db()
        {
            if (!System.IO.File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + path))
                {
                    sqlite.Open();
                    string sql = "create table test (name varchar(20), id varchar(20))";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("La base de datos no se puede crear");
                return;
            }
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);
            try
            {
                cmd.CommandText = "INSERT INTO test(name, id) VALUES(@name, @id)"; 
                string NAME = txtNombre.Text;
                string ID = txtID.Text;
                cmd.Parameters.AddWithValue("@name", NAME);
                cmd.Parameters.AddWithValue("@id", ID);

                dataGridView1.ColumnCount = 2;
                dataGridView1.Columns[0].Name = "Nombre";
                dataGridView1.Columns[1].Name = "ID";
                string[] row =   new string[] { NAME, ID };
                dataGridView1.Rows.Add(row);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo insertar la infromación: " + ex.Message);
                return;
            }
        }
    }
}
