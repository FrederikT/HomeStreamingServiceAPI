namespace HomeStreamingServiceAPI
{
    public class Show
    {
        private int id;
        private string title;
        private string originalTitle;
        private Franchise franchise;

        public Show(int id, string title, string originalTitle, Franchise franchise)
        {
            this.id = id;
            this.title = title;
            this.originalTitle = originalTitle;
            this.franchise = franchise;
        }

        public Show(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public Show(int id, string title, string originalTitle)
        {
            this.id = id;
            this.title = title;
            this.originalTitle = originalTitle;
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Title
        {
            get => title;
            set => title = value;
        }

        public string OriginalTitle
        {
            get => originalTitle;
            set => originalTitle = value;
        }

        public Franchise Franchise
        {
            get => franchise;
            set => franchise = value;
        }
    }
}
