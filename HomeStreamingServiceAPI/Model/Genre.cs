using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI.Model
{
    public class Genre
    {
        private string name;

        public Genre(string name)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name
        {
            get => name;
            set => name = value;
        }


        public override bool Equals(object obj)
        {
            Genre genre = null;
            try
            {
                genre = (Genre)obj;
            }
            catch
            {
                return false;
            }

            if (this.name == genre.name)
            {
                return true;
            }

            return false;

        }

    }
}
