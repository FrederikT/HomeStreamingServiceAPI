using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI.Model
{
    public class Adaptation : MetaData
    {
        private List<Genre> genre;
        private Franchise franchise;

        public Adaptation(int id, string title, string originalTitle, string description, List<Genre> genre, Franchise franchise) : base(id, title, originalTitle, description)
        {
            this.genre = genre;
            this.franchise = franchise;
        }

        public Adaptation(int id, string title, List<Genre> genre, Franchise franchise) : base(id, title)
        {
            this.genre = genre;
            this.franchise = franchise;
        }

        public Adaptation(int id, string title, string originalTitle, string description, List<Genre> genre) : base(id, title, originalTitle, description)
        {
            this.genre = genre;
        }

        public Adaptation(int id, string title, List<Genre> genre) : base(id, title)
        {
            this.genre = genre;
        }
        public Adaptation(int id, string title, string originalTitle) : base(id, title)
        {
            this.OriginalTitle = originalTitle;
        }

        public Adaptation(MetaData meta, Franchise adaptationFranchise) : base(meta)
        {
            this.franchise = adaptationFranchise;
        }

        protected Adaptation(Adaptation adaptation) : base((MetaData) adaptation)
        {
            this.genre = adaptation.genre;
            this.franchise = adaptation.franchise;
        }

        public List<Genre> Genre => genre;

        public Franchise Franchise => franchise;

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            //should compare by id 
            return base.Equals(obj);
        }
    }
}
