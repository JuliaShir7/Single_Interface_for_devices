using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KP_OOP
{
    public enum ApplianceType { Чайник, Телевизор, Радио, Холодильник, Кондиционер, Пылесос, Вентилятор };
    public abstract class Appliance : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private string name, manufacturer, img;
        private ApplianceType type;
        private bool turn;
        private double x, y, imgW, imgH;
        
        public ApplianceType Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Manufacturer
        {
            get { return manufacturer; }
            set
            {
                manufacturer = value;
                OnPropertyChanged("Manufacturer");
            }
        }
        public string Img
        {
            get { return img; }
            set
            {
                img = value;
                OnPropertyChanged("Img");
            }
        }
        public double X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged("X");
            }
        }
        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged("Y");
            }
        }
        public double ImgWidth
        {
            get { return imgW; }
            set
            {
                imgW = value;
                OnPropertyChanged("ImgWidth");
            }
        }
        public double ImgHeight
        {
            get { return imgH; }
            set
            {
                imgH = value;
                OnPropertyChanged("ImgHeight");
            }
        }
        public bool Turn
        {
            get { return turn; }
            set { turn = value; }
        }
        public Appliance(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t)
        {
            Type = type;
            Name = n;
            Manufacturer = m;
            Img = img;
            X = x;
            Y = y;
            ImgWidth = imgW;
            ImgHeight = imgH;
            turn = t; 
        }
        public override string ToString()
        {
            return Name;
        }
    }

    public abstract class WorkingWithSound : Appliance
    {
        private int loud;
        public int Loud
        {
            get { return loud; }
            set
            {
                loud = value;
                OnPropertyChanged("Loud");
            }
        }
        public WorkingWithSound(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int loud) : base(type, n, m, img, x, y, imgW, imgH, t)
        {
            Loud = loud;
        }
    }
    public abstract class WorkingWithTemperature : Appliance
    {
        private int temperature;
        public int Temperature 
        {
            get { return temperature; }
            set
            {
                temperature = value;
                OnPropertyChanged("Temperature");
            }
        }
        public WorkingWithTemperature(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int temperature) : base(type, n, m, img, x, y, imgW, imgH, t)
        {
            Temperature = temperature;
        }
    }
    public abstract class WorkingWithPower : Appliance
    {
        private int maxPower;
        public int MaxPower
        {
            get { return maxPower; }
            set
            {
                maxPower = value;
                OnPropertyChanged("MaxPower");
            }
        }
        private int power;
        public int Power
        {
            get { return power; }
            set
            {
                power = value;
                OnPropertyChanged("Power");
            }
        }
        public WorkingWithPower(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int maxPower, int power) : base(type, n, m, img, x, y, imgW, imgH, t)
        {
            MaxPower = maxPower;
            Power = power;
        }
    }
    public class TV : WorkingWithSound
    {
        private int channel;
        private int numOfChannels;
        public int Channel
        {
            get { return channel; }
            set
            {
                channel = value;
                OnPropertyChanged("Channel");
            }
        }
        public int NumberOfChannels
        {
            get { return numOfChannels; }
            set
            {
                numOfChannels = value;
                OnPropertyChanged("NumberOfChannels");
            }
        }

        public TV(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int loud, int numOfChannels, int channel) : base(type, n, m, img, x, y, imgW, imgH, t, loud)
        {
            Channel = channel;
            NumberOfChannels = numOfChannels;
        }
    }
    public class Radio : WorkingWithSound
    {
        private double frequency;
        public double Frequency
        {
            get { return frequency; }
            set
            {
                frequency = value;
                OnPropertyChanged("Frequency");
            }
        }
        public Radio(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int loud, double frequency) : base(type, n, m, img, x, y, imgW, imgH, t, loud)
        {
            Frequency = frequency;
        }
    }
    public class Fridge : WorkingWithTemperature
    {
        public bool Defrosting { get; set; }
        public Fridge(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int temperature, bool defrosting) : base(type, n, m, img, x, y, imgW, imgH, t, temperature)
        {
            Defrosting = defrosting;
        }
    }
    public class Condition : WorkingWithTemperature
    {
        public Condition(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int temperature) : base(type, n, m, img, x, y, imgW, imgH, t, temperature) { }
    }
    public class Kettle : WorkingWithTemperature
    {
        public Kettle(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int temperature) : base(type, n, m, img, x, y, imgW, imgH, t, temperature) { }
    }
    public class VacuumCleaner : WorkingWithPower
    {
        public VacuumCleaner(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int maxPower, int power) : base(type, n, m, img, x, y, imgW, imgH, t, maxPower, power) { }
    }
    public class Blower : WorkingWithPower
    {
        public Blower(ApplianceType type, string n, string m, string img, double x, double y, double imgW, double imgH, bool t, int maxPower, int power) : base(type, n, m, img, x, y, imgW, imgH, t, maxPower, power) { }
    }
}
