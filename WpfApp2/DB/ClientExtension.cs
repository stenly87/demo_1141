using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.DB
{
    public partial class Client
    {
        public string PhotoPathAbsolute 
        {
            get => Environment.CurrentDirectory + "\\" + PhotoPath;
        }
        public bool GenderMan 
        {
            get => Gender.Code == "м";
            set
            {
                if (value)
                {
                    Gender = DBInstance.GetInstance().Gender.
                        FirstOrDefault(code => code.Code == "м");
                }
            }
        }
        public bool GenderWoman
        {
            get => Gender.Code == "ж";
            set
            {
                if (value)
                {
                    Gender = DBInstance.GetInstance().Gender.
                        FirstOrDefault(code => code.Code == "ж");
                }
            }
        }

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
