using FamilyTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BarringtonFile
    {
    public class Importer
        {
        private Dictionary<String, List<Family>> families;
        private Dictionary<String, Person> people;

        public Importer()
            {
            this.families = new Dictionary<String, List<Family>>();
            this.people = new Dictionary<String, Person>();
            }

        const String startOfFamilyMarker = "***";
        const String endOfFamilyMarker = "###";
        private StreamReader reader = null;
        private int lineNumber = 0;

        String datePlaceRegEx =
            @"\s(probably\s)?(a twin\s)?(in\s)?(or about\s)?"
                + @"(?<ireland>Ireland)?"  // Special case "born in Ireland
                + @"(?<day1>\d{1,4})?\s*"
                + @"(?<month1a>[A-Za-z]{3,9})?\s*"
                + @"(,?\s*(or)?\s+(?<month1b>[A-Za-z]{3,9}))?\s*"
                + @"(,?\s*(or)?\s+(?<month1c>[A-Za-z]{3,9}))?\s*"
                + @"(?<year1>\d{4})?\s*"
                + @"(?<julian1>\(Old Style or Julian\))?\s*"
                + @"(\(New Style or Gregorian\))?\s*"
                + @"(\(according to[^\)]*\))?\s*"
                + @"(\(depending [^\)]*\))?\s*"
                + @"(or\s)?"
                + @"(?<day2>\d{1,4})?\s*"
                + @"(?<month2>[A-Za-z]{3,9})?\s*"
                + @"(?<year2>\d{4})?\s*"
                + @"(?<julian2>\(Old Style or Julian\))?\s*"
                + @"(\(New Style or Gregorian\))?\s*"
                + @"(\(according to[^\)]*\))?\s*"
                + @"(\(depending [^\)]*\))?\s*"
                + @"(probably\s*)"
                + @"(at\s)?(or near\s)?(?<place>[^;]*);?"/**/
            ;

        private int idPerson = 1;
        private Person NewPerson(Name name, String text, Person.SexEnum sex)
            {
            Person person = new Person(name, text, sex);

            person.Id = String.Format("P{0}", this.idPerson++);

            this.people[person.Id] = person;

            person.Birth = ParseEvent("born", text);
            person.Death = ParseEvent("died", text);

            return person;
            }

#if false
        private String ParseName(String canonicalName, String lastName, String text)
            {

            }
#endif

        private Event ParseEvent(String tag, String text)
            {
            Regex regex = new Regex(tag + this.datePlaceRegEx);
            Event newEvent = null;

            Match match = regex.Match(text);
            if (match.Success)
                {
                Log("match: ({0}-{1}) /{2}/", true, true, match.Index, match.Length, match.Value);
                StringBuilder builder = new StringBuilder();
                Group day1 = match.Groups["day1"];
                Group month1a = match.Groups["month1a"];
                Group month1b = match.Groups["month1b"];
                Group month1c = match.Groups["month1c"];
                Group year1 = match.Groups["year1"];
                Group day2 = match.Groups["day2"];
                Group month2 = match.Groups["month2"];
                Group year2 = match.Groups["year2"];
                Group placeGroup = match.Groups["place"];
                Group ireland = match.Groups["ireland"];
                if (day1.Success)
                    {
                    Log("    day1 : {0}", true, true, day1.Value);
                    }
                if (month1a.Success)
                    {
                    Log("    month1a : {0}", true, true, month1a.Value);
                    }
                if (month1b.Success)
                    {
                    Log("    month1b : {0}", true, true, month1b.Value);
                    }
                if (month1c.Success)
                    {
                    Log("    month1c : {0}", true, true, month1c.Value);
                    }
                if (year1.Success)
                    {
                    Log("    year1 : {0}", true, true, year1.Value);
                    }
                if (day2.Success)
                    {
                    Log("   day2 : {0}", true, true,day2.Value);
                    }
                if (month2.Success)
                    {
                    Log("    month2 : {0}", true, true, month2.Value);
                    }
                if (year2.Success)
                    {
                    Log("    year2 : {0}", true, true, year2.Value);
                    }
                if (placeGroup.Success)
                    {
                    Log("    place : {0}", true, true, placeGroup.Value);
                    }

                Date date1 = null;
                Date date2 = null;
                String place = null;
                if (ireland.Success)
                    {
                    place = "ireland";
                    }
                else
                    {
                    bool julian1 = match.Groups["julian1"].Success;
                    bool julian2 = match.Groups["julian2"].Success;
                    if (day1.Success || month1a.Success || year1.Success)
                        {
                        if (day1.Length > 2)
                            {
                            // day1 is actually the year
                            date1 = new Date(null, null, day1.Value, julian1);
                            }
                        else
                            {
                            date1 = new Date(day1.Value, month1a.Value, year1.Value, julian1);
                            }
                        }
                    if (day2.Success || month2.Success || year2.Success)
                        {
                        if (day2.Length > 2)
                            {
                            // day2 is actually the year
                            date2 = new Date(null, null, day2.Value, julian2);
                            }
                        else
                            {
                            date2 = new Date(day2.Value, month2.Value, year2.Value, julian2);
                            }
                        }
                    place = placeGroup.Value;
                    }

                newEvent = Event.Create(date1, date2, place);
                }

            return newEvent;
            }

        private String ReadLine()
            {
            while (true)
                {
                String line = this.reader.ReadLine();
                this.lineNumber++;
                if (line.StartsWith("+"))
                    {
                    // Put a "+" in front of a line and set a breakpoint here to see how the line is processed.
                    line = line.Substring(1);
                    }
                if (!line.StartsWith("// "))
                    {
                    return line;
                    }
                }


            }
        private class LogEntry
            {
            public String Format;
            public bool Enabled;
            public bool LineNumber;
            }
        private enum LogEntryNumber
            {
            DuplicateFamily = 0,
            MissingHusband,
            MissingWife,
            InvalidChild,
            NoParents,
            ParentsNotFound,
            ExpectedParentsNotFound,
            }
        private LogEntry[] logEntries =
            {
            new LogEntry() {Format = "Duplicate family: {0}", Enabled = true, LineNumber = true },
            new LogEntry() {Format = "Missing husband data ({0}).", Enabled = true, LineNumber = true },
            new LogEntry() {Format = "Missing wife data ({0}).", Enabled = true, LineNumber = true },
            new LogEntry() {Format = "Invalid child line: {0}", Enabled = true, LineNumber = true },
            new LogEntry() {Format = "No parents for: {0}", Enabled = false, LineNumber = false },
            new LogEntry() {Format = "Can't find parents ({0}) for {1}", Enabled = false, LineNumber = false },
            new LogEntry() {Format = "Can't find expected parents ({0}) for {1}", Enabled = true, LineNumber = false },
            };
        private void Log(LogEntryNumber logNumber, params object [] args)
            {
            LogEntry logEntry = this.logEntries[(int) logNumber];

            Log(logEntry.Format, logEntry.Enabled, logEntry.LineNumber, args);
            }

        private void Log(String format, bool enabled, bool lineNumber, params object [] args)
            {
            if (enabled)
                {
                if (lineNumber)
                    {
                    Console.Write("{0} - ", this.lineNumber);
                    }
                Console.WriteLine(format, args);
                }
            }

        private void ReadFamily(String firstLine)
            {
            String[] names = firstLine.Substring(startOfFamilyMarker.Length + 1).ToUpper().Split('/');
            Name husbandName = Name.ParseSurnameFirst(names[0].Trim());
            Name wifeName = Name.ParseSurnameFirst(names[1].Trim());
            String key = husbandName.Canonical + " AND " + wifeName.Canonical;

            Family family = new Family();
            List<Family> existingFamilies;
            if (this.families.TryGetValue(key, out existingFamilies))
                {
                Log(LogEntryNumber.DuplicateFamily, key);
                }
            else
                {
                existingFamilies = new List<Family>();
                this.families[key] = existingFamilies;
                }
            existingFamilies.Add(family);

            String husbandLine = ReadLine();
            if (husbandLine == endOfFamilyMarker)
                {
                Log(LogEntryNumber.MissingHusband, key);
                return;
                }

            family.Husband = NewPerson(husbandName, husbandLine, Person.SexEnum.Male);

            String wifeLine = ReadLine();
            if (wifeLine == endOfFamilyMarker)
                {
                Log(LogEntryNumber.MissingWife, key);
                return;
                }

            if (wifeLine.StartsWith("(1)"))
                {
                Log(LogEntryNumber.MissingWife, key);
                family.ChildrenLines.Add(wifeLine);
                }
            else
                {
                family.Wife = NewPerson(wifeName, wifeLine, Person.SexEnum.Female);
                }

            while (!this.reader.EndOfStream)
                {
                String line = ReadLine();
                if (line == endOfFamilyMarker)
                    return;

                if (!line.StartsWith("("))
                    {
                    Log(LogEntryNumber.InvalidChild, line);
                    }
                family.ChildrenLines.Add(line);
                }
            }
        public void Load(String path)
            {
            using (FileStream streamIn = File.OpenRead(path))
                {
                using (this.reader = new StreamReader(streamIn))
                    {
                    while (!this.reader.EndOfStream)
                        {
                        String line = ReadLine();

                        if (line.StartsWith(startOfFamilyMarker))
                            {
                            if (line[startOfFamilyMarker.Length] == '+')
                                {
                                throw new InvalidOperationException();
                                }
                            ReadFamily(line);
                            }
                        }

#if false
                    foreach (KeyValuePair<String, List<Family>> pair in this.families)
                        {
                        foreach(Family family in pair.Value)
                            {
                            ConnectParents(family);
                            }
                        }
#else
                    foreach (KeyValuePair<String,Person> pair in this.people)
                        {
                        ConnectParents(pair.Value);
                        }
#endif
                    }
                }
            }

#if false
        private void ConnectParents(Family family)
            {
                
                ConnectParents(family.Husband);
                ConnectParents(family.Wife);
            }
#endif

        const String sonTo = "son to ";
        const String daughterTo = "daughter to ";
        const String toWhomRefer = ", (to whom refer)";
        private void ConnectParents(Person person)
            {
            String parentRef = (person.Sex == Person.SexEnum.Male) ? sonTo : daughterTo;
            int iParentRef = person.Text.IndexOf(parentRef);
            if (iParentRef != -1)
                {
                int iParentsStart = iParentRef + parentRef.Length;
                int iParentsEnd = person.Text.IndexOf(toWhomRefer, iParentsStart);
                bool expectParents = true;
                if (iParentsEnd == -1)
                    {
                    iParentsEnd = person.Text.Length - 1;
                    expectParents = false;
                    }

                int iSemiColon = person.Text.IndexOf(';', iParentsStart);
                if (iSemiColon != -1)
                    {
                    if (iSemiColon < iParentsEnd)
                        {
                        iParentsEnd = iSemiColon;
                        expectParents = false;
                        }
                    }

                String parentNames = person.Text.Substring(iParentsStart, iParentsEnd - iParentsStart)
                                           .StripCommentsAndExtraWhitespace(true);
                List<Family> parentFamilies;
                if (this.families.TryGetValue(parentNames.ToUpper(), out parentFamilies))
                    {
                    if (parentFamilies.Count == 1)
                        {
                        Family parentFamily = parentFamilies[0];
                        parentFamily.Children.Add(person);
                        person.Father = parentFamily.Husband;
                        person.Mother = parentFamily.Wife;
                        }
                    }
                else
                    {
                    if (expectParents)
                        {
                        Log(LogEntryNumber.ExpectedParentsNotFound, parentNames, person.Name.Fullname);
                        }
                    else
                        {
                        Log(LogEntryNumber.ParentsNotFound, parentNames, person.Name.Fullname);
                        }
                    }
                }
            else
                {
                Log(LogEntryNumber.NoParents, person.Name.Fullname);
                }
            }

        private void ParseChildData(Family family, String data)
            {
            }

        private String FlipName(String name)
            {
            String[] nameParts = name.ToUpper().Split(' ');
            int iLastName = nameParts.Length - 1;

            String lastName = nameParts[iLastName];

            StringBuilder builder = new StringBuilder();

            builder.Append(lastName);
            builder.Append(',');
            for (int i = 0; i < iLastName; i++)
                {
                builder.Append(' ');
                builder.Append(nameParts[i]);
                }

            return builder.ToString();
            }

        public void ExportGEDCOM(String fileName)
            {
            using (FileStream stream = File.OpenWrite(fileName))
                {
                using (StreamWriter writer = new StreamWriter(stream))
                    {
                    writer.WriteLine("0 HEAD");
                    writer.WriteLine("1 CHAR UTF-8");
                    writer.WriteLine(@"1 SOUR http://freepages.rootsweb.com/~database/misc/BARRINGTON.htmAncestry.com");
                    writer.WriteLine("2 VERS (2010.3)");
                    writer.WriteLine("2 NAME The Barrington Family Association");
                    writer.WriteLine("1 GEDC");
                    writer.WriteLine("2 VERS 5.5");
                    writer.WriteLine("2 FORM LINEAGE-LINKED");

                    foreach (KeyValuePair<String, Person> pair in this.people)
                        {
                        Person person = pair.Value;

                        writer.WriteLine("0 @{0}@ INDI", person.Id);
                        writer.WriteLine("1 NAME {0}", person.Name.GEDCOMname);
                        switch (person.Sex)
                            {
                            case Person.SexEnum.Male:
                                writer.WriteLine("1 SEX M");
                                break;
                            case Person.SexEnum.Female:
                                writer.WriteLine("1 SEX F");
                                break;

                            }
                        if (person.Birth != null)
                            {
                            writer.WriteLine("1 BIRT");
                            String dateString = person.Birth.DateString();
                            if (!String.IsNullOrEmpty(dateString))
                                {
                                writer.WriteLine("2 DATE {0}", dateString);
                                }
                            if (!String.IsNullOrEmpty(person.Birth.Place))
                                {
                                writer.WriteLine("2 PLAC {0}", person.Birth.Place);
                                }
                            }
                        if (person.Death != null)
                            {
                            writer.WriteLine("1 DEAT");
                            String dateString = person.Death.DateString();
                            if (!String.IsNullOrEmpty(dateString))
                                {
                                writer.WriteLine("2 DATE {0}", dateString);
                                }
                            if (!String.IsNullOrEmpty(person.Death.Place))
                                {
                                writer.WriteLine("2 PLAC {0}", person.Death.Place);
                                }
                            }
                        }
                    int idFamily = 1;
                    foreach (KeyValuePair<String, List<Family>> pair in this.families)
                        {
                        List<Family> families = pair.Value;

                        if (families.Count == 1)
                            {
                            Family family = families[0];

                            writer.WriteLine("0 @F{0}@ FAM", idFamily++);
                            writer.WriteLine("1 HUSB @{0}@", family.Husband.Id);
                            writer.WriteLine("1 WIFE @{0}@", family.Wife.Id);
                            foreach (Person child in family.Children)
                                {
                                writer.WriteLine("1 CHIL @{0}@", child.Id);
                                }
                            }
                        }
                    writer.WriteLine("0 TRLR");
                    }
                }
            }
        }

#if false
    private int MinIndexOf(String s, String[] matchStrings, bool after)
            {
            int minIndex = s.Length;

            for (int i = 0; i < matchStrings.Length; i++)
                {
                int index = s.IndexOf(matchStrings[i]);
                if (index != -1)
                    {
                    if (after)
                        {
                        index += matchStrings[i].Length;
                        }
                    if (index < minIndex)
                        {
                        minIndex = index;
                        }
                    }
                }

            if (minIndex == s.Length)
                return -1;

            return minIndex;
            }
#endif
    }
