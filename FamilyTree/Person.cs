using System;

namespace FamilyTree
    {
    public class Person
        {
        public Person(Name name, String text, SexEnum sex)
            {
            this.Name = name;
            Text = text;
            Sex = sex;
            }

        public Name Name { get; set; }
        public String Id { get; set; }

        public Person Father { get; set; }
        public Person Mother { get; set; }

        public Event Birth { get; set; }
        public Event Death { get; set; }

        public String Text { get; set; }

        public enum SexEnum { Male, Female };
            
        public SexEnum Sex { get; set; }

        public override bool Equals(object obj)
            {
            Person otherPerson = obj as Person;

            if (otherPerson == null)
                {
                return false;
                }

            if (this.Sex != otherPerson.Sex)
                {
                return false;
                }

            if (!this.Name.Equals(otherPerson.Name))
                {
                return false;
                }

            if ((this.Birth != null) && (!this.Birth.Equals(otherPerson.Birth)))
                {
                return false;
                }

            if ((this.Death != null) && (!this.Death.Equals(otherPerson.Death)))
                {
                return false;
                }

            return true;
            }
        }
    }
