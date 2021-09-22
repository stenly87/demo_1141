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

namespace WpfApp2.Views
{
    /// <summary>
    /// Interaction logic for ListClientsPage.xaml
    /// </summary>
    public partial class ListClientsPage : Page
    {
        public ListClientsPage()
        {
            InitializeComponent();
            DataContext = new ListClientsViewModel();
        }

        private void SortClick(object sender, MouseButtonEventArgs e)
        {
            var parametr = ((TextBlock)sender).Tag as string;
            ((ListClientsViewModel)DataContext).Sort(parametr);
        }

        private void SortCountVisitsClick(object sender, MouseButtonEventArgs e)
        {
            ((ListClientsViewModel)DataContext).Sort("CountVisits");
        }
    }
}
