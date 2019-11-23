using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace HomeStreamingServiceAPI
{
    public class Genre
    {
        private string name;

        public Genre(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get { return name; }
            set { string name = value; }
        }

    }
}
