using System;

namespace FamilyTree
    {
    public class Person
        {
        public Person(Name name, String text)
            {
            this.Name = name;
            Text = text;
            }

        public Name Name { get; set; }

        private static int idCur = 1;
        private String id = null;
        public String Id
            {
            get
                {
                if (this.id == null)
                    {
                    this.id = String.Format("P{0}", idCur++);
                    }

                return this.id;
                }

            }

        public Family Family { get; set; }
        public Person Father
            {
            get
                {
                if (Family == null)
                    return null;

                return Family.Husband;
                }
            }

        public Person Mother
            {
            get
                {
                if (Family == null)
                    return null;

                return Family.Wife;
                }
            }

        public Event Birth { get; set; }
        public Event Death { get; set; }

        public String Text { get; set; }

        public enum SexEnum { Male, Female };
            
        public SexEnum? Sex { get; set; }

        public bool SameAs(object obj)
            {
            Person otherPerson = obj as Person;

            if (otherPerson == null)
                {
                return false;
                }

            // Must have at least one of these to compare equal.
            if ((otherPerson.Family == null) && (otherPerson.Birth == null) && (otherPerson.Death == null))
                {
                return false;
                }

            // Must have at least one of these to compare equal.
            if ((this.Birth != null) && (!this.Birth.SameAs(otherPerson.Birth)))
                {
                return false;
                }

            if (this.Sex.HasValue && otherPerson.Sex.HasValue)
                {
                // Only compare Sex if both specify it. Not specifying is a wildcard.
                if (this.Sex != otherPerson.Sex)
                    {
                    return false;
                    }
                }

            if (!this.Name.SameAs(otherPerson.Name))
                {
                return false;
                }

            if ((this.Family != null) && (this.Family != otherPerson.Family))
                {
                return false;
                }

            // If there isn't anything more than name that matches, we can't say they are equal
            if ((this.Family == null) && (this.Birth == null) && (this.Death == null))
                {
                return false;
                }

            if ((this.Death != null) && (!this.Death.SameAs(otherPerson.Death)))
                {
                return false;
                }

            return true;
            }
        }
    }
