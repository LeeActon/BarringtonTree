using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree
    {
    public class Date
        {
        String[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        String[] shortMonths = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEPT", "OCT", "NOV", "DEC" };

        public int? Day { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }

        public bool Julian { get; set; }

        public Date(String day, String month, String year, bool julian)
            {
            if (!String.IsNullOrEmpty(day))
                {
                Day = int.Parse(day);
                }
            if (!String.IsNullOrEmpty(month))
                {
                for (int i = 0; i < this.months.Length; i++)
                    {
                    if (month == this.months[i])
                        {
                        Month = i;
                        }
                    }
                if (!Month.HasValue)
                    {
                    Console.WriteLine("Bad month: {0}", month);
                    }
                }
            if (!String.IsNullOrEmpty(year))
                {
                Year = int.Parse(year);
                }

            Julian = julian;
            }
        public override string ToString()
            {
            StringBuilder builder = new StringBuilder();

            bool insertSpace = true;
            if (Day.HasValue)
                {
                builder.Append(Day.Value.ToString());
                insertSpace = true;
                }

            if (Month.HasValue)
                {
                if (insertSpace)
                    {
                    builder.Append(' ');
                    }
                builder.Append(this.shortMonths[Month.Value]);
                }

            if (Year.HasValue)
                {
                if (insertSpace)
                    {
                    builder.Append(' ');
                    }
                builder.Append(Year.Value.ToString());
                }

            return builder.ToString();
            }

        public override bool Equals(object obj)
            {
            Date otherDate = obj as Date;
            if (otherDate == null)
                {
                return false;
                }

            if (this.Day != otherDate.Day)
                {
                return false;
                }
            if (this.Month != otherDate.Month)
                {
                return false;
                }
            if (this.Year != otherDate.Year)
                {

                return false;
                }
            if (this.Julian != otherDate.Julian)
                {
                return false;
                }

            return true;
            }
        }
    }
