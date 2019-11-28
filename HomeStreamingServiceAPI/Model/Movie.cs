using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI.Model
{
    public class Movie : Adaptation
    {
        private int duration;
        private string filePath;

        public Movie(int id, string title, string originalTitle, string description, List<Genre> genre, Franchise franchise, int duration) : base(id, title, originalTitle, description, genre, franchise)
        {
            this.duration = duration;
        }

        public Movie(int id, string title, List<Genre> genre, Franchise franchise, int duration) : base(id, title, genre, franchise)
        {
            this.duration = duration;
        }

        public Movie(int id, string title, string originalTitle, string description, List<Genre> genre, int duration) : base(id, title, originalTitle, description, genre)
        {
            this.duration = duration;
        }

        public Movie(int id, string title, List<Genre> genre, int duration) : base(id, title, genre)
        {
            this.duration = duration;
        }

        public Movie(int id, string title, string originalTitle, string description, List<Genre> genre, Franchise franchise, int duration, string filePath) : base(id, title, originalTitle, description, genre, franchise)
        {
            this.duration = duration;
            this.filePath = filePath;
        }

        public Movie(int id, string title, List<Genre> genre, Franchise franchise, int duration, string filePath) : base(id, title, genre, franchise)
        {
            this.duration = duration;
            this.filePath = filePath;
        }

        public Movie(int id, string title, string originalTitle, string description, List<Genre> genre, int duration, string filePath) : base(id, title, originalTitle, description, genre)
        {
            this.duration = duration;
            this.filePath = filePath;
        }

        public Movie(int id, string title, List<Genre> genre, int duration, string filePath) : base(id, title, genre)
        {
            this.duration = duration;
            this.filePath = filePath;
        }

        public Movie(Adaptation adaptation, int duration) : base(adaptation)
        {
            this.duration = duration;
        }

        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }


        public int Duration => duration;


        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return base.ToString()+"  duration: "+duration;
        }
    }
}
