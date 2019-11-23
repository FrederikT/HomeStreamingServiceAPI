using System;
using System.Collections.Generic;

namespace HomeStreamingServiceAPI
{
    public class Video
    {
        private int id;
        private int time;
        private string[] studio;
        private string description;
        private DateTime releaseDate;
        private List<Genre> genres;

        public Video(int id, int time, string[] studio, string description, DateTime releaseDate, List<Genre> genres)
        {
            this.id = id;
            this.time = time;
            this.studio = studio;
            this.description = description;
            this.releaseDate = releaseDate;
            this.genres = genres;
        }
        public Video(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        public Video(int id)
        {
            this.id = id;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public int Time
        {
            get => time;
            set => time = value;
        }

        public string[] Studio
        {
            get => studio;
            set => studio = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public DateTime ReleaseDate
        {
            get => releaseDate;
            set => releaseDate = value;
        }

        public List<Genre> Genres
        {
            get => genres;
            set => genres = value;
        }
    }
}