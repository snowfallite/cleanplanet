using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
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

namespace cleanplanetapp.Forms.Manager
{
    
    public partial class ViewHistory : Window
    {
        public ObservableCollection<PartnerRatingHistory> PartnerRatingHistory { get; }
        public int _partnerId;
        public ViewHistory(int partnerId)
        {
            InitializeComponent();
            _partnerId = partnerId;
    
            PartnerRatingHistory = new ObservableCollection<PartnerRatingHistory>();

            LoadPartnerRatingHistory();
            DataContext = this;

        }

        private void LoadPartnerRatingHistory()
        {

            try
            {
                using (var ctx = new ApplicationDbContext())
                {

                    var list = ctx.PartnersRatingHistory
                                        .Where(p => p.PartnerId == _partnerId).Include(p => p.Partner).Include(p => p.Employee).ToList();
                    Title = "История рейтинга партнера: " + list.FirstOrDefault().Partner.Name;
                    PartnerRatingHistory.Clear();
                    foreach (var p in list) PartnerRatingHistory.Add(p);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки истории рейтинга: " + ex.Message + "\n " + ex.InnerException.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);


            }
        }
    }
}
