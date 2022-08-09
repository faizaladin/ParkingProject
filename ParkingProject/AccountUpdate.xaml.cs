using System;
using System.Collections.Generic;
using MySqlConnector;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class AccountUpdate : ContentPage
    {
        public AccountUpdate()
        {
            InitializeComponent();
            firstName.Text = Employee.firstName;
            lastName.Text = Employee.lastName;
            idEntry.Text = Employee.email;
            cellNum.Text = Employee.cellNum.ToString();
        }

        public async void updateInfo(Object sender, EventArgs e)
        {
            message.Text = "";

            if (string.IsNullOrEmpty(firstName.Text) || string.IsNullOrWhiteSpace(firstName.Text))
            {
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

            else if (!Int64.TryParse(cellNum.Text, out long number2) || cellNum.Text.Length != 10)
            {
                message.Text = "Please enter a valid cell phone number";
                message.TextColor = Color.Red;
            }

            else
            {

                long cell = Int64.Parse(cellNum.Text);
                MySqlConnection conn;
                conn = new MySqlConnection(mysqlStrings.ConnectionString);
                MySqlCommand checkemail = new MySqlCommand("Select count(*) from employee where email = '" + idEntry.Text + "'", conn);
                MySqlCommand command = new MySqlCommand("UPDATE Employee set email = '" + idEntry.Text + "', firstname = '" + firstName.Text + "', lastname = '" + lastName.Text + "', cellnum = " + cell + " where employeeID = " + Employee.id, conn);
                conn.Open();
                long count = Convert.ToInt64(checkemail.ExecuteScalar());
                if (count > 0 && idEntry.Text != Employee.email)
                {
                    message.Text = "This email is already linked to an account";
                    message.TextColor = Color.Red;
                    conn.Close();
                }
                else
                {
                    command.ExecuteNonQuery();
                    Employee.email = idEntry.Text;
                    Employee.firstName = firstName.Text;
                    Employee.lastName = lastName.Text;
                    Employee.cellNum = cell;
                    await Navigation.PopAsync();
                    conn.Close();
                }
                

            }
        }

        public void changePass(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new Checkpass());
        }
    }
}
