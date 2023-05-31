using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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



namespace KP_OOP
{
    /// <summary>
    /// Логика взаимодействия для Room.xaml
    /// </summary>
    
    public partial class Room : Page
    {
        Point? p;
        Image image;
        Appliance appliance;
        ObservableCollection<Appliance> apps = new ObservableCollection<Appliance> { };
       
        public Room(string path, bool create)
        {
            InitializeComponent();
            Files f = new Files(this);
            RemoteControl rc = new RemoteControl(this);
            applianceType.ItemsSource = Enum.GetValues(typeof(ApplianceType)).Cast<ApplianceType>();
            apps = OnFillEvent(path, apps);
            Apps.ItemsSource = apps;
            foreach (Appliance a in apps)
            {
                Area.Children.Add(CreateImage(a));
            }
            string[] n = path.Split('\\', '.');
            foreach (string i in n)
            {
                if (!i.Contains("txt") && !i.Contains("rooms"))
                {
                    roomName.Text = i;
                }
            }
            if (create)
            {
                Delete.Visibility = Visibility.Collapsed;
                StPanel_AppInfo.Visibility = Visibility.Collapsed;
                Remote.Visibility = Visibility.Collapsed;
            }
        }
        public delegate ObservableCollection<Appliance> FillCollectionDelegat(string path, ObservableCollection<Appliance> apps);
        public event FillCollectionDelegat FillEvent;
        private ObservableCollection<Appliance> OnFillEvent(string path, ObservableCollection<Appliance> apps)
        {
            if (FillEvent != null)
                FillEvent(path, apps);
            return apps;
        }

        public delegate void SaveChangesDelegat(ObservableCollection<Appliance> apps, string room);
        public event SaveChangesDelegat SaveChangesEvent;
        private void OnSaveChangesEvent(ObservableCollection<Appliance> apps, string room)
        {
            if (SaveChangesEvent != null)
                SaveChangesEvent(apps, room);
        }

        public delegate ObservableCollection<Appliance> TurnApplianceDelegat(Appliance a, ObservableCollection<Appliance> apps, Canvas c);
        public event TurnApplianceDelegat TurnEvent;
        private ObservableCollection<Appliance> OnTurnEvent(Appliance a, ObservableCollection<Appliance> apps, Canvas c)
        {
            if (TurnEvent != null)
               apps=TurnEvent(a, apps, c);
            return apps;
        }

        public delegate ObservableCollection<Appliance> WorkWithApplianceDelegat(ObservableCollection<Appliance> apps, Appliance a, Canvas c, Slider slider, int? num);
        public event WorkWithApplianceDelegat WorkWithApplianceEvent;
        private ObservableCollection<Appliance> OnWorkWithApplianceEvent(ObservableCollection<Appliance> apps, Appliance a, Canvas c, Slider slider, int? num)
        {
            if (WorkWithApplianceEvent != null)
                apps = WorkWithApplianceEvent(apps, a, c, slider, num);
            return apps;
        }

        private Image CreateImage(Appliance a)
        {
            Image img = new Image();
            img.Source = new BitmapImage(new Uri(a.Img, UriKind.Relative));
            img.Name = ImageName(a);
            img.Stretch = Stretch.Fill;
            Canvas.SetLeft(img, a.X);
            Canvas.SetTop(img, a.Y);
            img.Width = a.ImgWidth;
            img.Height = a.ImgHeight;
            img.MouseDown += ImageMouseDown;
            img.MouseMove += ImageMouseMove;
            img.MouseUp += ImageMouseUp;
            return img;
        } //добавляем изображение прибора на Canvas
        private string ImageName(Appliance a)
        {
            string[] str = a.Name.Split(' ');
            string name = null;
            foreach (string s in str)
            {
                name = name + s;
            }
            return name;
        }
        private void DeleteApplianceImage(Appliance a)
        {
            var images = Area.Children.OfType<Image>().ToList();
            foreach (Image i in images)
            {
                if (i.Name == ImageName(a))
                {
                    Area.Children.Remove(i);
                }
            }
        } 
        private void ButtonBackClick(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Frame());
        }
        private void ImageMouseDown(object sender, MouseButtonEventArgs e)
        {
            image = sender as Image;
            p = e.GetPosition(Area);
            foreach (Appliance a in apps)
            {
                if (((a.X > e.GetPosition(Area).X)||(e.GetPosition(Area).X < (a.X + a.ImgWidth)))&&((a.Y > e.GetPosition(Area).Y)||(e.GetPosition(Area).Y < (a.Y + a.ImgHeight))))
                {
                    appliance = a;
                }
            }
            image.CaptureMouse();
        }
        private void ImageMouseMove(object sender, MouseEventArgs e)
        {
            if (p == null)
                return;
            var point = e.GetPosition(this) - (Vector)p.Value;
            Canvas.SetLeft(image, point.X);
            Canvas.SetTop(image, point.Y);
            SetCoordinate(point, appliance);
        }
        
        private void ImageMouseUp(object sender, MouseButtonEventArgs e)
        {
            p = null;
            image.ReleaseMouseCapture();
        }
        private void SetCoordinate(Point p, Appliance app)
        {
            app.X = p.X;
            app.Y = p.Y;
            ChooseAppForImage(app.ToString());
        } 
        private void ChooseAppForImage(string s)
        {
            foreach (var item in Apps.Items)
            {
                if (item.ToString() == s)
                {
                    Apps.SelectedValue = item;
                }
            }
        } //выбор прибора при нажатии на изображение
        private void applianceType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (applianceType.SelectedItem.ToString() == ApplianceType.Телевизор.ToString())
            {
                AddFuncProperty.Content = "Введите число подключенных каналов";
                AddFuncProperty.Visibility = Visibility.Visible;
                newAddFuncProperty.Visibility = Visibility.Visible;
            }
            if (applianceType.SelectedItem.ToString() == ApplianceType.Пылесос.ToString() || applianceType.SelectedItem.ToString() == ApplianceType.Вентилятор.ToString())
            {
                AddFuncProperty.Visibility = Visibility.Visible;
                AddFuncProperty.Content = "Введите максимальный уровень мощности";
                newAddFuncProperty.Visibility = Visibility.Visible;
            }
        }  //дополнительная информация при добавлении нового прибора
        private void ButtonSaveClick(object sender, RoutedEventArgs e) 
        {
            OnSaveChangesEvent(apps, roomName.Text);
        }
        private Appliance ChooseAppliance()
        {
            Appliance app = null;
            foreach (Appliance a in apps)
            {
                if (Apps.SelectedItem.ToString() == a.Name)
                {
                    app = a;
                    break;
                }
            }
            return app;
        } 
        private void ButtonDeleteClick(object sender, RoutedEventArgs e)
        {
            DeleteApplianceImage(ChooseAppliance());
            apps.Remove(ChooseAppliance());
        }
        private void ButtonAddClick(object sender, RoutedEventArgs e)
        {
            Add.Visibility = Visibility.Collapsed;
            StPanel_AppInfo.Visibility = Visibility.Collapsed;
            StPanel_AddApp.Visibility = Visibility.Visible;
        } //активируем панель для добавления прибора
        private void ButtonAddAppClick(object sender, RoutedEventArgs e)
        {
            Appliance a = new VacuumCleaner(ApplianceType.Пылесос, newName.Text, newManufacturer.Text, "Resources\\offVacuumCleaner.jpg", 100, 120, 70, 100, false, Convert.ToInt32(newAddFuncProperty.Text), 0);
            if (applianceType.SelectedItem is ApplianceType.Телевизор)
            {
                a = new TV(ApplianceType.Телевизор, newName.Text, newManufacturer.Text, "Resources\\offTV.jpg", 50, 120, 90, 50, false, 0, Convert.ToInt32(newAddFuncProperty.Text), 1);
            }
            else
            {
                if (applianceType.SelectedItem is ApplianceType.Радио)
                {
                    a = new Radio(ApplianceType.Радио, newName.Text, newManufacturer.Text, "Resources\\offRadio.jpg", 20, 130, 60, 60, false, 0, 0);
                }
                else
                {
                    if (applianceType.SelectedItem is ApplianceType.Кондиционер)
                    {
                        a = new Condition(ApplianceType.Кондиционер, newName.Text, newManufacturer.Text, "Resources\\offCondition.jpg", 50, 40, 80, 50, false, 20);
                    }
                    else
                    {
                        if (applianceType.SelectedItem is ApplianceType.Холодильник)
                        {
                            a = new Fridge(ApplianceType.Холодильник, newName.Text, newManufacturer.Text, "Resources\\offFridge.jpg", 120, 100, 60, 150, false, 15, false);
                        }
                        else
                        {
                            if (applianceType.SelectedItem is ApplianceType.Чайник)
                            {
                                a = new Kettle(ApplianceType.Чайник, newName.Text, newManufacturer.Text, "Resources\\offKettle.jpg", 180, 170, 60, 70, false, 0);
                            }
                            else
                            {
                                if (applianceType.SelectedItem is ApplianceType.Вентилятор)
                                {
                                    a = new Blower(ApplianceType.Вентилятор, newName.Text, newManufacturer.Text, "Resources\\offBlower.jpg", 70, 110, 60, 110, false, Convert.ToInt32(newAddFuncProperty.Text), 0);
                                }
                            }
                        }
                    }
                }
            }
            apps.Add(a);
            Area.Children.Add(CreateImage(a));
            Add.Visibility = Visibility.Visible;
            StPanel_AppInfo.Visibility = Visibility.Visible;
            StPanel_AddApp.Visibility = Visibility.Collapsed;
        } //добавляем прибор
        private void ButtonTurnApplianceClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ChooseAppliance().Turn == true)
                {
                    turn.Content = "Включить";
                }
                else
                {
                    turn.Content = "Выключить";
                }
                apps = OnTurnEvent(ChooseAppliance(), apps, Area);
                Area.Children.Add(CreateImage(ChooseAppliance()));
                EnableFunctions(ChooseAppliance());
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Пожалуйта, выберите прибор, который хотите включить!");
            }
        } 
        private void HideRemoteControl()
        {
            SP_forButton.Visibility = Visibility.Collapsed;
            SP_forSlider.Visibility = Visibility.Collapsed;
            SP_forSlider2.Visibility = Visibility.Collapsed;
            SP_forTextBox.Visibility = Visibility.Collapsed;
        } 
        private Appliance ChooseFunctions(Appliance a)
        {
            sliderF.IsEnabled = true;
            SP_forSlider.Visibility = Visibility.Visible;
            sliderF.TickFrequency = 1;
            sliderF.Minimum = 0;
            sliderF.Maximum = 100;
            if (a is WorkingWithSound)
            {
                label_sliderFunc.Content = "Громкость";
                sliderF.Value = (a as WorkingWithSound).Loud;
            }
            if (a is WorkingWithTemperature)
            {
                label_sliderFunc.Content = "Температура";
                sliderF.Value = (a as WorkingWithTemperature).Temperature;
                if (a is Condition)
                {
                    sliderF.Minimum = 18;
                    sliderF.Maximum = 30;
                }
            }
            if (a is WorkingWithPower)
            {
                label_sliderFunc.Content = "Мощность";
                sliderF.Maximum = (a as WorkingWithPower).MaxPower;
                sliderF.Value = (a as WorkingWithPower).Power;
            }
            if (a is TV)
            {
                SP_forTextBox.Visibility = Visibility.Visible;
                textboxF.IsEnabled = true;
                textboxF.Text = (a as TV).Channel.ToString();
                button_NextChannel.IsEnabled = true;
            }
            else
            {
                if (a is Radio)
                {
                    SP_forSlider2.Visibility = Visibility.Visible;
                    label2_sliderFunc.Content = "Частота";
                    sliderF2.TickFrequency = 0.1;
                    sliderF2.IsEnabled = true;
                    sliderF2.Value = (a as Radio).Frequency;
                }
                else
                {
                    if (a is Fridge)
                    {
                        SP_forButton.Visibility = Visibility.Visible;
                        sliderF.Minimum = -5;
                        sliderF.Maximum = 5;
                        button_Defroste.IsEnabled = true;
                    }
                }
            }
            return a;
        } //выбор элементов управления на пульте
        private void EnableFunctions(Appliance a)
        {
            if (a.Turn == false)
            {
                HideRemoteControl();
            }
            else
            {
                ChooseFunctions(a);
            }
        } 
        private void ListBox_Apps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HideRemoteControl();
            if (Apps.SelectedItem != null)
            {
                EnableFunctions(ChooseAppliance());
                if (ChooseAppliance().Turn == true)
                {
                    turn.Content = "Выключить";
                }
                if (ChooseAppliance().Turn == false)
                {
                    turn.Content = "Включить";
                }
            }
        } //изменение пульта при изменении прибора
        private void Button_NextChannel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                apps = OnWorkWithApplianceEvent( apps, ChooseAppliance(), Area, null, Convert.ToInt32(textboxF.Text));
            }
            catch (InfoInputException ex)
            {
                MessageBox.Show(ex.Message);
            } 
            catch(FormatException)
            {
                MessageBox.Show("Пожалуйста, введите номер канала, на который хотите переключить!");
            }
        }  

        private void button_Defroste_Click(object sender, RoutedEventArgs e)
        {
            if ((ChooseAppliance() as Fridge).Defrosting == false)
            {
                button_Defroste.Content = "Разморозка";
            }
            else
            {
                button_Defroste.Content = "Разморозить";
            }
            apps = OnWorkWithApplianceEvent(apps, ChooseAppliance(), Area, null,null);
            Area.Children.Add(CreateImage(ChooseAppliance()));
        } 
        private void slider_MouseMove(object sender, MouseEventArgs e)
        {
            Slider s = (sender as Slider);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                apps= OnWorkWithApplianceEvent(apps, ChooseAppliance(), Area, s, null);
            }
        }  
    }
}
