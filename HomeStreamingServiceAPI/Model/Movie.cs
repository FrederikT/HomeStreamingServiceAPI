using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI
{
    public class Movie : Video 
    {
        private string title;
        private string originalTitle;
        private Franchise franchise;

        public Movie(int id, int time, string[] studio, string description, DateTime releaseDate, List<Genre> genres, string title, string originalTitle, Franchise franchise) : base(id, time, studio, description, releaseDate, genres)
        {
            this.title = title;
            this.originalTitle = originalTitle;
            this.franchise = franchise;
        }

        public Movie(int id, string description, string title, string originalTitle, Franchise franchise) : base(id, description)
        {
            this.title = title;
            this.originalTitle = originalTitle;
            this.franchise = franchise;
        }

        public Movie(int id, string title, string originalTitle, Franchise franchise) : base(id)
        {
            this.title = title;
            this.originalTitle = originalTitle;
            this.franchise = franchise;
        }

        public Movie(int id, int time, string[] studio, string description, DateTime releaseDate, List<Genre> genres, string title, string originalTitle) : base(id, time, studio, description, releaseDate, genres)
        {
            this.title = title;
            this.originalTitle = originalTitle;
        }

        public Movie(int id, string description, string title, string originalTitle) : base(id, description)
        {
            this.title = title;
            this.originalTitle = originalTitle;
        }

        public Movie(int id, string title, string originalTitle) : base(id)
        {
            this.title = title;
            this.originalTitle = originalTitle;
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

        public Franchise Franchise
        {
            get => franchise;
            set => franchise = value;
        }
    }
}
