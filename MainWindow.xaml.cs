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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool create = true;
        ObservableCollection<string> listOfrooms;
        public MainWindow()
        {
            InitializeComponent();
            Files files = new Files(this);
            Back.Visibility = Visibility.Hidden;
            listOfrooms = OnReturnListOfRoomsEvent();
            listofRooms.ItemsSource = listOfrooms;
        }
        public delegate ObservableCollection<string> ReturnListOfRoomsDelegat();

        public event ReturnListOfRoomsDelegat ReturnListOfRoomsEvent;
        private ObservableCollection<string> OnReturnListOfRoomsEvent()
        {
            if (ReturnListOfRoomsEvent != null)
                listOfrooms = ReturnListOfRoomsEvent();
            return listOfrooms;
        }   
        private void ChooseAction(string type)
        {
            if (type == "create")
            {
                create = true;
                DeleteRoom.Visibility = Visibility.Collapsed;
                ChooseRoom.Visibility = Visibility.Collapsed;
                CreateRoom.Visibility = Visibility.Visible;
                warning.Content = "Пожалуйста, введите наименование новой комнаты. Не используйте те названия, которые уже есть!";
                textbox_newroom_name.Visibility = Visibility.Visible;
                listofRooms.IsEnabled = false;
            }
            if (type == "delete")
            {
                CreateRoom.Visibility = Visibility.Collapsed;
                ChooseRoom.Visibility = Visibility.Collapsed;
                DeleteRoom.Visibility = Visibility.Visible;
            }
            if (type == "choose")
            {
                CreateRoom.Visibility = Visibility.Collapsed;
                DeleteRoom.Visibility = Visibility.Collapsed;
                ChooseRoom.Visibility = Visibility.Visible;
            }
        }
        private void OpenSubMenu()
        {
            Back.Visibility = Visibility.Visible;
            mainmenu.Visibility = Visibility.Collapsed;
            choosingRoom.Visibility = Visibility.Visible;
           
        }
        private void load_room_Click(object sender, RoutedEventArgs e)
        {
            OpenSubMenu();
            ChooseAction("choose");
        }
        
        private void create_room_Click(object sender, RoutedEventArgs e)
        { 
            OpenSubMenu();
            ChooseAction("create");
        }

        private void delete_room_Click(object sender, RoutedEventArgs e)
        {
            OpenSubMenu();
            ChooseAction("delete");
        }

        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            MainMenu.NavigationService.Navigate(new Frame());
            Back.Visibility = Visibility.Hidden;
            mainmenu.Visibility = Visibility.Visible;
            choosingRoom.Visibility = Visibility.Collapsed;
        }
        private string SelectRoom()
        {
            string path = "rooms\\" + listofRooms.SelectedItem.ToString() + ".txt";
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
                if (s == listofRooms.SelectedItem.ToString())
                {
                    listOfrooms.Remove(s);
                    break;
                }
            }
        }
        private void ButtonChooseRoomClick(object sender, RoutedEventArgs e)
        {
            create = false;
            MainMenu.NavigationService.Navigate(new Room(SelectRoom(), create));
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
                string str = "rooms\\" + textbox_newroom_name.Text + ".txt";
                FileStream file = File.Create(str);
                file.Close();
                MainMenu.NavigationService.Navigate(new Room(str, create));
            }
        }
    }
}

