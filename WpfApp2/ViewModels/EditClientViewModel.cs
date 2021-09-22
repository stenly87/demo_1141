using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.DB;
using WpfApp2.Tools;

namespace WpfApp2.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        public Client EditClient { get; set; }

        public EditClientViewModel(Client editClient)
        {
            EditClient = editClient;
        }

        public CustomCommand SelectImage { get; set; }
        public CustomCommand Save { get; set; }


    }
}
