using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace click_gas.Models
{
    public class Time
    {
        public Time(string time)
        {
            string h = new string(time.Take(2).ToArray());
            horas = int.Parse(h);
  
            string m = time.Substring(time.Length-2);
            minutos = int.Parse(m);
        }

        public Time(double time)
        {
            horas = 0;

            float minTotal = (float) time * 60;
            while (minTotal >= 60)
            {
                horas++;
                minTotal -= 60;
            }
            minutos = (int) minTotal;
        }

        public int horas { get; set; }

        public int minutos { get; set; }

        public float GetTimeFloat()
        {
            return (horas + ((float) minutos / 60));
        }

        public string GetTimeString()
        {
            return horas + ":" + minutos;
        }
    }
}