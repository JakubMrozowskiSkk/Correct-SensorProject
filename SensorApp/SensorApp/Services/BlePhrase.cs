using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Modeles;
namespace SensorApp.Servis
{
    public class BlePhrase : IBlePhrase
    {
        public IEnumerable<byte> StringToByteList(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToList();
        }
        public IEnumerable<BleDeviceModel> PhraseBlue(string com)
        {

            List<BleDeviceModel> ListOfBlue = new List<BleDeviceModel>();
            string[] phrase = com.Split(' ', '\t','\n');
            int size = phrase.Length;
            string mac = "";
            int DBM = 0;
            IEnumerable<byte> BL;
            bool y = true;
            for (int i = 0; i < size; i++)
            {

                if (phrase[i] == "(update):")
                {
                    mac = phrase[i + 1];
                    y = false;
                }
                if (phrase[i] == "(new):")
                {
                    mac = phrase[i + 1];
                    y = true;

                }
                if (phrase[i] == "dBm")
                {
                    DBM = Convert.ToInt32(phrase[i - 1]);

                }
                if (phrase[i] == "Manufacturer:")
                {
                    if (y)
                    {
                        string input = phrase[i + 1].ToString();
                        int input_size = input.Length;
                        input = input.Substring(1, input_size - 3);
                        BL = StringToByteList(input);
                        BleDeviceModel tmp = new BleDeviceModel(mac, DBM, BL);
                        ListOfBlue.Add(tmp);
                    }
                    else
                    {
                        ListOfBlue[ListOfBlue.FindIndex(x => x.Mac == mac)].AddToRSSI(DBM);
                    }

                }
            }
            return ListOfBlue;
            /*Działa
            int mac = 8;
            int id = 10;
            int nb = 14;
            int x = 12;
            string[] phrase = com.Split(' ');
            int size = phrase.Length;
            size = (size - nb) / x;
            for(int i=0; i < size; i++)
            {
                Sensor tmp = new Sensor(phrase[mac + i * x], phrase[id + i * x], phrase[nb + i * x]);
                ListOfBlue.Add(tmp);
            }
            */
        }

        
    }
}

