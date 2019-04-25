using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree
    {
    public class Event
        {
        public Date Date1 { get; set; }
        public Date Date2 { get; set; }

        public String Place { get; set; }

        public Event(Date date1, Date date2, String place)
            {
            if ((date1 != null) && date1.Julian)
                {
                // Always put the Julian date second
                Date1 = date2;
                Date2 = date1;
                }
            else
                {
                Date1 = date1;
                Date2 = date2;
                }
            Place = place;
            }

        public static Event Create(Date date1, Date date2, String place)
            {
            if (date1 != null || date2 != null || place != null)
                return new Event(date1, date2, place);

            return null;
            }

        public String DateString()
            {
            String dateString = null;

            if (Date1 != null)
                {
                if ((Date2 != null) && !Date2.Julian)
                    {
                    dateString = String.Format("BET {0} AND {1}", Date1.ToString(), Date2.ToString());
                    }
                else
                    {
                    dateString = Date1.ToString();
                    }
                }

            return dateString;
            }

        public override bool Equals(object obj)
            {
            Event otherEvent = obj as Event;

            if (otherEvent == null)
                return false;

            if (this.Date1 != null)
                {
                if (!this.Date1.Equals(otherEvent.Date1))
                    {
                    return false;
                    }
                }
            else if (otherEvent.Date1 != null)
                {
                return false;
                }

            if (this.Date2 != null)
                {
                if (!this.Date2.Equals(otherEvent.Date2))
                    {
                    return false;
                    }
                }
            else if (otherEvent.Date2 != null)
                {
                return false;
                }

            if (this.Place != null)
                {
                if (!this.Place.Equals(otherEvent.Place))
                    {
                    return false;
                    }
                }
            else if (otherEvent.Place != null)
                {
                return false;
                }

            return true;
            }
        }
    }
