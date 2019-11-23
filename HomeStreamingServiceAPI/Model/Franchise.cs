using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI
{
    public class Franchise
    {
        private int id;
        private string name;

        public Franchise(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

    }
}
