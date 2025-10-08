using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using cleanplanetapp.Svc;

namespace cleanplanetapp.Forms
{
   
    public static class Session
    {
        public static int emp_id {  get; set; }
        public static string emp_name { get; set; }
        public static string emp_role { get; set; }
        
        
    }

    public partial class Connect : Window
    {

        public Connect()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var hash = HashGenerator.ComputeSHA512(pbPassword.Password);
                string login = tbLogin.Text.Trim();
                Employee employee;
                using (var ctx = new ApplicationDbContext())
                {
                    employee = ctx.Employees
                                       .FirstOrDefault(emp =>
                                            emp.Username == login &&
                                            emp.PasswordHash == hash);

                    if (employee == null)
                    {
                        MessageBox.Show("Неверный логин или пароль",
                                       "Ошибка авторизации",
                                       MessageBoxButton.OK,
                                       MessageBoxImage.Error);
                        return;
                    }
                    Session.emp_id = employee.EmployeeId;
                    Session.emp_name = employee.FullName;
                    Session.emp_role = employee.Position.PositionName;
                }
                
                if (employee.Position.PositionName == "Менеджер")
                {
                    MainManager main = new MainManager();

                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("У вас нет доступа к этой программе",
                                    "Ошибка авторизации",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
