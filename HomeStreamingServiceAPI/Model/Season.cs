using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStreamingServiceAPI.Model
{
    public class Season : MetaData
    {
        private Adaptation show;
        

        public Season(int id, Adaptation show,  string title, string originalTitle, string description) : base(id, title, originalTitle, description)
        {
            this.show = show;
        }

        public Season(int id, Adaptation show, string title) : base(id, title)
        {
            this.show = show;
        }

        public Season(MetaData meta, Adaptation show) : base(meta)
        {
            this.show = show;
        }


        public Adaptation Show
        {
            get => show;
            set => show = value;
        }

    }
}
