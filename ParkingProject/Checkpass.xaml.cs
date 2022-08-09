using System;
using System.Collections.Generic;
using MySqlConnector;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class Checkpass : ContentPage
    {
        public Checkpass()
        {
            InitializeComponent();
        }

        public void confirmpass(object sender, EventArgs e)
        {
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            Hasher createHash = new Hasher();
            String salt = "";
            MySqlCommand retrieveSalt = new MySqlCommand("Select salt from employee where employeeID = " + Employee.id, conn);
            conn.Open();
            using (MySqlDataReader reader = retrieveSalt.ExecuteReader())
            {
                while (reader.Read())
                {
                    salt = reader[0].ToString();
                }
            }
            conn.Close();

            if (createHash.GenerateHash(oldpass.Text, salt) == Employee.password)
            {
                Shell.Current.Navigation.PushAsync(new Changepass());
            }
            else
            {
                message.Text = "Incorrect Password";
                message.TextColor = Color.Red;
            }
        }
    }
}
