using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Логика взаимодействия для Patner.xaml
    /// </summary>
    public partial class PartnerDetails : Window
    {   
        public Partner partner;
        public PartnerDetails(Partner selectedPartner)
        {
            InitializeComponent();
            labelPartnerID.Content = selectedPartner.PartnerId;
            tbName.Text = selectedPartner.Name;
            tbContact.Text = selectedPartner.Contact;
            tbAddress.Text = selectedPartner.Address;
            tbCommission.Text = selectedPartner.Commission.ToString();
            tbRating.Text = selectedPartner.Rating.ToString();
            partner = selectedPartner;
            
        }

        public PartnerDetails()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {   try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    var partnerToUpdate = ctx.Partners.Find(partner.PartnerId);
                    partnerToUpdate.Name = tbName.Text;
                    partnerToUpdate.Contact = tbContact.Text;
                    partnerToUpdate.Address = tbAddress.Text;
                    partnerToUpdate.Commission = decimal.Parse(tbRating.Text);
                    partnerToUpdate.Rating = decimal.Parse(tbRating.Text);

                    ctx.SaveChanges();

                    DialogResult = true;
                    Close();
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + ex.InnerException.InnerException.Message);
            }
            
        }

        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
