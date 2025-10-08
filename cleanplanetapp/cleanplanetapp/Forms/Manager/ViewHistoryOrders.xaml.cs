using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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

namespace cleanplanetapp.Forms.Manager
{
    /// <summary>
    /// Логика взаимодействия для ViewHistoryOrders.xaml
    /// </summary>
    public partial class ViewHistoryOrders : Window
    {
        public ObservableCollection<Order> PartnerOrders { get; }
        private int _partnerId;

        public ViewHistoryOrders(int partnerId)
        {
            InitializeComponent();
            _partnerId = partnerId;

            PartnerOrders = new ObservableCollection<Order>();

            LoadPartnerOrders();

            DataContext = this;
        }

        private void LoadPartnerOrders()
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                   
                    var orders = ctx.Orders
                                    .Where(o => o.PartnerId == _partnerId)
                                    .Include(o => o.Service)
                                    .ToList();

                    
                    if (!orders.Any())
                    {
                        MessageBox.Show("У партнёра нет выполненных заказов.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                   
                    PartnerOrders.Clear();
                    foreach (var o in orders)
                    {
                        PartnerOrders.Add(o);
                        
                    }

                
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки заказов: " + ex.InnerException.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
