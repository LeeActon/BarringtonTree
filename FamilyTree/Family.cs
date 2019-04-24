using System;
using System.Collections.Generic;

namespace FamilyTree
    {
    public class Family
        {
        public Person Husband { get; set; }
        public Person Wife { get; set; }

#if false
        private List<Person> parents = null;
        public List<Person> Parents
            {
            get
                {
                if (this.parents == null)
                    {
                    this.parents = new List<Person>();
                    }

                return this.parents;
                }
            }
#endif

        private List<Person> children = null;
        public List<Person> Children
            {
            get
                {
                if (this.children == null)
                    {
                    this.children = new List<Person>();
                    }

                return this.children;
                }
            }

        private List<String> childrenLines = null;
        public List<String> ChildrenLines
            {
            get
                {
                if (this.childrenLines == null)
                    {
                    this.childrenLines = new List<String>();
                    }

                return this.childrenLines;
                }
            }
        }
    }
