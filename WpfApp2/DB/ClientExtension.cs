using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.DB
{
    public partial class Client
    {
        public DateTime? LastVisitDate 
        {
            get
            {
                var lastRow = ClientService.LastOrDefault();
                return lastRow?.StartTime;
            }
        }

        public int CountVisits
        {
            get => ClientService?.Count() ?? 0;
        }
    }
}
