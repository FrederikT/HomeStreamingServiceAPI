using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI.Model
{
    public class MetaData
    {
        private int id;
        private string title;
        private string originalTitle;
        private string description;

        public MetaData(int id, string title, string originalTitle, string description)
        {
            this.id = id;
            this.title = title;
            this.originalTitle = originalTitle;
            this.description = description;
        }

        public MetaData(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        protected MetaData(MetaData meta)
        { 
            this.id = meta.id;
            this.title = meta.title;
            this.originalTitle = meta.originalTitle;
            this.description = meta.description;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Title
        {
            get => title;
            set => title = value;
        }

        public string OriginalTitle
        {
            get => originalTitle;
            set => originalTitle = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public override string ToString()
        {
            string s = originalTitle + " " + title + " " + description; 
            return s;
        }

        private sealed class IdEqualityComparer : IEqualityComparer<MetaData>
        {
            public bool Equals(MetaData x, MetaData y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.id == y.id;
            }

            public int GetHashCode(MetaData obj)
            {
                return obj.id;
            }
        }

        //!!DEPENDENCY: Other classes use comparison only by id!
        public override bool Equals(object obj)
        {
            MetaData meta=null;
            try
            {
                 meta = (MetaData) obj;
            }
            catch
            {
                return false;
            }

            if (this.id == meta.id)
            {
                return true;
            }

            return false;

        }
    }
}
