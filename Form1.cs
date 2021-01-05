using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tsqliteMem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // to keep database alive.
        SqliteConnection g_con = null;
        private void button1_Click(object sender, EventArgs e)
        {
            var connectionString = "Data Source=sharedmemdb;Mode=Memory;Cache=Shared";

            g_con = new SqliteConnection(connectionString);
            if(g_con!=null)
            {
                g_con.Open();

                var command1 = g_con.CreateCommand();
                command1.CommandText =
                    "CREATE TABLE testdata ( Text TEXT );" +
                    "INSERT INTO testdata ( Text ) VALUES ( 'Hello Word!!' );";
                command1.ExecuteNonQuery();

           
            }
            // try query
            using (var connection2 = new SqliteConnection(connectionString))
            {
                connection2.Open();

                var command2 = connection2.CreateCommand();
                command2.CommandText = "SELECT Text FROM testdata;";


                var message = command2.ExecuteScalar() as string;
                MessageBox.Show(message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {// do backup
            var connectionString = "Data Source=sharedmemdb;Mode=Memory;Cache=Shared";
            using (var location = new SqliteConnection(connectionString))
            using (var destination = new SqliteConnection(@"Data Source=c:\temp\backupDb.db;"))
            {
                location.Open();
                destination.Open();
                location.BackupDatabase(destination,"main","main");
                if (File.Exists(@"c:\temp\backupDb.db"))
                {
                    MessageBox.Show("Backupdb.db done");
                }
              //  location.BackupDatabase(destination, "main", "main", -1, null, 0);
            }
            // try query
            string dstr = @"Data Source=c:\temp\backupDb.db;";
            
                using (var connection2 = new SqliteConnection(dstr))
            {
                connection2.Open();

                var command2 = connection2.CreateCommand();
                                command2.CommandText = "SELECT Text FROM testdata;";
               // command2.CommandText = ".tables;";


                var message = command2.ExecuteScalar() as string;
                MessageBox.Show(message);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            // try query file 
            string dstr = @"Data Source=c:\temp\backupDb.db;";

            using (var connection2 = new SqliteConnection(dstr))
            {
                connection2.Open();

                var command2 = connection2.CreateCommand();
                command2.CommandText = "SELECT Text FROM testdata;";
                // command2.CommandText = ".tables;";


                var message = command2.ExecuteScalar() as string;
                MessageBox.Show(message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        { // restore and query
            var connectionString = "Data Source=sharedmemdb;Mode=Memory;Cache=Shared";
                g_con = new SqliteConnection(connectionString);
          
                using (var location = new SqliteConnection(@"Data Source=c:\temp\backupDb.db;"))
              {
                    location.Open();
                g_con.Open();
                location.BackupDatabase(g_con);
            }

            // try query
            using (var connection2 = new SqliteConnection(connectionString))
            {
                connection2.Open();

                var command2 = connection2.CreateCommand();
                command2.CommandText = "SELECT Text FROM testdata;";
 //               command2.CommandText = ".tables;";


                var message = command2.ExecuteScalar() as string;
                MessageBox.Show(message);
            }
        }

       
    }
}
