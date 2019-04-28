using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace FamilyTree
    {
    public class Name
        {

        private String nameRegex = @"(?<surname>(\bvon\s)?\b[\w\-\']+\b)(\s(?<suffix>Jr.|Sr.|II|III))?$";
        private String explicitNameRegex = @"^=(?<givenNames>.*)\s*/(?<surname>.*)/\s?(?<suffix>.*)";
        //private String nameRegex = @"(?<givenNames>(\-\-\-|.+\b\.?))\s+(?<lastName>(\bvon\s)?\b[\w\-\']+\b)(\s(?<suffix>Jr.|Sr.|II|III))?$";

        public Name()
            {
            }

        public Name(String name)
            {
            bool nameSet = false;
            if (name[0] == '=')
                {
                Regex regex = new Regex(this.explicitNameRegex, RegexOptions.IgnoreCase);
                Match matchName = regex.Match(name);
                if (matchName.Success)
                    {
                    GivenNames = matchName.Groups["givenNames"].Value.Trim();
                    Surname = matchName.Groups["surname"].Value.Trim();
                    Suffix = matchName.Groups["suffix"].Value.Trim();
                    nameSet = true;
                    }
                }
            else
                {
                Regex regex = new Regex(this.nameRegex, RegexOptions.IgnoreCase);
                Match matchName = regex.Match(name);
                if (matchName.Success)
                    {
                    Group surnameGroup = matchName.Groups["surname"];
                    GivenNames = name.Substring(0, surnameGroup.Index).Trim();
                    Surname = surnameGroup.Value.Trim();
                    Suffix = matchName.Groups["suffix"].Value.Trim();
                    nameSet = true;
                    }
                }

            if (!nameSet)
                {
                this.fullName = name;
                }
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
                    if (!String.IsNullOrEmpty(Prefix))
                        {
                        builder.Append(Prefix);
                        insertSpace = true;
                        }
                    if (!String.IsNullOrEmpty(GivenNames))
                        {
                        if (insertSpace)
                            {
                            builder.Append(' ');
                            }
                        builder.Append(GivenNames);
                        insertSpace = true;
                        }
                    if (!String.IsNullOrEmpty(Surname))
                        {
                        if (insertSpace)
                            {
                            builder.Append(' ');
                            }
                        builder.Append(Surname);
                        insertSpace = true;
                        }
                    if (!String.IsNullOrEmpty(Suffix))
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

#if true // TODO: Remove after we know all the suffixes
        public static List<String> Suffixes = new List<String>();
        private static void AddSuffix(String suffix)
            {
            if (!Suffixes.Contains(suffix))
                {
                Suffixes.Add(suffix);
                }
            }
#endif

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
                    AddSuffix(name.Suffix); //TODO: Remove
                    }
                }

            return name;
            }
        public bool SameAs(object obj)
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
