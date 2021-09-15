using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.DB
{
    public static class DBInstance
    {
        static db_prepodEntities instance;

        public static db_prepodEntities GetInstance()
        {
            if (instance == null)
                instance = new db_prepodEntities();
            return instance;
        }
    }
}
