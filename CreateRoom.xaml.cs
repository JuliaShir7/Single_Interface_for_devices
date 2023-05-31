using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CreateRoom.xaml
    /// </summary>
    public partial class CreateRoom : Page
    {
        public CreateRoom()
        {
            InitializeComponent();
        }
       
        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            //AddApliance addApliance = new AddApliance();
            //addApliance.Show();
            //Add.Visibility = Visibility.Collapsed;
            //StPanel_AddApp.Visibility = Visibility.Visible;
            if (applianceType.SelectedItem is ApplianceType.Телевизор)
            {
                TV tv = new TV(ApplianceType.Телевизор, newName.Text, newManufacturer.Text, "offTV.jpg", 50, 120, 100, 50, false, 0, 1);

            }
            else
            {
                if (applianceType.SelectedItem is ApplianceType.Чайник)
                {
                    Kettle k = new Kettle(ApplianceType.Чайник, newName.Text, newManufacturer.Text, "offKettle.jpg", 30, 30, 40, 30, false, 0);
                }
                else
                {
                    if (applianceType.SelectedItem is ApplianceType.Кондиционер)
                    {
                        MessageBox.Show("Проверяет");
                    }
                    else
                    {
                        MessageBox.Show("Error");
                    }
                }
            }
        }
       
        public void ButtonAddAppClick(object sender, RoutedEventArgs e)
        {
           
        }
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
