using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using cleanplanetapp.Forms.Manager;

namespace cleanplanetapp.Forms
{
    /// <summary>
    /// Логика взаимодействия для Connect.xaml
    /// </summary>
    /// 
    public static class CurrentUser
    {
        private static string username;
        private static string name;
        private static string role;
     
        public static string Username { get { return username; }  set { username = value; } }
        public static string Name { get { return name; } set { name = value; } }
        public static string Role { get { return role; } set { role = value; } }
        
    }

    public partial class Connect : Window
    {

        public Connect()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            CurrentUser.Role = "Менеджер";
            MainManager main = new MainManager();
            
            main.ShowDialog();
        }
    }
}
