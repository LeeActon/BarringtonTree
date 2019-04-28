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

            Console.WriteLine("Event with no dates or place: '{0}'");

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

        public bool SameAs(object obj)
            {
            Event otherEvent = obj as Event;

            if (otherEvent == null)
                return false;

            if (this.Date1 != null)
                {
                if (!this.Date1.SameAs(otherEvent.Date1))
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
                if (!this.Date2.SameAs(otherEvent.Date2))
                    {
                    return false;
                    }
                }
            else if (otherEvent.Date2 != null)
                {
                return false;
                }

            if (!String.IsNullOrEmpty(this.Place))
                {
                if (!this.Place.Equals(otherEvent.Place))
                    {
                    // Place can be messy... ignore it for now
                    //return false;
                    Console.WriteLine("Places don't match ({0}", (Date1 == null) ? "unknown" : this.Date1.ToString());
                    Console.WriteLine(">>>{0}", this.Place);
                    Console.WriteLine(">>>{0}", otherEvent.Place);
                    }
                }
            else if (!String.IsNullOrEmpty(otherEvent.Place))
                {
                // Place can be messy... ignore it for now
                //return false;
                }

            return true;
            }
        }
    }
