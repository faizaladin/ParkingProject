using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using MySqlConnector;

namespace ParkingProject
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private void goToSignup(Object sender, EventArgs e) {
            Shell.Current.GoToAsync($"{nameof(SignupPage)}");
        }

        private void goToLogin(Object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"{nameof(Login)}");
        }


        MySqlConnection conn;
        private void Connect(Object sender, EventArgs e)
        {
            conn = new MySqlConnection("Server=192.168.1.36;User ID=faiz;Password=faiz;Database=ParkingDB");
            conn.Open();
            conn.Close();
            
        }
    }
}
