using System;
using System.Collections.Generic;
using MySqlConnector;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        public async void addToDatabase(Object sender, EventArgs e)
        {
            message.Text = "";

            if (string.IsNullOrEmpty(firstName.Text) || string.IsNullOrWhiteSpace (firstName.Text)) {
                message.Text = "Please enter your first name";
                message.TextColor = Color.Red;
            }
            else if (string.IsNullOrEmpty(lastName.Text) || string.IsNullOrWhiteSpace(lastName.Text))
            {
                message.Text = "Please enter your last name";
                message.TextColor = Color.Red;
            }
            else if (string.IsNullOrEmpty(idEntry.Text) || string.IsNullOrWhiteSpace(idEntry.Text))
            {
                message.Text = "Please enter your email address";
                message.TextColor = Color.Red;
            }
            else if (string.IsNullOrEmpty(cellNum.Text) || string.IsNullOrWhiteSpace(cellNum.Text))
            {
                message.Text = "Please enter your cell phone number";
                message.TextColor = Color.Red;
            }
            else if (string.IsNullOrEmpty(password.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                message.Text = "Please enter your password";
                message.TextColor = Color.Red;
            }
            else if (string.IsNullOrEmpty(confirmPassword.Text) || string.IsNullOrWhiteSpace(confirmPassword.Text))
            {
                message.Text = "Please confirm your password";
                message.TextColor = Color.Red;
            }

            else if (!password.Text.Equals(confirmPassword.Text)) {
                message.Text = "Your password does not match";
                message.TextColor = Color.Red;
            }

            else if (!Int64.TryParse(cellNum.Text, out long number2) || cellNum.Text.Length != 10)
            {
                message.Text = "Please enter a valid cell phone number";
                message.TextColor = Color.Red;
            }

            else
            {
                
                long cell = Int64.Parse(cellNum.Text);
                Hasher createHash = new Hasher();
                String salt = createHash.GenerateSalt(10);
                String hashedpass = createHash.GenerateHash(password.Text, salt);
                MySqlConnection conn;
                conn = new MySqlConnection(mysqlStrings.ConnectionString);
                MySqlCommand checkemail = new MySqlCommand("Select count(*) from employee where email = '" + idEntry.Text + "'", conn);
                MySqlCommand command = new MySqlCommand(mysqlStrings.GenerateRegisterCommand(firstName.Text, lastName.Text, idEntry.Text, cell, hashedpass, salt), conn);
                MySqlCommand getID = new MySqlCommand("Select employeeID from employee where email = '" + idEntry.Text + "' and password = '" + hashedpass + "'", conn);
                conn.Open();
                long count = Convert.ToInt64(checkemail.ExecuteScalar());
                if (count > 0)
                {
                    message.Text = "This email is already linked to an account";
                    message.TextColor = Color.Red;
                }
                else
                {
                    command.ExecuteNonQuery();
                    using (MySqlDataReader reader = getID.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee.id = int.Parse(reader[0].ToString());
                        }

                    }
                    conn.Close();
                    Employee.firstName = firstName.Text;
                    Employee.lastName = lastName.Text;
                    Employee.password = hashedpass;
                    Employee.cellNum = cell;
                    Employee.email = idEntry.Text;
                    Employee.switchValue = false;
                    Employee.spotID = 0;
                    await Shell.Current.Navigation.PopToRootAsync(false);
                    await Shell.Current.GoToAsync($"//{nameof(UserPage)}");

                }
                

            }

        }
    }
}
