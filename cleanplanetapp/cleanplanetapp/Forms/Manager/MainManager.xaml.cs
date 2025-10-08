using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace cleanplanetapp.Forms.Manager
{
    public partial class MainManager : Window
    {
       
        public ObservableCollection<Partner> Partners { get; }
            

        public MainManager()
        {
            InitializeComponent();

            Partners = new ObservableCollection<Partner>();
            DataContext = this;

            Title = $"Главная форма | Пользователь: {Session.emp_name} | Роль: {Session.emp_role}";
            labelInfo.Content = $"Роль : {Session.emp_role}";

       
            Loaded += async (_, __) => await LoadPartnersAsync();
        }

        private async Task LoadPartnersAsync()
        {
            
            using (var ctx = new ApplicationDbContext())
            {
                
                var list = await ctx.Partners
                                    .AsNoTracking()  
                                    .ToListAsync();

                Partners.Clear();
                foreach (var p in list) Partners.Add(p);
            }

            labelItemCount.Content = $"Количество записей : {Partners.Count}";
        }

      
        private async void lbData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lbData.SelectedItem is Partner selectedPartner)
            {
                await EditPartner(selectedPartner);

            }
        }
        private async Task EditPartner(Partner partner)
        {
            var editWindow = new PartnerDetails(partner);
            editWindow.Owner = this;
            editWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? result = editWindow.ShowDialog();

            if (result == true)
            {
                MessageBox.Show("Успешное изменение", "Успех");
                await LoadPartnersAsync();
            }
            lbData.SelectedItem = null;
        }

       

        private async void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbData.SelectedItem is Partner selectedPartner)
            {
                await EditPartner(selectedPartner);

            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new PartnerDetails();
            addWindow.Owner = this;
            addWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                MessageBox.Show("Успешное добавление", "Успех");
                await LoadPartnersAsync();
            }
            lbData.SelectedItem = null;
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbData.SelectedItem is Partner selectedPartner)
            {
                // Подтверждение удаления
                var result = MessageBox.Show(
                    $"Вы действительно хотите удалить партнёра \"{selectedPartner.Name}\" и всю его историю?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool deleted = await DeletePartnerAsync(selectedPartner.PartnerId);
                    if (deleted)
                    {
                        MessageBox.Show("Партнёр удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                        await LoadPartnersAsync();
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите партнёра для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task<bool> DeletePartnerAsync(int partnerId)
        {
            try
            {
                using (var ctx = new ApplicationDbContext())
                {
                    
                    var partner = await ctx.Partners.FindAsync(partnerId);
                    if (partner == null)
                    {
                        MessageBox.Show("Партнёр не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }

                  
                    var historyRecords = ctx.PartnersRatingHistory.Where(h => h.PartnerId == partnerId);
                    ctx.PartnersRatingHistory.RemoveRange(historyRecords);

                    
                    ctx.Partners.Remove(partner);

                 
                    await ctx.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении партнёра: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(Partners);
            if (view == null) return;

            string searchText = tbSearch.Text.Trim().ToLower();

            view.Filter = partnerObj =>
            {
                if (partnerObj is Partner partner)
                {
                    return partner.Name.ToLower().Contains(searchText);
                }
                return false;
            };
        }
    }
}
