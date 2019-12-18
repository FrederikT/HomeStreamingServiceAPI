using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;

namespace HomeStreamingServiceAPI.Model
{
    public class Episode : MetaData
    {
        private int duration;
        private string filePath;
        private Season season;

        public Episode(int id, Season season, string title, string originalTitle, string description, int duration) : base(id, title, originalTitle, description)
        {
            this.duration = duration;
            this.season = season;
        }

        public Episode(int id, Season season, string title, int duration) : base(id, title)
        {
            this.duration = duration;
            this.season = season;
        }

        public Episode(int id, Season season, string title, string originalTitle, string description, int duration, string filePath) : base(id, title, originalTitle, description)
        {
            this.duration = duration;
            this.filePath = filePath;
            this.season = season;
        }

        public Episode(int id, Season season, string title, int duration, string filePath) : base(id, title)
        {
            this.duration = duration;
            this.filePath = filePath;
            this.season = season;
        }

        public Episode(MetaData meta, Season season, int duration) : base(meta)
        {
            this.duration = duration;
            this.season = season;
        }

        public int Duration
        {
            get => duration;
            set => duration = value;
        }

        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }

        public Season Season
        {
            get => season;
            set => season = value;
        }
    }
}
