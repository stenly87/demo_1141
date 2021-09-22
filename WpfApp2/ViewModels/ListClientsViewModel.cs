using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.DB;
using WpfApp2.Tools;

namespace WpfApp2.ViewModels
{
    public class ListClientsViewModel : BaseViewModel
    {
        List<Client> searchResult = new List<Client>();
        int paginationSkip = 0;
        private string searchType;
        private string searchText = "";
        private string viewRowsCount;
        private Gender filterGender;

        public string SearchType
        {
            get => searchType;
            set
            {
                searchType = value;
                Search();
            }
        }
        public List<string> SearchTypes { get; set; }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                Search();
            }
        }

        public List<Gender> Genders { get; set; }
        public Gender FilterGender { 
            get => filterGender;
            set
            {
                filterGender = value;
                Search();
            }
        }
        public List<Client> Clients { get; set; }
        public Client SelectedClient { get; set; }

        public CustomCommand AddClient { get; set; }
        public CustomCommand EditClient { get; set; }
        public CustomCommand RemoveClient { get; set; }

        public CustomCommand ViewBack { get; set; }
        public CustomCommand ViewForward { get; set; }

        public CustomCommand SearchByBirthdayCurrentMonth { get; set; }
        
        public string ViewRows { get; set; }
        public List<string> ViewRowsCountTypes { get; set; }
        public string ViewRowsCount
        {
            get => viewRowsCount;
            set
            {
                viewRowsCount = value;
                paginationSkip = 0;
                PaginationView();
            }
        }

        public ListClientsViewModel()
        {
            Genders = DBInstance.GetInstance().Gender.ToList();
            Genders.Add(new Gender { Name = "Все", Code = "" });
            FilterGender = Genders.Last();
            ViewRowsCountTypes = new List<string>();
            ViewRowsCountTypes.Add("10");
            ViewRowsCountTypes.Add("50");
            ViewRowsCountTypes.Add("200");
            ViewRowsCountTypes.Add("Все");
            ViewRowsCount = ViewRowsCountTypes[0];
            SearchTypes = new List<string>();
            SearchTypes.Add("ФИО");
            SearchTypes.Add("email");
            SearchTypes.Add("Телефон");
            SearchType = SearchTypes[0];

            ViewBack = new CustomCommand(() =>
            {
                int.TryParse(ViewRowsCount, out int countRowsByPage);
                paginationSkip -= countRowsByPage;
                PaginationView();
            });

            ViewForward = new CustomCommand(() =>
            {
                int.TryParse(ViewRowsCount, out int countRowsByPage);
                paginationSkip += countRowsByPage;
                if (paginationSkip >= searchResult.Count())
                    paginationSkip -= countRowsByPage;
                PaginationView();
            });

            SearchByBirthdayCurrentMonth = new CustomCommand(()=> 
            {
                searchResult = DBInstance.GetInstance().Client.
                    Where(s => s.Birthday.HasValue &&
                        s.Birthday.Value.Month == DateTime.Now.Month).ToList();
                PaginationView();
            });

            Search();
        }

        private void Search()
        {
            if (searchType == null)
                return;
            if (searchType == "ФИО")
                searchResult = DBInstance.GetInstance().Client.Where(c =>
                    (c.FirstName.Contains(searchText) ||
                    c.LastName.Contains(searchText) ||
                    c.Patronymic.Contains(searchText)) &&
                    c.GenderCode.Contains(filterGender.Code)).ToList();
            else if (searchType == "email")
                searchResult = DBInstance.GetInstance().Client.Where(c =>
                    c.Email.Contains(searchText) &&
                    c.GenderCode.Contains(filterGender.Code)).ToList();
            else
                searchResult = DBInstance.GetInstance().Client.Where(c =>
                   c.Phone.Contains(searchText) &&
                    c.GenderCode.Contains(filterGender.Code)).ToList();

            paginationSkip = 0;
            PaginationView();
        }

        private void PaginationView()
        {
            int.TryParse(ViewRowsCount, out int countRowsByPage);

            if (paginationSkip < 0)
                paginationSkip = 0;
            try
            {
                if (countRowsByPage != 0)
                    Clients = searchResult.Skip(paginationSkip).Take(countRowsByPage).ToList();
                else
                    Clients = searchResult;
                ViewRows = $"{paginationSkip + Clients.Count()} из {searchResult.Count()}";
                SignalChanged(nameof(Clients));
                SignalChanged(nameof(ViewRows));
            }
            catch
            {
                
            }
        }

        internal void Sort(string parametr)
        {
            if (parametr == "LastName")
                searchResult.Sort((x, y) => x.LastName.CompareTo(y.LastName));
            else if (parametr == "LastVisitDate")
            {
                var sorted = searchResult.
                    Where(cl => cl.LastVisitDate != null).ToList();
                sorted.Sort((x, y) => y.LastVisitDate?.CompareTo(x.LastVisitDate) ?? 1);
                searchResult.RemoveAll(cl => cl.LastVisitDate != null);
                searchResult.InsertRange(0, sorted);
            }
            else if (parametr == "CountVisits")
                searchResult.Sort((x, y) => y.CountVisits.CompareTo(x.CountVisits));
            PaginationView();
        }

    }
}
