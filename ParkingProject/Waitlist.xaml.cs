using System;
using System.Collections.Generic;
using MySqlConnector;
using Xamarin.Forms;

namespace ParkingProject
{
    public partial class Waitlist : ContentPage
    {
        public Waitlist()
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
                message.Text = "You cannot join the waitlist as you currently own a spot or you are currently trying to link a spot.";
                message.TextColor = Color.Red;
                joinWaitlist.IsEnabled = false;
                removeWaitlist.IsEnabled = false;
            }
            else if (onWaitlist > 0)
            {
                joinWaitlist.IsEnabled = false;
                removeWaitlist.IsEnabled = true;
                message.Text = "You have successfully joined the waitlist";
            }
            else
            {
                joinWaitlist.IsEnabled = true;
                removeWaitlist.IsEnabled = false;
                message.Text = "";
            }
            conn.Close();
        }

        public void join(Object sender, EventArgs e)
        {
            DateTime timenow = DateTime.Now;
            string resquestDate = timenow.ToString("yyyy-MM-dd HH:mm:ss");
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand insertWaitlist = new MySqlCommand("insert into waitlist(employeeID, request_date) values (" + Employee.id + ", '" + resquestDate + "')", conn);
            conn.Open();
            insertWaitlist.ExecuteNonQuery();
            conn.Close();
            joinWaitlist.IsEnabled = false;
            removeWaitlist.IsEnabled = true;
            message.Text = "You have successfully joined the waitlist";
            message.TextColor = Color.Green;
        }

        public void remove(Object sender, EventArgs e)
        {
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand deleteWaitlist = new MySqlCommand("delete from waitlist where employeeID = " + Employee.id, conn);
            MySqlCommand deleteSpot = new MySqlCommand("delete from spots where employeeID = " + Employee.id, conn);
            conn.Open();
            deleteWaitlist.ExecuteNonQuery();
            deleteSpot.ExecuteNonQuery();
            conn.Close();
            removeWaitlist.IsEnabled = false;
            joinWaitlist.IsEnabled = true;
            message.Text = "";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand checkspot = new MySqlCommand("select count(*) from spots where employeeID = " + Employee.id, conn);
            MySqlCommand checkwaitlist = new MySqlCommand("select count(*) from waitlist where employeeID = " + Employee.id, conn);
            conn.Open();
            long count = Convert.ToInt64(checkspot.ExecuteScalar());
            long onWaitlist = Convert.ToInt64(checkwaitlist.ExecuteScalar());
            if (count > 0)
            {
                message.Text = "You cannot join the waitlist as you currently own a spot or you are currently trying to link a spot.";
                message.TextColor = Color.Red;
                joinWaitlist.IsEnabled = false;
                removeWaitlist.IsEnabled = false;
            }
            else if (onWaitlist > 0)
            {
                joinWaitlist.IsEnabled = false;
                removeWaitlist.IsEnabled = true;
                message.Text = "You have successfully joined the waitlist";
                message.TextColor = Color.Green;
            }
            else
            {
                joinWaitlist.IsEnabled = true;
                removeWaitlist.IsEnabled = false;
                message.Text = "";
            }
            conn.Close();
        }


    }
}
