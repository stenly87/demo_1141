using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp2.DB;
using WpfApp2.Tools;
using WpfApp2.Views;

namespace WpfApp2.ViewModels
{
    public class EditClientViewModel : BaseViewModel
    {
        private readonly Image img;

        public Client EditClient { get; set; }

        public CustomCommand SelectImage { get; set; }
        public CustomCommand Save { get; set; }

        public EditClientViewModel(Client editClient, Image img)
        {
            EditClient = editClient;
            this.img = img;
            if (EditClient.PhotoPath != null)
            {
                SetImageSource(img);
            }
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
                
                string currDir = Environment.CurrentDirectory;
                string fileName = new FileInfo(openFileDialog.FileName).Name;
                string destFileName = $"{currDir}\\Клиенты\\{fileName}";

                if (File.Exists(destFileName))
                {
                    fileName = editClient.ID + fileName;
                    destFileName = $"{currDir}\\Клиенты\\{fileName}";
                }

                img.Source = null;
                File.Copy(openFileDialog.FileName, destFileName, true);
                editClient.PhotoPath = $"\\Клиенты\\{fileName}";

                SetImageSource(img);
            });

            Save = new CustomCommand(()=>
            {
                if (EditClient.ID != 0)
                {
                    var old = DBInstance.GetInstance().Client.
                        FirstOrDefault(c => c.ID == EditClient.ID);
                    DBInstance.GetInstance().Entry(old).
                        CurrentValues.SetValues(EditClient);
                }
                else
                {
                    if (EditClient.FirstName == null ||
                        EditClient.LastName == null ||
                        EditClient.Phone == null ||
                        EditClient.Gender == null)
                    {
                        MessageBox.Show("Данные клиента введены не полностью");
                        return;
                    }
                    EditClient.RegistrationDate = DateTime.Now;
                    DBInstance.GetInstance().Client.Add(EditClient);
                }
                try
                {
                    DBInstance.GetInstance().SaveChanges();
                }
                catch (System.Data.Entity.Core.EntitySqlException e)
                {
                    MessageBox.Show($"EntitySqlException. Ошибка {e.Message}");
                    return;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Нет связи с сервером. Ошибка {e.Message}");
                    return;
                }
                MainWindow.Navigate(new ListClientsPage());
            });
        }

        private void SetImageSource(Image img)
        {
            var path = Environment.CurrentDirectory + "\\" + EditClient.PhotoPath;
            var viewImg = new BitmapImage();
            viewImg.BeginInit();
            viewImg.CacheOption = BitmapCacheOption.OnLoad;
            viewImg.UriSource = new Uri(path, UriKind.Absolute);
            viewImg.EndInit();
            img.Source = viewImg;
        }
    }
}
