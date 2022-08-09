using System;
using System.Collections.Generic;
using MySqlConnector;
using Xamarin.Forms;

namespace ParkingProject
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
           
        }

        private async void checkLogin(Object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(idEntry.Text) || string.IsNullOrWhiteSpace(idEntry.Text))
            {
                message.Text = "Please enter your email address";
                message.TextColor = Color.Red;
            }

            else if (string.IsNullOrEmpty(password.Text) || string.IsNullOrWhiteSpace(password.Text))
            {
                message.Text = "Please enter your password";
                message.TextColor = Color.Red;
            }


            else
            {
                string pass = password.Text;
                MySqlConnection conn;
                conn = new MySqlConnection(mysqlStrings.ConnectionString);
                MySqlCommand retrieveSalt = new MySqlCommand("Select salt from employee where email = '" + idEntry.Text + "'", conn);
                Hasher createHash = new Hasher();
                String salt = "";
        
                
                
                conn.Open();

                using (MySqlDataReader reader = retrieveSalt.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        salt = reader[0].ToString();
                    }
                }

                String hashedpass = createHash.GenerateHash(password.Text, salt);

                MySqlCommand verifyLogin = new MySqlCommand(mysqlStrings.GenerateLoginCommand(idEntry.Text, hashedpass), conn);
                MySqlCommand retrieveUser = new MySqlCommand(mysqlStrings.GenerateRetrieveEmployeeInfoCommand(idEntry.Text, hashedpass), conn);

                long count = Convert.ToInt64(verifyLogin.ExecuteScalar());

                if (count == 1)
                {

                    using (MySqlDataReader reader = retrieveUser.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Employee.email = reader[0].ToString();
                            Employee.id = int.Parse(reader[1].ToString());
                            Employee.firstName = reader[2].ToString();
                            Employee.lastName = reader[3].ToString();
                            Employee.cellNum = Int64.Parse(reader[4].ToString());
                            Employee.password = reader[5].ToString();
                        }

                    }
                    
                    MySqlCommand retrieveSpot = new MySqlCommand(mysqlStrings.GenerateRetrievePermanentSpotCommand(Employee.id), conn);

                    using (MySqlDataReader reader = retrieveSpot.ExecuteReader()) {
                        while (reader.Read())
                        {
                            Employee.spotID = int.Parse(reader[0].ToString());
                        }

                    }

                    MySqlCommand retreiveSwitch = new MySqlCommand(mysqlStrings.GenerateSwitchCommand(Employee.spotID), conn);

                    

                    using (MySqlDataReader reader = retreiveSwitch.ExecuteReader())
                    {
                        int counter = 0;
                        while (reader.Read())
                        {
                            counter = int.Parse(reader[0].ToString());
                        }

                        if (counter > 0)
                        {
                            Employee.switchValue = true;
                        }

                        else
                        {
                            Employee.switchValue = false;
                        }
                    }

                    await Shell.Current.Navigation.PopToRootAsync(false);
                    await Shell.Current.GoToAsync($"//{nameof(UserPage)}");

                    idEntry.Text = null;
                    password.Text = null;

                }
                else
                {
                    message.Text = "Invalid Login";
                    message.TextColor = Color.Red;
                }

                conn.Close();
            }
        }
    }
}
