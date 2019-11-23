using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI
{
    public class Season
    {
        private int id;
        private string title;
        private string originalTitle;
        private int number;
        private Show show;

        public Season(int id, string title, string originalTitle, int number, Show show)
        {
            this.id = id;
            this.title = title;
            this.originalTitle = originalTitle;
            this.number = number;
            this.show = show;
        }

        public Season(int id, Show show)
        {
            this.id = id;
            this.show = show;
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

        public int Number
        {
            get => number;
            set => number = value;
        }

        public Show Show
        {
            get => show;
            set => show = value;
        }
    }
}
