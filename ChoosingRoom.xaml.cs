using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace KP_OOP
{
    /// <summary>
    /// Логика взаимодействия для ChoosingRoom.xaml
    /// </summary>
    public partial class ChoosingRoom : Page
    {
        ObservableCollection<string> listOfrooms; 
        public ChoosingRoom(string func)
        {
            InitializeComponent();
            Files f = new Files();
            listOfrooms = f.ListOfRooms();
            if (func == "create")
            {
                DeleteRoom.Visibility = Visibility.Collapsed;
                ChooseRoom.Visibility = Visibility.Collapsed;
                warning.Content = "Пожалуйста, введите наименование новой комнаты. Не используйте те названия, Которые уже есть!";
                textbox_newroom_name.Visibility = Visibility.Visible;
                ListOfFiles.IsEnabled = false;
            }
            if (func == "delete")
            {
                CreateRoom.Visibility = Visibility.Collapsed;
                ChooseRoom.Visibility = Visibility.Collapsed;
            }
            if (func == "choose")
            {
                CreateRoom.Visibility = Visibility.Collapsed;
                DeleteRoom.Visibility = Visibility.Collapsed;
            }
            ListOfFiles.ItemsSource = listOfrooms;
        }
        bool create = true;

        private string SelectRoom()
        {
            string path = "rooms\\" + ListOfFiles.SelectedItem.ToString() + ".txt";
            return path;
        }
        private void ButtonDeleteRoomClick(object sender, RoutedEventArgs e)
        {
            FileInfo fileInf = new FileInfo(SelectRoom());
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
            foreach (string s in listOfrooms)
            {
                if (s == ListOfFiles.SelectedItem.ToString())
                {
                    listOfrooms.Remove(s);
                    break;
                }
            }
        }
        private void ButtonChooseRoomClick(object sender, RoutedEventArgs e)
        {
            create = false;
            this.NavigationService.Navigate(new Room(SelectRoom(),create));
        }

        private void ButtonCreateRoomClick(object sender, RoutedEventArgs e)
        { 
            foreach (string s in listOfrooms)
            {
                if (s == textbox_newroom_name.Text)
                {
                    create = false;
                    MessageBox.Show("Комната с таким именем уже существует, пожалуйста, измените название!");
                    break;
                }
            }
            if (create)
            {
                string str= "rooms\\" + textbox_newroom_name.Text + ".txt"; 
                FileStream file = File.Create(str);
                file.Close();
                this.NavigationService.Navigate(new Room(str,create));
            }
        }
    }
}
