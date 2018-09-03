using System;
using System.Collections;
using System.Collections.Generic;

namespace DateControl.Calendar
{
    public class Mounth
    {
        private uint MounthNumber { get; set; }
        private uint YearNumber { get; set; }

        public string Name { get; set; }
        public string Year { get; set; }
        public IEnumerable Days { get; set; }

        public Mounth(Mounths mounth, uint year)
        {
            Year = year.ToString();
            YearNumber = year;
            MounthNumber = (uint)mounth;
            Name = mounth.ToString();
            Days = GetDays();
        }

        private IEnumerable GetDays()
        {
            int mounthCode = GetMounthCode();
            int centuryCode = GetCenturyCode();
            int yearCode = (centuryCode + Convert.ToInt32(Year.Remove(0, Year.Length - 2)) +
                            (Convert.ToInt32(Year.Remove(0, Year.Length - 2)) / 4)) % 7;
            int date = (yearCode + mounthCode + 1) % 7;
            if (YearNumber % 4 == 0 &&
                (YearNumber % 100 != 0 || YearNumber % 400 == 0))
            {
                date -= 1;
                if (date == -1)
                    date = 6;
            }

            return GetMounthDays(date);
        }

        private IEnumerable GetMounthDays(int date)
        {
            List<int> list = new List<int>();
            int[] arr = new[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int k = (int)MounthNumber % 11;
            int count = arr[k];
            if (count == 28 && YearNumber % 4 == 0 &&
                (YearNumber % 100 != 0 || YearNumber % 400 == 0))
                count = 29;
            int last = date - 2;
            if (last < 0)
                last += 7;
            int lastCount = arr[(MounthNumber - 1) % 11];
            for (int i = lastCount - last + 1; i <= lastCount; i++)
            {
                list.Add(i);
            }

            for (int i = 1; i <= count; ++i)
            {
                list.Add(i);
            }

            return list;
        }

        private int GetCenturyCode()
        {
            int[] k = new[] { 6, 4, 2, 0 };
            int year = Convert.ToInt32(Year.Remove(Year.Length - 2));
            return k[(year - 16) % 4];
        }

        private int GetMounthCode()
        {
            int code = -1;
            switch (MounthNumber)
            {
                case 0:
                    code = 1;
                    break;
                case 1:
                    code = 4;
                    break;
                case 2:
                    code = 4;
                    break;
                case 3:
                    code = 0;
                    break;
                case 4:
                    code = 2;
                    break;
                case 5:
                    code = 5;
                    break;
                case 6:
                    code = 0;
                    break;
                case 7:
                    code = 3;
                    break;
                case 8:
                    code = 6;
                    break;
                case 9:
                    code = 1;
                    break;
                case 10:
                    code = 4;
                    break;
                case 11:
                    code = 6;
                    break;
            }

            return code;
        }
    }
}