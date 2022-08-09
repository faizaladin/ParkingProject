using System;
using System.Collections.Generic;
using MySqlConnector;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class LinkSpot : ContentPage
    {
        public LinkSpot()
        {
            InitializeComponent();
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand checkspot = new MySqlCommand("select count(*) from spots where employeeID = " + Employee.id, conn);
            MySqlCommand checkwaitlist = new MySqlCommand("select count(*) from waitlist where employeeID = " + Employee.id, conn);
            conn.Open();
            long count = Convert.ToInt64(checkspot.ExecuteScalar());
            long onWaitlist = Convert.ToInt64(checkwaitlist.ExecuteScalar());
            if (count > 0)
            {
                confirmSpot.IsEnabled = false;
                message.Text = "Your request is being processed";
                message.TextColor = Color.Green;
            }
            else if (onWaitlist > 0)
            {
                confirmSpot.IsEnabled = false;
                message.Text = "Please remove yourself from the waitlist before linking your spot";
                message.TextColor = Color.Red;
            }
            conn.Close();

        }

        public void giveSpot(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(linkSpot.Text) || string.IsNullOrWhiteSpace(linkSpot.Text))
            {
                message.Text = "Please enter the spot number";
                message.TextColor = Color.Red;
            }
            else if (!Int64.TryParse(linkSpot.Text, out long number2))
            {
                message.Text = "Please enter a valid spot number";
                message.TextColor = Color.Red;
            }
            else if (int.Parse(linkSpot.Text) <= 0 || int.Parse(linkSpot.Text) >= 50)
            {
                message.Text = "Please enter a valid spot number";
                message.TextColor = Color.Red;
            }
            else
            {

                MySqlConnection conn;
                conn = new MySqlConnection(mysqlStrings.ConnectionString);
                MySqlCommand checkspot = new MySqlCommand("select count(*) from spots where spotID = " + int.Parse(linkSpot.Text), conn);
                MySqlCommand addspot = new MySqlCommand("Insert into spots(spotID, spot_type, employeeID) values (-1, 'P', " + Employee.id + ")", conn);
                conn.Open();
                long count = Convert.ToInt64(checkspot.ExecuteScalar());
                if (count > 0)
                {
                    message.Text = "This spot has already been claimed";
                    message.TextColor = Color.Red;
                }
                else
                {
                    addspot.ExecuteNonQuery();
                    message.Text = "Your request is being processed";
                    message.TextColor = Color.Green;
                    confirmSpot.IsEnabled = false;
                }
                conn.Close();
            }

        }

    }
}
