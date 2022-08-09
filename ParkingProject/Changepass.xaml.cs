using System;
using System.Collections.Generic;
using MySqlConnector;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class Changepass : ContentPage
    {
        public Changepass()
        {
            InitializeComponent();
        }

        public void changePass(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(newpass.Text) || string.IsNullOrWhiteSpace(newpass.Text))
            {
                message.Text = "Please enter your new password";
                message.TextColor = Color.Red;
            }

            else if (!newpass.Text.Equals(confirmpass.Text)) {
                message.Text = "Your password does not match";
                message.TextColor = Color.Red;
            }

            else
            {
                MySqlConnection conn;
                conn = new MySqlConnection(mysqlStrings.ConnectionString);
                Hasher createHash = new Hasher();
                String salt = createHash.GenerateSalt(10);
                String hashedpass = createHash.GenerateHash(newpass.Text, salt);
                MySqlCommand command = new MySqlCommand("update employee set password = '" + hashedpass + "' , salt = '" + salt + "' where employeeID = " + Employee.id, conn);
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                Employee.password = newpass.Text;
                Navigation.PopAsync();
                Navigation.PopAsync();
            }
        }

    }
}
