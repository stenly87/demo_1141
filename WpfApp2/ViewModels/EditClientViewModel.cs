using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp2.DB;
using WpfApp2.Tools;

namespace WpfApp2.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        public Client EditClient { get; set; }

        public CustomCommand SelectImage { get; set; }
        public CustomCommand Save { get; set; }

        public EditClientViewModel(Client editClient)
        {
            EditClient = editClient;

            SelectImage = new CustomCommand(()=> 
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() != true)
                    return;
                if (!File.Exists(openFileDialog.FileName))
                {
                    MessageBox.Show("Файл не найден!");
                    return;
                }
                if (new FileInfo(openFileDialog.FileName).Length > 2 * 1024 * 1024)
                {
                    MessageBox.Show("Размер файла превышает 2МБ!");
                    return;
                }
                string destFileName = $"{Environment.CurrentDirectory}\\Клиенты\\" +
                    $"{new FileInfo(openFileDialog.FileName).Name}";
                if (File.Exists(destFileName))
                    destFileName = $"{Environment.CurrentDirectory}\\Клиенты\\" +
                    $"{editClient.ID}{new FileInfo(openFileDialog.FileName).Name}";
                File.Copy(openFileDialog.FileName, destFileName, true);
                editClient.PhotoPath = destFileName.
                    Replace($"{Environment.CurrentDirectory}\\", "");
                SignalChanged("PhotoPathAbsolute"); // не пашет!
            });
        }
    }
}
