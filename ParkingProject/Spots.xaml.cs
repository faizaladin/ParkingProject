using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MySqlConnector;

namespace ParkingProject
{
    public partial class Spots : ContentPage
    {
        ObservableCollection<Spot> spots = new ObservableCollection<Spot>();

        public ObservableCollection<Spot> _spots {

            get {
                return spots;
            }
            set
            {
                spots = value;
                OnPropertyChanged();
            }

        }

        public string fdate;
        public string reserveDate;
        public string test;
        public string checkAvailability = "";
        public TimeSpan time1;
        public TimeSpan time2;
        public int totaltime1;
        public int totaltime2;
        int i = 0;

        public Spots(string fdate, TimeSpan time1, TimeSpan time2, string reserveDate)
        {
            InitializeComponent();
            this.fdate = fdate;
            this.time1 = time1;
            this.time2 = time2;
            this.reserveDate = reserveDate;

            totaltime1 = (int)(time1.Hours * 2) + (int)(time1.Minutes / 30);
            totaltime2 = (int)(time2.Hours * 2) + (int)(time2.Minutes / 30);

            for(int x = 0; x < totaltime2-totaltime1; x++)
            {
               checkAvailability = checkAvailability.Insert(0,"0");
            }

            

            SpotView.ItemsSource = spots;

            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand loadspots = new MySqlCommand(mysqlStrings.GenerateAvailableSpotsCommand(fdate), conn);
            conn.Open();


            using (MySqlDataReader reader = loadspots.ExecuteReader())
            {
                while (reader.Read())
                {
                    spots.Add(new Spot { spotID = int.Parse(reader[0].ToString()), availability = reader[1].ToString(), displayname = "Spot " + reader[0].ToString()});

                }

            }
            conn.Close();

            
            for (int z = 0; z < spots.Count; z++)
            {
                if (!spots[z].availability.Substring(totaltime1, totaltime2-totaltime1).Equals(checkAvailability)) {
                    spots[z] = null;
                }

            }

            spots.Remove(null);


        }

        public async void finishRes(object sender, EventArgs e)
        {
            string firsttime = time1.ToString();
            string secondtime = time2.ToString();
            string updatestring = "";

           

            for (int x = 0; x < totaltime2 - totaltime1; x++)
            {
                updatestring = updatestring.Insert(0, "1");
            }

            if (!(SpotView.SelectedItem == null))
            {
                
                Spot temp = (Spot)SpotView.SelectedItem;
                updatestring = temp.availability.Substring(0, totaltime1) + updatestring + temp.availability.Substring(totaltime2);
                MySqlConnection conn;
                conn = new MySqlConnection(mysqlStrings.ConnectionString);
                MySqlCommand insertRes = new MySqlCommand("INSERT into Reservation(employeeID, spotID, date, start_time, end_time) values (" + Employee.id + "," + temp.spotID + "," + "'" + reserveDate + "'" + "," + "'" + firsttime + "'" + "," + "'" + secondtime + "'" + ")", conn);
                MySqlCommand updateAvailability = new MySqlCommand("UPDATE available_spots set availability = " + "'" + updatestring + "'" + "WHERE date = CONVERT(" + "'" + fdate + "'" + ", DATE) and spotID =" + temp.spotID , conn);
                MySqlCommand checkstring = new MySqlCommand("Select availability from available_spots where date = CONVERT(" + "'" + fdate + "'" + ", DATE) and spotID =" + temp.spotID, conn);
                MySqlDataReader reader;
                conn.Open();
                reader = checkstring.ExecuteReader();
                reader.Read();
                string check = reader[0].ToString();
                reader.Close();
                if (!check.Substring(totaltime1, totaltime2 - totaltime1).Contains("1")){
                    insertRes.ExecuteNonQuery();
                    updateAvailability.ExecuteNonQuery();
                }
                conn.Close();
                
                
                
                
            }

            await Shell.Current.Navigation.PopToRootAsync(false);

        }

       
    }
}
