using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gestion_etudiantes
{
    
    public partial class Form1 : Form
    {
        // create connection string ---Mode connecte //
        string connString = "Data Source=DESKTOP-4N95T5P\\SQLEXPRESS01;Initial Catalog=gestion_ecole;Integrated Security=True";
        SqlConnection connection;
        SqlCommand commande;
        SqlDataAdapter dataAdapter; //import dataadapter//
        DataTable datatable; //import datatable//





        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            connection = new SqlConnection(connString);
            connection.Open();

            chargedonnees();





        }

        private void chargedonnees()
        {
            string query = "select * from etudiantes "; // commade sql 
            commande = new SqlCommand(query, connection); // sql commande 
            dataAdapter = new SqlDataAdapter(commande); // dataadapter
            datatable = new DataTable(); // instance datatable
            dataAdapter.Fill(datatable); 
            table.DataSource = datatable;
        }

        private void modifier_Click(object sender, EventArgs e)
        {
            if (table.SelectedRows.Count > 0)
            {
                int selectedRowIndex = table.SelectedRows[0].Index;
                int selectedClientId = Convert.ToInt32(table["id", selectedRowIndex].Value);

                // Get updated values from textboxes or other input fields
                string updatedNom = nom.Text;
                string updatedPrenom = prenom.Text;
                int updatedAge = Convert.ToInt32(age.Text);

                // Update query
                string updateQuery = "UPDATE etudiantes SET nom = @Nom, prenom = @Prenom, age = @Age WHERE id = @ClientId";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    // Add parameters to the command
                    cmd.Parameters.AddWithValue("@Nom", updatedNom);
                    cmd.Parameters.AddWithValue("@Prenom", updatedPrenom);
                    cmd.Parameters.AddWithValue("@Age", updatedAge);
                    cmd.Parameters.AddWithValue("@ClientId", selectedClientId);

                    // Execute the update command
                    cmd.ExecuteNonQuery();
                }

                // Update the corresponding row in the DataTable
                table["nom", selectedRowIndex].Value = updatedNom;
                table["prenom", selectedRowIndex].Value = updatedPrenom;
                table["age", selectedRowIndex].Value = updatedAge;

                // Optionally, update the DataGridView with the new data
                table.Refresh();
            }






        }

        private void ajouter_Click(object sender, EventArgs e)
        {
            string ajo = "INSERT INTO etudiantes (Nom, prenom, age) " +
           "VALUES (@Nom, @prenom, @age)";
            commande = new SqlCommand(ajo, connection);
            commande.Parameters.AddWithValue("@Nom", nom.Text);
            commande.Parameters.AddWithValue("@prenom", prenom.Text);
            commande.Parameters.AddWithValue("@age", age.Text);
            commande.ExecuteNonQuery();
            chargedonnees();
            MessageBox.Show("ajoute avec success ");

        }

        private void table_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void Supprimer_Click(object sender, EventArgs e)
        {
           

            if (table.SelectedRows.Count > 0)
            {
                // Récupérez l'ID de la ligne sélectionnée (assurez-vous d'avoir une colonne d'ID dans votre DataGridView)
                int selectedRowIndex = table.SelectedRows[0].Index;
                int selectedClientId = Convert.ToInt32(table["id", selectedRowIndex].Value);

                // Supprimez la ligne de la base de données
                string deleteQuery = "DELETE FROM etudiantes WHERE id = @ClientId";
                using (SqlCommand cmd = new SqlCommand(deleteQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ClientId", selectedClientId);
                    cmd.ExecuteNonQuery();
                }


                // Supprimez la ligne du DataTable lié au DataGridView
                table.Rows.RemoveAt(selectedRowIndex);
                chargedonnees();
                // Réinitialisez les TextBox
                nom.Text = "";
                prenom.Text = "";
                age.Text = "";
            }
        }
    }



}
    

