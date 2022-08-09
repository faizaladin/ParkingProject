using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MySqlConnector;

using Xamarin.Forms;

namespace ParkingProject
{
    public partial class ManageSpot : ContentPage
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
        public string date1;
        public string date2;
        int count = 0;


        public ManageSpot()
        {
            InitializeComponent();

            ReservationView.ItemsSource = reservations;
            if (!(Employee.spotID <= 0))
            {
                freeSpot.IsVisible = true;
                spotinfo.Text = "Spot Number: " + Employee.spotID.ToString();
                
                if (Employee.switchValue)
                {
                    freeSpot.IsToggled = true;
                    popupView.IsVisible = false;
                    currentReservations.IsVisible = true;
                    MySqlConnection conn;
                    MySqlConnection nameconn;
                    conn = new MySqlConnection(mysqlStrings.ConnectionString);
                    nameconn = new MySqlConnection(mysqlStrings.ConnectionString);
                    MySqlCommand loadspots = new MySqlCommand(mysqlStrings.GenerateGetCurrentReservations(Employee.spotID), conn);
                    conn.Open();

                    using (MySqlDataReader reader = loadspots.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firstname = "";
                            string lastname = "";
                            Reservation temp = new Reservation { employeeID = int.Parse(reader[0].ToString()), spotID = int.Parse(reader[1].ToString()), date = reader[2].ToString(), startime = reader[3].ToString(), endtime = reader[4].ToString(), displayname = "Test" };
                            DateTime tempdate = DateTime.Parse(temp.date);
                            string formatdate = tempdate.ToString("MM/dd/yyyy");
                            DateTime tempstart = DateTime.Parse(temp.startime);
                            string formatstart = tempstart.ToString("hh:mm tt");
                            DateTime tempend = DateTime.Parse(temp.endtime);
                            string formatend = tempend.ToString("hh:mm tt");
                            MySqlCommand loadname = new MySqlCommand("Select firstname, lastname from employee where employeeID = " + temp.employeeID, nameconn);
                            nameconn.Open();
                            using (MySqlDataReader namereader = loadname.ExecuteReader())
                            {
                                while (namereader.Read())
                                {
                                    firstname = namereader[0].ToString();
                                    lastname = namereader[1].ToString();
                                }
                            }
                            nameconn.Close();
                            temp.displayname = firstname + " " + lastname + " | " + formatdate + " | " + formatstart + "-" +  formatend;
                            reservations.Add(temp);
                        }

                    }
                    conn.Close();
                }
                else
                {
                    freeSpot.IsToggled = false;
                }

            }
            else
            {
                freeSpot.IsVisible = false;
                linkspot.IsVisible = true;
                spotinfo.FontSize = 15;
                spotinfo.Margin= 15;
                spotinfo.Text = "You currently do not own a spot. To get your own spot please join the waitlist. If you already have a spot but have not linked it, please link the spot.";
            }
            
        }

        public async void changePopup(object sender, EventArgs e)
        {
            if (freeSpot.IsToggled == true)
            {
                popupView.IsVisible = true;
            }
            else if (count == 0)
            {
                bool answer = await DisplayAlert("Alert", "Would you to reclaim your spot?", "Yes", "No");
                if (!answer)
                {
                    freeSpot.IsToggled = true;
                    popupView.IsVisible = false;
                }
                else
                {
                    MySqlConnection conn;
                    conn = new MySqlConnection(mysqlStrings.ConnectionString);
                    MySqlCommand removeAvailability = new MySqlCommand("delete from available_spots where spotID = " + Employee.spotID, conn);
                    MySqlCommand removeReservations = new MySqlCommand("delete from reservation where spotID=" + Employee.spotID, conn);
                    reservations.Clear();
                    conn.Open();
                    removeAvailability.ExecuteNonQuery();
                    removeReservations.ExecuteNonQuery();
                    conn.Close();
                    Employee.switchValue = false;
                }
            }
            
        }

        public void closePopUp(Object sender, EventArgs e)
        {
            count = 1;
            freeSpot.IsToggled = false;
            popupView.IsVisible = false;
            count = 0;
            
        }

        public void makeAvailable(Object sender, EventArgs e)
        {
            List<DateTime> selectedDates = calendar.SelectedDates;
            if (selectedDates != null)
            {
                for (int i = 0; i < selectedDates.Count; i++)
                {
                    
                    string formatdate = selectedDates[i].ToString("yyyy-MM-dd");
                    MySqlConnection conn;
                    conn = new MySqlConnection(mysqlStrings.ConnectionString);
                    MySqlCommand check = new MySqlCommand("select count(*) from available_spots where date = CONVERT(" + "'" + formatdate + "'" + ", DATE) and spotID = " + Employee.spotID, conn);
                    MySqlCommand command = new MySqlCommand("insert into available_spots(spotID, availability, date) values (" + Employee.spotID + ", '000000000000000000000000000000000000000000000000', CONVERT(" + "'" + formatdate + "'" + ", DATE))", conn);
                    conn.Open();
                    int count = Convert.ToInt16(check.ExecuteScalar());
                    if (count == 0)
                    {
                        command.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
            popupView.IsVisible = false;
            Employee.switchValue = true;
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

        public void refreshList()
        {
            reservations.Clear();
            if (!(Employee.spotID <= 0))
            {
                freeSpot.IsVisible = true;
                spotinfo.Text = "Spot Number: " + Employee.spotID.ToString();

                if (Employee.switchValue)
                {
                    freeSpot.IsToggled = true;
                    popupView.IsVisible = false;
                    currentReservations.IsVisible = true;
                    MySqlConnection conn;
                    MySqlConnection nameconn;
                    conn = new MySqlConnection(mysqlStrings.ConnectionString);
                    nameconn = new MySqlConnection(mysqlStrings.ConnectionString);
                    MySqlCommand loadspots = new MySqlCommand(mysqlStrings.GenerateGetCurrentReservations(Employee.spotID), conn);
                    conn.Open();

                    using (MySqlDataReader reader = loadspots.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string firstname = "";
                            string lastname = "";
                            Reservation temp = new Reservation { employeeID = int.Parse(reader[0].ToString()), spotID = int.Parse(reader[1].ToString()), date = reader[2].ToString(), startime = reader[3].ToString(), endtime = reader[4].ToString(), displayname = "Test" };
                            DateTime tempdate = DateTime.Parse(temp.date);
                            string formatdate = tempdate.ToString("MM/dd/yyyy");
                            DateTime tempstart = DateTime.Parse(temp.startime);
                            string formatstart = tempstart.ToString("hh:mm tt");
                            DateTime tempend = DateTime.Parse(temp.endtime);
                            string formatend = tempend.ToString("hh:mm tt");
                            MySqlCommand loadname = new MySqlCommand("Select firstname, lastname from employee where employeeID = " + temp.employeeID, nameconn);
                            nameconn.Open();
                            using (MySqlDataReader namereader = loadname.ExecuteReader())
                            {
                                while (namereader.Read())
                                {
                                    firstname = namereader[0].ToString();
                                    lastname = namereader[1].ToString();
                                }
                            }
                            nameconn.Close();
                            temp.displayname = firstname + " " + lastname + " | " + formatdate + " | " + formatstart + "-" + formatend;
                            reservations.Add(temp);
                        }

                    }
                    conn.Close();
                }
                else
                {
                    freeSpot.IsToggled = false;
                }

            }
            else
            {
                freeSpot.IsVisible = false;
                linkspot.IsVisible = true;
                spotinfo.FontSize = 15;
                spotinfo.Margin = 15;
                spotinfo.Text = "You currently do not own a spot. To get your own spot please join the waitlist. If you already have a spot but have not linked it, please link the spot.";
            }

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            refreshList();
            if (!(Employee.spotID <= 0))
            {
                linkspot.IsVisible = false;
            }
            else
            {
                linkspot.IsVisible = true;
            }
        }

        public void goLink(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new LinkSpot());
        }



    }
}
