using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.ViewModels;
using WpfApp2.DB;

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for EditClientPage.xaml
    /// </summary>
    public partial class EditClientPage : Page
    {
        public EditClientPage(Client client)
        {
            InitializeComponent();
            if (client.ID == 0)
                labelID.Visibility = 
                    labelIDValue.Visibility =
                    Visibility.Collapsed;
            DataContext = new EditClientViewModel(client);
        }
    }
}
