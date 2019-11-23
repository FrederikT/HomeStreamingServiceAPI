using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI
{
    public class Episode : Video
    {
        private Season season;
        private int numberInSeason;

        public Episode(int id, int time, string[] studio, string description, DateTime releaseDate, List<Genre> genres, Season season, int numberInSeason) : base(id, time, studio, description, releaseDate, genres)
        {
            this.season = season;
            this.numberInSeason = numberInSeason;
        }

        public Episode(int id, string description, Season season, int numberInSeason) : base(id, description)
        {
            this.season = season;
            this.numberInSeason = numberInSeason;
        }

        public Episode(int id, Season season, int numberInSeason) : base(id)
        {
            this.season = season;
            this.numberInSeason = numberInSeason;
        }

        public Season Season
        {
            get => season;
            set => season = value;
        }

        public int NumberInSeason
        {
            get => numberInSeason;
            set => numberInSeason = value;
        }
    }
}
