using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace cleanplanetapp.Forms.Manager
{
    public partial class MainManager : Window
    {
       
        public ObservableCollection<Partner> Partners { get; } =
            new ObservableCollection<Partner>();

        public MainManager()
        {
            InitializeComponent();

           
            DataContext = this;

           
            labelInfo.Content = $"Роль : {CurrentUser.Role}";

       
            Loaded += async (_, __) => await LoadPartnersAsync();
        }

        private async Task LoadPartnersAsync()
        {
            
            using (var ctx = new ApplicationDbContext())
            {
                
                var list = await ctx.Partners
                                    .AsNoTracking()   // только чтение
                                    .ToListAsync();

                // Очищаем старый список и заполняем новыми данными
                Partners.Clear();
                foreach (var p in list) Partners.Add(p);
            }

            // Обновляем подпись с количеством
            labelItemCount.Content = $"Количество записей : {Partners.Count}";
        }

      
        private async void lbData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lbData.SelectedItem is Partner selectedPartner)
            {
          
                var editWindow = new PartnerDetails(selectedPartner);
                editWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                // Ожидаем закрытия окна
                bool? result = editWindow.ShowDialog();

                // Если пользователь нажал «Сохранить» → перечитываем список
                if (result == true)
                {
                    MessageBox.Show("Успешное изменение", "Успех");
                    await LoadPartnersAsync();   // простейший способ обновить UI
                }

                // Снимаем выделение, чтобы можно было кликнуть тот же элемент ещё раз
                lbData.SelectedItem = null;
            }
        }
    }
}
