using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree
    {
    public class Name
        {

        public Name()
            {
            }

        private String fullName = null;
        public String Fullname
            {
            get
                {
                if (this.fullName == null)
                    {
                    StringBuilder builder = new StringBuilder();
                    bool insertSpace = false;
                    if (Prefix != null)
                        {
                        builder.Append(Prefix);
                        insertSpace = true;
                        }
                    if (GivenNames != null)
                        {
                        if (insertSpace)
                            {
                            builder.Append(' ');
                            }
                        builder.Append(GivenNames);
                        insertSpace = true;
                        }
                    if (Surname != null)
                        {
                        if (insertSpace)
                            {
                            builder.Append(' ');
                            }
                        builder.Append(Surname);
                        insertSpace = true;
                        }
                    if (Suffix != null)
                        {
                        if (insertSpace)
                            {
                            builder.Append(' ');
                            }
                        builder.Append(Suffix);
                        insertSpace = true;
                        }
                    this.fullName = builder.ToString();
                    }

                return this.fullName;
                }
            }
        public String GEDCOMname
            {
            get
                {
                StringBuilder builder = new StringBuilder();
                bool insertSpace = false;
                if (Prefix != null)
                    {
                    builder.Append(Prefix);
                    insertSpace = true;
                    }
                if (GivenNames != null)
                    {
                    if (insertSpace)
                        {
                        builder.Append(' ');
                        }
                    builder.Append(GivenNames);
                    insertSpace = true;
                    }
                if (Surname != null)
                    {
                    if (insertSpace)
                        {
                        builder.Append(' ');
                        }
                    builder.Append('/');
                    builder.Append(Surname);
                    builder.Append('/');
                    insertSpace = true;
                    }
                if (Suffix != null)
                    {
                    if (insertSpace)
                        {
                        builder.Append(' ');
                        }
                    builder.Append(Suffix);
                    insertSpace = true;
                    }
                return builder.ToString();
                }
            }

        public String Surname { get; set; }
        public String GivenNames { get; set; }
        public String Prefix { get; set; }
        public String Suffix { get; set; }

        public String Canonical
            {
            get
                {
                return Fullname.ToUpper();
                }
            }
#if false
        public static String Unflip(String name)
            {
            String strippedName = name.StripCommentsAndExtraWhitespace();
            String[] nameParts = strippedName.Split(',');

            if (nameParts.Length == 1)
                {
                return name.Trim();
                }

            String newName = nameParts[1].Trim() + " " + nameParts[0].Trim();
            if ((nameParts.Length == 3) && !String.IsNullOrWhiteSpace(nameParts[2]))
                {
                newName = newName + " " + nameParts[2];
                }

            return newName.ToUpper();
            }
#endif
        public static Name ParseSurnameFirst(String flippedName)
            {
            String[] nameParts = flippedName.StripCommentsAndExtraWhitespace().Split(',');
            Name name = new Name();

            if (nameParts.Length == 1)
                {
                name.Surname = nameParts[0].Trim();
                }
            else
                {
                name.Surname = nameParts[0].Trim();
                name.GivenNames = nameParts[1].Trim();

                if ((nameParts.Length == 3) && !String.IsNullOrWhiteSpace(nameParts[2]))
                    {
                    name.Suffix = nameParts[2].Trim();
                    }
                }

            return name;
            }
        public override bool Equals(object obj)
            {
            Name otherName = obj as Name;

            if (otherName == null)
                {
                return false;
                }

            if (this.Canonical != otherName.Canonical)
                {
                return false;
                }

            return true;
            }
        }
    }
