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

        public Episode(int id, string title, string originalTitle, string description, int duration) : base(id, title, originalTitle, description)
        {
            this.duration = duration;
        }

        public Episode(int id, string title, int duration) : base(id, title)
        {
            this.duration = duration;
        }

        public Episode(int id, string title, string originalTitle, string description, int duration, string filePath) : base(id, title, originalTitle, description)
        {
            this.duration = duration;
            this.filePath = filePath;
        }

        public Episode(int id, string title, int duration, string filePath) : base(id, title)
        {
            this.duration = duration;
            this.filePath = filePath;
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
    }
}
