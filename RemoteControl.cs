using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace KP_OOP
{
    class RemoteControl
    {
        
        //private MediaPlayer player=new MediaPlayer(); 
        public RemoteControl(Room r)
        {
            //Room r = new Room();
            r.TurnEvent += TurnApp;
            r.WorkWithApplianceEvent += WorkWithAppliance;
        }
       
        private ObservableCollection<Appliance> TurnApp(Appliance a, ObservableCollection<Appliance> apps, Canvas c)
        {
            DeleteApplianceImage(a, c);
            if (a.Turn == false)
            {
                a.Img = "Resources\\on" + a.Img.Substring(13);
                a.Turn = true;
            }
            else
            {
                a.Turn = false;
                a.Img = "Resources\\off" + a.Img.Substring(12);
            }
            a.Turn = a.Turn;
            a.Img = a.Img;
            return apps;
        }
        private void DeleteApplianceImage(Appliance a, Canvas c)
        {
            var images = c.Children.OfType<Image>().ToList();
            string[] str = a.Name.Split(' ');
            string name = null;
            foreach (string s in str)
            {
                name = name + s;
            }
            foreach (Image i in images)
            {
                if (i.Name == name)
                {
                    c.Children.Remove(i);
                }
            }
        }
       
        private ObservableCollection<Appliance> WorkWithAppliance(ObservableCollection<Appliance> apps, Appliance a, Canvas c, Slider slider, int? num)
        {
            if(slider!=null)
            {
                if (slider.Name == "sliderF")
                {
                    if (a is WorkingWithTemperature || a is WorkingWithSound)
                    {
                        AnimateIcons(c, a, Convert.ToInt32(slider.Value));
                    }
                    if (a is WorkingWithPower)
                    {
                        AnimateValueChanges(c, a, Convert.ToInt32(slider.Value));
                        (a as WorkingWithPower).Power = Convert.ToInt32(slider.Value);
                    }
                }
                if (slider.Name == "sliderF2")
                {
                    AnimateValueChanges(c, a, Convert.ToInt32(slider.Value));
                    (a as Radio).Frequency = Convert.ToDouble(slider.Value);
                }
            }
            else
            {
                if (num != null)
                {
                    if (num > (a as TV).NumberOfChannels)
                    {
                        throw new InfoInputException("Данный канал не настроен на вашем устройстве!");
                    }
                    else
                    {
                        AnimateValueChanges(c, a, (int)num);
                        (a as TV).Channel = (int)num;
                    }
                }
                else
                {
                    if ((a as Fridge).Defrosting == false)
                    {
                        a.Img = "Resources\\Defrosting.jpg";
                        (a as Fridge).Defrosting = true;
                    }
                    else
                    {
                        (a as Fridge).Defrosting = false;
                        a.Img = "Resources\\onFridge.jpg";
                    }
                    (a as Fridge).Defrosting = (a as Fridge).Defrosting;
                    (a as Fridge).Img = (a as Fridge).Img;   
                }
            }
            return apps;
        } 
        private void AnimateValueChanges(Canvas c, Appliance a, int num)
        {
            TextBlock tb = new TextBlock();
            Canvas.SetLeft(tb, a.X + 5);
            Canvas.SetTop(tb, a.Y + 5);
            tb.Width = 45; tb.Height = 20; tb.Name = "tb";
            tb.FontSize = 10; tb.TextAlignment = TextAlignment.Center;
            tb.FontWeight = FontWeights.Bold;
            tb.Foreground = Brushes.White;
            tb.Background = Brushes.Black;
            tb.Text = ">> " + num.ToString();
            c.Children.Add(tb);
            var myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            myDoubleAnimation.AutoReverse = true;
            tb.BeginAnimation(TextBlock.OpacityProperty, myDoubleAnimation);
        }
        private void AnimateIcons(Canvas c, Appliance a, int value)
        {
            Image img = new Image();
            if (a is WorkingWithTemperature)
            {
                img.Source = new BitmapImage(new Uri("Resources\\snow.jpg", UriKind.Relative));
                (a as WorkingWithTemperature).Temperature = value;
            }
            if (a is WorkingWithSound)
            {
                img.Source = new BitmapImage(new Uri("Resources\\SoundIcon.jpg", UriKind.Relative));
                (a as WorkingWithSound).Loud = value;
            }
            img.Name = "icon";
            img.Width = 20; img.Height = 20;
            img.Stretch = Stretch.Fill;
            Canvas.SetLeft(img, a.X+5);
            Canvas.SetTop(img, a.Y+5);
            c.Children.Add(img);
            var myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 0.0;
            myDoubleAnimation.To = 1.0;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            myDoubleAnimation.AutoReverse = true;
            img.BeginAnimation(Image.OpacityProperty, myDoubleAnimation);
        }
        
    } 
    public class InfoInputException : Exception
    {
        public InfoInputException() { }
        public InfoInputException(string message) : base(message) { }
        public InfoInputException(string message, Exception inner) : base(message, inner) { }
        public InfoInputException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
   
}
