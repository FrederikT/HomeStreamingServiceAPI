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
    }
}
