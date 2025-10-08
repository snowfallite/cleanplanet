using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace cleanplanetapp.Forms.Manager
{
    public partial class PartnerDetails : Window
    {
        public Partner partner;
        public List<string> partnerTypes = new List<string> { "Корпоративный клиент", "Розничный пункт" };

        public PartnerDetails(Partner selectedPartner)
        {
            InitializeComponent();
            partner = selectedPartner;

     
            labelPartnerID.Content = selectedPartner.PartnerId;
            tbName.Text = selectedPartner.Name;
            tbDirector.Text = selectedPartner.Director;
            tbEmail.Text = selectedPartner.Email;
            tbPhone.Text = selectedPartner.Phone;
            tbAddress.Text = selectedPartner.Address;
            tbCommission.Text = selectedPartner.Commission.ToString();
            tbRating.Text = selectedPartner.Rating.ToString();
            cbPartnerType.ItemsSource = partnerTypes;
            cbPartnerType.SelectedItem = selectedPartner.PartnerType;
            Title = $"Редактирование партнёра: {selectedPartner.Name}";

            try
            {
             
                using (var ctx = new ApplicationDbContext())
                {
                    var partnerHistory = ctx.PartnersRatingHistory.FirstOrDefault(p => p.PartnerId == partner.PartnerId);
                    btnHistory.IsEnabled = partnerHistory != null;
                    var partnerHistoryOrders = ctx.Orders.FirstOrDefault(p => p.PartnerId == partner.PartnerId);
                    btnHistoryOrders.IsEnabled = partnerHistoryOrders != null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка при проверке истории партнёра.\n{e.InnerException.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public PartnerDetails()
        {
            InitializeComponent();
            btnHistory.IsEnabled = false;
            btnHistoryOrders.IsEnabled = false;
            cbPartnerType.ItemsSource = partnerTypes;
            cbPartnerType.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    
                    if (string.IsNullOrWhiteSpace(tbName.Text))
                    {
                        MessageBox.Show("Введите название партнёра.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(tbDirector.Text))
                    {
                        MessageBox.Show("Введите директора партнёра.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(tbEmail.Text))
                    {
                        MessageBox.Show("Введите Email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(tbPhone.Text))
                    {
                        MessageBox.Show("Введите телефон.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                   
                    if (string.IsNullOrWhiteSpace(tbAddress.Text))
                    {
                        MessageBox.Show("Введите адрес.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(tbRating.Text))
                    {
                        MessageBox.Show("Введите рейтинг.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(tbCommission.Text))
                    {
                        MessageBox.Show("Введите комиссию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (cbPartnerType.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите тип партнёра.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                  
                    if (!decimal.TryParse(tbRating.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal newRating))
                    {
                        MessageBox.Show("Введите корректное число для рейтинга.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (newRating < 0 || newRating > 5)
                    {
                        MessageBox.Show("Рейтинг должен быть от 0 до 5.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                  
                    if (!decimal.TryParse(tbCommission.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal newCommission))
                    {
                        MessageBox.Show("Введите корректное число для комиссии.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                 
                    if (partner == null)
                    {
                        var newPartner = new Partner
                        {
                            Name = tbName.Text.Trim(),
                            Director = tbDirector.Text.Trim(),
                            Email = tbEmail.Text.Trim(),
                            Phone = tbPhone.Text.Trim(),
                      
                            Address = tbAddress.Text.Trim(),
                            Rating = newRating,
                            Commission = newCommission,
                            PartnerType = cbPartnerType.SelectedItem.ToString()
                        };

                        ctx.Partners.Add(newPartner);
                    }
                    else 
                    {
                        var partnerToUpdate = ctx.Partners.Find(partner.PartnerId);
                        if (partnerToUpdate == null)
                        {
                            MessageBox.Show("Партнёр не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                       
                        if (partnerToUpdate.Rating != newRating)
                        {
                            var historyRecord = new PartnerRatingHistory
                            {
                                PartnerId = partnerToUpdate.PartnerId,
                                OldRating = partnerToUpdate.Rating,
                                NewRating = newRating,
                                ChangedAt = DateTime.Now,
                                ChangedBy = Session.emp_id,
                            
                            };
                            ctx.PartnersRatingHistory.Add(historyRecord);
                        }

                       
                        partnerToUpdate.Name = tbName.Text.Trim();
                        partnerToUpdate.Director = tbDirector.Text.Trim();
                        partnerToUpdate.Email = tbEmail.Text.Trim();
                        partnerToUpdate.Phone = tbPhone.Text.Trim();
                        partnerToUpdate.Address = tbAddress.Text.Trim();
                        partnerToUpdate.Rating = newRating;
                        partnerToUpdate.Commission = newCommission;
                        partnerToUpdate.PartnerType = cbPartnerType.SelectedItem.ToString();
                    }

                    ctx.SaveChanges();
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + (ex.InnerException != null ? $" {ex.InnerException.Message}" : ""));
            }
        }


        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (partner != null)
            {
                var viewHistory = new ViewHistory(partner.PartnerId)
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                viewHistory.ShowDialog();
            }
        }

        private void btnHistoryOrders_Click(object sender, RoutedEventArgs e)
        {
            if (partner != null)
            {
                var viewHistoryOrders = new ViewHistoryOrders(partner.PartnerId)
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                viewHistoryOrders.ShowDialog();
            }
        }
    }
}
