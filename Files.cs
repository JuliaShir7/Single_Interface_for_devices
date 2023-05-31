using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KP_OOP
{
    public class Files
    {
        public Files(MainWindow mw) 
        {
            mw.ReturnListOfRoomsEvent += ListOfRooms;
        }
        public Files(Room r)
        {
            r.FillEvent += Filling;
            r.SaveChangesEvent += SaveChanges;
        }

        private ObservableCollection<Appliance> apps = new ObservableCollection<Appliance> { };
        public ObservableCollection<string> ListOfRooms()
        {
            ObservableCollection<string> roomsfiles = new ObservableCollection<string>();
            foreach (string file in Directory.GetFiles("rooms"))
            {
                roomsfiles.Add(System.IO.Path.GetFileNameWithoutExtension(file));
            }
            return roomsfiles;
        }
        public List<string> ReadFile(string path)
        {
            StreamReader reader = new StreamReader(path);
            List<string> list = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                list.Add(line);
            }
            reader.Close();
            return list;
        }
        public ObservableCollection<Appliance> Filling(string path, ObservableCollection<Appliance> apps)
        {
            foreach (string l in ReadFile(path))
            {
                string[] t = l.Split('#');

                if (t[0] == "Телевизор")
                    apps.Add(new TV(ApplianceType.Телевизор, t[1], t[2], t[3], Convert.ToDouble(t[4]), Convert.ToDouble(t[5]), Convert.ToDouble(t[6]), Convert.ToDouble(t[7]), Convert.ToBoolean(t[8]), Convert.ToInt32(t[9]), Convert.ToInt32(t[10]), Convert.ToInt32(t[11])));
                if (t[0] == "Радио")
                    apps.Add(new Radio(ApplianceType.Радио, t[1], t[2], t[3], Convert.ToDouble(t[4]), Convert.ToDouble(t[5]), Convert.ToDouble(t[6]), Convert.ToDouble(t[7]), Convert.ToBoolean(t[8]), Convert.ToInt32(t[9]), Convert.ToDouble(t[10])));
                if (t[0] == "Кондиционер")
                    apps.Add(new Condition(ApplianceType.Кондиционер, t[1], t[2], t[3], Convert.ToDouble(t[4]), Convert.ToDouble(t[5]), Convert.ToDouble(t[6]), Convert.ToDouble(t[7]), Convert.ToBoolean(t[8]), Convert.ToInt32(t[9])));
                if (t[0] == "Холодильник")
                    apps.Add(new Fridge(ApplianceType.Холодильник, t[1], t[2], t[3], Convert.ToDouble(t[4]), Convert.ToDouble(t[5]), Convert.ToDouble(t[6]), Convert.ToDouble(t[7]), Convert.ToBoolean(t[8]), Convert.ToInt32(t[9]), Convert.ToBoolean(t[10])));
                if (t[0] == "Чайник")
                    apps.Add(new Kettle(ApplianceType.Чайник, t[1], t[2], t[3], Convert.ToDouble(t[4]), Convert.ToDouble(t[5]), Convert.ToDouble(t[6]), Convert.ToDouble(t[7]), Convert.ToBoolean(t[8]), Convert.ToInt32(t[9])));
                if (t[0] == "Вентилятор")
                    apps.Add(new Blower(ApplianceType.Вентилятор, t[1], t[2], t[3], Convert.ToDouble(t[4]), Convert.ToDouble(t[5]), Convert.ToDouble(t[6]), Convert.ToDouble(t[7]), Convert.ToBoolean(t[8]), Convert.ToInt32(t[9]), Convert.ToInt32(t[10])));
                if (t[0] == "Пылесос")
                    apps.Add(new VacuumCleaner(ApplianceType.Пылесос, t[1], t[2], t[3], Convert.ToDouble(t[4]), Convert.ToDouble(t[5]), Convert.ToDouble(t[6]), Convert.ToDouble(t[7]), Convert.ToBoolean(t[8]), Convert.ToInt32(t[9]), Convert.ToInt32(t[10])));

            }
            return apps;
        }
        public void SaveChanges(ObservableCollection<Appliance> apps, string path)
        {
            List<string> list = new List<string>();
            foreach (Appliance a in apps)
            {
                string str=a.Type.ToString() + "#" + a.Name + "#" + a.Manufacturer + "#" + a.Img + "#" + a.X + "#" + a.Y + "#" + a.ImgWidth + "#" + a.ImgHeight + "#" + a.Turn + "#";
                if(a is WorkingWithSound)
                    str = str + (a as WorkingWithSound).Loud;
                if (a is WorkingWithTemperature)
                    str = str + (a as WorkingWithTemperature).Temperature;
                if (a is WorkingWithPower)
                    str = str + (a as WorkingWithPower).MaxPower + "#" + (a as WorkingWithPower).Power;
                if (a is TV)
                    str = str + "#" + (a as TV).NumberOfChannels + "#" + (a as TV).Channel;
                else
                {
                    if (a is Radio)
                        str = str + "#" + (a as Radio).Frequency;
                    else
                    {
                        if (a is Fridge)
                            str = str + "#" + (a as Fridge).Defrosting;
                    }
                }
                list.Add(str);
            }
            path = "rooms\\" + path + ".txt";
            StreamWriter writer = new StreamWriter(path);
            foreach(string s in list)
            {
                writer.WriteLine(s);
            }
            writer.Close();
        }
    }
}
