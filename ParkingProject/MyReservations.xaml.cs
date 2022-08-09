using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySqlConnector;
using Xamarin.Forms;

namespace ParkingProject
{
    public partial class MyReservations : ContentPage
    {
        ObservableCollection<Reservation> reservations = new ObservableCollection<Reservation>();
        public ObservableCollection<Reservation> _reservations {

            get
            {
                return reservations;
            }
            set
            {
                reservations = value;
                OnPropertyChanged();
            }
        }

        public MyReservations()
        {
            InitializeComponent();

            ReservationView.ItemsSource = reservations;
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand loadspots = new MySqlCommand(mysqlStrings.GenerateMyReservations(Employee.id), conn);
            conn.Open();

            using (MySqlDataReader reader = loadspots.ExecuteReader())
            {
                while (reader.Read())
                {
                    Reservation temp = new Reservation { employeeID = int.Parse(reader[0].ToString()), spotID = int.Parse(reader[1].ToString()), date = reader[2].ToString(), startime = reader[3].ToString(), endtime = reader[4].ToString(), displayname = "Test" };
                    DateTime tempdate = DateTime.Parse(temp.date);
                    string formatdate = tempdate.ToString("MM/dd/yyyy");
                    DateTime tempstart = DateTime.Parse(temp.startime);
                    string formatstart = tempstart.ToString("hh:mm tt");
                    DateTime tempend = DateTime.Parse(temp.endtime);
                    string formatend = tempend.ToString("hh:mm tt");
                    temp.displayname = Employee.firstName + " " + Employee.lastName + " | " + formatdate + " | " + formatstart + "-" + formatend;
                    reservations.Add(temp);
                }

            }
            conn.Close();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            Reservation model = item.BindingContext as Reservation;
            DateTime date = DateTime.Parse(model.date);
            string finaldate = date.ToString("yyyy-MM-dd");
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand getSpot = new MySqlCommand("select availability from available_spots where date = '" + finaldate + "' and spotID = " + model.spotID, conn);
            DateTime tempstart = DateTime.Parse(model.startime);
            TimeSpan formatstart = tempstart.TimeOfDay;
            DateTime tempend = DateTime.Parse(model.endtime);
            TimeSpan formatend = tempend.TimeOfDay;

            int totaltime1;
            int totaltime2;

            totaltime1 = (int)(formatstart.Hours * 2) + (int)(formatstart.Minutes / 30);
            totaltime2 = (int)(formatend.Hours * 2) + (int)(formatend.Minutes / 30);

            string updatestring = "";
            string finalstring = "";

            for (int x = 0; x < totaltime2 - totaltime1; x++)
            {
                updatestring = updatestring.Insert(0, "0");
            }

            conn.Open();


            using (MySqlDataReader reader = getSpot.ExecuteReader())
            {
                while (reader.Read())
                {
                    finalstring = reader[0].ToString();

                }

            }


            updatestring = finalstring.Substring(0, totaltime1) + updatestring + finalstring.Substring(totaltime2);

            MySqlCommand updateAvailability = new MySqlCommand("UPDATE available_spots set availability = " + "'" + updatestring + "'" + "WHERE  date = '" + finaldate + "' and spotID =" + model.spotID, conn); ;

            updateAvailability.ExecuteNonQuery();
            string fdate = date.ToString("yyyy-MM-dd HH:mm:ss");
            MySqlCommand removeRes = new MySqlCommand("delete from reservation where employeeID = " + model.employeeID + " and spotID = " + model.spotID + " and date = '" + fdate + "' and start_time = '" + model.startime + "' and end_time = '" + model.endtime + "'", conn);
            removeRes.ExecuteNonQuery();

            conn.Close();



            reservations.Remove(model);


        }

        private void OnEdit(object sender, EventArgs e)
        {
            SwipeItem item = sender as SwipeItem;
            Reservation model = item.BindingContext as Reservation;
            Shell.Current.Navigation.PushAsync(new ChangeRes(model));
        }

        public void clicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(MakeRes)}");
        }

        public void refreshList()
        {
            reservations.Clear();
            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand loadspots = new MySqlCommand(mysqlStrings.GenerateMyReservations(Employee.id), conn);
            conn.Open();

            using (MySqlDataReader reader = loadspots.ExecuteReader())
            {
                while (reader.Read())
                {
                    Reservation temp = new Reservation { employeeID = int.Parse(reader[0].ToString()), spotID = int.Parse(reader[1].ToString()), date = reader[2].ToString(), startime = reader[3].ToString(), endtime = reader[4].ToString(), displayname = "Test" };
                    DateTime tempdate = DateTime.Parse(temp.date);
                    string formatdate = tempdate.ToString("MM/dd/yyyy");
                    DateTime tempstart = DateTime.Parse(temp.startime);
                    string formatstart = tempstart.ToString("hh:mm tt");
                    DateTime tempend = DateTime.Parse(temp.endtime);
                    string formatend = tempend.ToString("hh:mm tt");
                    temp.displayname = Employee.firstName + " " + Employee.lastName + " | " + formatdate + " | " + formatstart + "-" + formatend;
                    reservations.Add(temp);
                }

            }
            conn.Close();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            refreshList();
        }
    }
}
