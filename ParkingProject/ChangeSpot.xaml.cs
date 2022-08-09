using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MySqlConnector;

namespace ParkingProject
{
    public partial class ChangeSpot : ContentPage
    {

        ObservableCollection<Spot> spots = new ObservableCollection<Spot>();
        public ObservableCollection<Spot> _spots {

            get
            {
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
        public Reservation currentRes;
        Spot potential;
        int i = 0;

        public ChangeSpot(Reservation temp, string fdate, TimeSpan time1, TimeSpan time2, string reserveDate, bool conflict)
        {
            InitializeComponent();
            this.fdate = fdate;
            this.time1 = time1;
            this.time2 = time2;
            this.reserveDate = reserveDate;
            currentRes = temp;
            totaltime1 = (int)(time1.Hours * 2) + (int)(time1.Minutes / 30);
            totaltime2 = (int)(time2.Hours * 2) + (int)(time2.Minutes / 30);
            int totaltime3 = (int)(TimeSpan.Parse(temp.startime).Hours * 2) + (int)(TimeSpan.Parse(temp.startime).Minutes / 30);
            int totaltime4 = (int)(TimeSpan.Parse(temp.endtime).Hours * 2) + (int)(TimeSpan.Parse(temp.endtime).Minutes / 30);

            for (int x = 0; x < totaltime2 - totaltime1; x++)
            {
                checkAvailability = checkAvailability.Insert(0, "0");
            }

            if (conflict)
            {
                Test.Text = "conflict";
            }

            SpotView.ItemsSource = spots;

            MySqlConnection conn;
            conn = new MySqlConnection(mysqlStrings.ConnectionString);
            MySqlCommand loadspots = new MySqlCommand(mysqlStrings.GenerateAvailableSpotsCommand(fdate), conn);
            MySqlCommand spotflict = new MySqlCommand("Select * from available_spots where date = '" + fdate + "' and spotID = " + temp.spotID, conn);
            conn.Open();

         
            using (MySqlDataReader reader = loadspots.ExecuteReader())
            {

                while (reader.Read())
                {
                    spots.Add(new Spot { spotID = int.Parse(reader[0].ToString()), availability = reader[1].ToString(), displayname = "Spot " + reader[0].ToString() });

                }

            }
        


            for (int z = 0; z < spots.Count; z++)
            {
                if (!spots[z].availability.Substring(totaltime1, totaltime2 - totaltime1).Equals(checkAvailability))
                {
                    spots[z] = null;
                }

            }

            spots.Remove(null);

            if (conflict)
            {
                using (MySqlDataReader reader1 = spotflict.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        potential = new Spot { spotID = int.Parse(reader1[0].ToString()), availability = reader1[1].ToString(), displayname = "Spot " + reader1[0].ToString() };
                    }

                }

                if (totaltime1 < totaltime3 && totaltime4 < totaltime2)
                {
                    if (!potential.availability.Substring(totaltime1, totaltime3 - totaltime1).Contains("1") && !potential.availability.Substring(totaltime4 + 1, totaltime2 - totaltime4).Contains("1"))
                    {
                        spots.Add(potential);
                    }
                }

                else if (totaltime1 < totaltime3)
                {
                    if (!potential.availability.Substring(totaltime1, totaltime3 - totaltime1).Contains("1"))
                    {
                        spots.Add(potential);
                    }

                }

                else if (totaltime4 < totaltime2)
                {
                    if (!potential.availability.Substring(totaltime4 + 1, totaltime2 - totaltime4).Contains("1"))
                    {
                        spots.Add(potential);
                    }
                }
                else if (totaltime1 >= totaltime3  && totaltime2 <= totaltime4)
                {
                    spots.Add(potential);
                }
            }

            conn.Close();

        }

        public async  void finishRes(object sender, EventArgs e)
        {
            string firsttime = time1.ToString();
            string secondtime = time2.ToString();
            string updatestring = "";
            string revert = "";
            string temprevert = "";

            DateTime tempDate = DateTime.Parse(currentRes.date);
            String currentDate = tempDate.ToString("yyyy-MM-dd HH:mm:ss");

            TimeSpan initialStart = TimeSpan.Parse(currentRes.startime);
            TimeSpan initialEnd = TimeSpan.Parse(currentRes.endtime);

            int initialtotaltime1 = (int)(initialStart.Hours * 2) + (int)(initialStart.Minutes / 30);
            int initialtotaltime2 = (int)(initialEnd.Hours* 2) + (int)(initialEnd.Minutes / 30);

            for (int x = 0; x < initialtotaltime2 - initialtotaltime1; x++)
            {
                revert = revert.Insert(0, "0");
            }

            for (int x = 0; x < totaltime2 - totaltime1; x++)
            {
                updatestring = updatestring.Insert(0, "1");
            }

            if (!(SpotView.SelectedItem == null))
            {

                Spot temp = (Spot)SpotView.SelectedItem;
                MySqlConnection conn;
                conn = new MySqlConnection(mysqlStrings.ConnectionString);

                MySqlCommand getAvailability = new MySqlCommand("Select availability from available_spots where date = '" + currentDate + "' and spotID = " + currentRes.spotID, conn);
                MySqlCommand deleteRes = new MySqlCommand("delete from reservation where employeeID = " + Employee.id + " and spotID = " + currentRes.spotID + " and date = '" + currentDate + "' and start_time = '" + currentRes.startime + "' and end_time = '" + currentRes.endtime + "'", conn);
                MySqlCommand insertRes = new MySqlCommand("INSERT into Reservation(employeeID, spotID, date, start_time, end_time) values (" + Employee.id + "," + temp.spotID + "," + "'" + reserveDate + "'" + "," + "'" + firsttime + "'" + "," + "'" + secondtime + "'" + ")", conn);
                MySqlCommand checkstring = new MySqlCommand("Select availability from available_spots where date = CONVERT(" + "'" + fdate + "'" + ", DATE) and spotID =" + temp.spotID, conn);

                MySqlDataReader reader;

                conn.Open();

                using (MySqlDataReader reader1 = getAvailability.ExecuteReader())
                {
                    while (reader1.Read())
                    {
                        temprevert = reader1[0].ToString();

                    }

                }

                revert = temprevert.Substring(0, initialtotaltime1) + revert + temprevert.Substring(initialtotaltime2);

                MySqlCommand revertAvailabilty = new MySqlCommand("UPDATE available_spots set availability = '" + revert + "'WHERE date = '" + currentDate + "' and spotID =" + currentRes.spotID, conn);
                revertAvailabilty.ExecuteNonQuery();

                reader = checkstring.ExecuteReader();
                reader.Read();
                string check = reader[0].ToString();
                reader.Close();

                updatestring = check.Substring(0, totaltime1) + updatestring + check.Substring(totaltime2);

                MySqlCommand updateAvailability = new MySqlCommand("UPDATE available_spots set availability = " + "'" + updatestring + "'" + "WHERE date = CONVERT(" + "'" + fdate + "'" + ", DATE) and spotID =" + temp.spotID, conn);

                if (!check.Substring(totaltime1, totaltime2 - totaltime1).Contains("1"))
                {
                   
                    deleteRes.ExecuteNonQuery();
                    insertRes.ExecuteNonQuery();
                    updateAvailability.ExecuteNonQuery();
                }
                conn.Close();




            }

            await Shell.Current.Navigation.PopToRootAsync(false);

        }
    }
}
