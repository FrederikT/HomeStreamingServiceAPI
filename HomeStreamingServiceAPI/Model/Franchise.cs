using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI.Model
{
    public class Franchise
    {
        private string name;
        private int id;

        public Franchise(string name, int id)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.id = id;
        }

        public string Name => name;

        public int Id => id;

        //!!DEPENDENCY: Other classes use comparison only by id!
        public override bool Equals(object obj)
        {
            Franchise franchise = null;
            try
            {
                franchise = (Franchise)obj;
            }
            catch
            {
                return false;
            }

            if (this.id == franchise.id)
            {
                return true;
            }

            return false;

        }
    }
}
