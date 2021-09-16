using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2.DB
{
    public partial class Tag
    {
        public string ARGBColor
        {
            get => "#FF" + Color;
        }
    }
}
