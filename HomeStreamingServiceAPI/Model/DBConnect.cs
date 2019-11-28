using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;

namespace HomeStreamingServiceAPI.Model
{
    class DBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string userName;
        private string password;
        private List<MetaData> metaDataList;
        private List<Genre> genreList;
        private List<Franchise> franchiseList;
        private List<Adaptation> adaptationList;
        private List<Movie> movieList;
        private List<Episode> episodeList;
        private List<MetaData> seasonList;
        private List<Adaptation> showList;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "localhost";
            database = "homestreamingservice";
            userName = "streaming_user";
            password = "streaming";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
                               database + ";" + "UID=" + userName + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                Debug.WriteLine("Successfully connected");
                return true;
            }
            catch (MySqlException e)
            {
                switch (e.Number)
                {
                    case 0: // Cannot connect to server.
                        Debug.WriteLine("Cannot connect to server");
                        break;

                    case 1045: //Invalid user name and/or password
                        Debug.WriteLine("Invalid username/password, please try again");
                        break;
                }

                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public void executeSQL(string sql)
        {
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public List<MetaData> GetMetaData()
        {
            metaDataList = new List<MetaData>();
            string query = "Select * from MetaData";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int id = int.Parse(dataReader["id"].ToString());
                    string title = dataReader["title"].ToString();
                    MetaData meta = new MetaData(id, title);
                    meta.OriginalTitle = dataReader["originalTitle"].ToString();
                    metaDataList.Add(meta);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return metaDataList;

        }

        public List<Genre> GetGenre()
        {
            genreList = new List<Genre>();
            string query = "Select * from Genre";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string name = dataReader["name"].ToString();
                    Genre genre = new Genre(name);
                    genreList.Add(genre);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return genreList;

        }


        public List<Franchise> GetFranchise()
        {
            franchiseList = new List<Franchise>();
            string query = "Select * from Franchise";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int id = int.Parse(dataReader["id"].ToString());
                    string name = dataReader["name"].ToString();
                    Franchise franchise = new Franchise(name, id);
                    franchiseList.Add(franchise);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return franchiseList;

        }

        public List<Episode> GetEpisode()
        {
            episodeList = new List<Episode>();
            string query = "Select * from Episode";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (metaDataList == null || metaDataList.Count == 0)
                {
                    GetMetaData();
                }

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int id = int.Parse(dataReader["id"].ToString());
                    int duration = int.Parse(dataReader["duration"].ToString());

                    // NComparator for metadata needed, find element
                    try
                    {
                        int index = metaDataList.IndexOf(new MetaData(id, "")); //comparator only checks for id...
                        MetaData meta = metaDataList[index];
                        Episode episode = new Episode(meta, duration);
                        episodeList.Add(episode);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("There was no metadata found for this episode:" + id);
                    }

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return episodeList;

        }

        public List<Adaptation> GetAdaptation()
        {
            adaptationList = new List<Adaptation>();
            string query = "Select * from Adaptation";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (metaDataList == null || metaDataList.Count == 0)
                {
                    GetMetaData();
                }

                if (franchiseList == null || franchiseList.Count == 0)
                {
                    GetFranchise();
                }

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int id = int.Parse(dataReader["ID"].ToString());
                    int franchise = int.Parse(dataReader["franchise"].ToString());

                    try
                    {
                        int index = metaDataList.IndexOf(new MetaData(id, "")); //comparator only checks for id...
                        MetaData meta = metaDataList[index];
                        Franchise adaptationFranchise = null;
                        if (franchise != null)
                        {
                            index = franchiseList.IndexOf(new Franchise("", franchise));
                            adaptationFranchise = franchiseList[index];
                        }

                        Adaptation adaptation = new Adaptation(meta, adaptationFranchise);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("There was no metadata or franchise found for this adaptation:" + id);
                    }

                }

                foreach (var adaption in adaptationList)
                {
                    GetGenreForAdapation(adaption.Id);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return adaptationList;

        }

        

        public List<Movie> GetMovie()
        {
            movieList = new List<Movie>();
            string query = "Select * from Movie";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if (adaptationList == null || adaptationList.Count == 0)
                {
                    GetAdaptation();
                }

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int id = int.Parse(dataReader["id"].ToString());
                    int duration = int.Parse(dataReader["duration"].ToString());

                    // NComparator for metadata needed, find element
                    try
                    {
                        int index = adaptationList.IndexOf(new Adaptation(id, "",
                            "")); //comparator only checks for id...
                        Adaptation adaptation = adaptationList[index];
                        Movie movie = new Movie(adaptation, duration);
                        movieList.Add(movie);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("There was no adaptation found for this movie:" + id);
                    }

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return movieList;

        }

        //get Show -> adaptations w/o child movie
        public List<Adaptation> GetShows()
        {
            showList = new List<Adaptation>();
            if (adaptationList == null || adaptationList.Count == 0)
            {
                GetAdaptation();
            }
            if (movieList == null || movieList.Count == 0)
            {
                GetMovie();
            }

            
            foreach (var adaptation in adaptationList)
            {
                bool foundMovieForAdaptation = false;
                foreach (var movie in movieList)
                {
                    if (adaptation.Id == movie.Id)
                    {
                        foundMovieForAdaptation = true;
                    }
                }
                if (!foundMovieForAdaptation)
                {
                    showList.Add(adaptation);
                }
            }

            return showList;
        }


        // get season -> metadata w/o child episode or adaptation

        public List<MetaData> GetSeason()
        {
            seasonList = new List<MetaData>();
            if (adaptationList == null || adaptationList.Count == 0)
            {
                GetAdaptation();
            }
            if (episodeList == null || episodeList.Count == 0)
            {
                GetMovie();
            }
            if (metaDataList == null || metaDataList.Count == 0)
            {
                GetMetaData();
            }

            foreach (var metadata in metaDataList)
            {
                bool foundEpisodeOrAdaptation = false;
                foreach (var episode in episodeList)
                {
                    if (metadata.Id == episode.Id)
                    {
                        foundEpisodeOrAdaptation = true;
                    }
                }
                foreach (var adaptation in adaptationList)
                {
                    if (metadata.Id == adaptation.Id)
                    {
                        foundEpisodeOrAdaptation = true;
                    }
                }
                if (!foundEpisodeOrAdaptation)
                {
                    seasonList.Add(metadata);
                }
            }

            return seasonList;
        }


        private void GetGenreForAdapation(int adaptionId)
        {
            string query = "Select * from Genre_Adaptation";

            if (genreList == null || genreList.Count == 0)
            {
                GetGenre();
            }

            if (adaptationList == null || adaptationList.Count == 0)
            {
                GetAdaptation();
            }

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    string name = dataReader["name"].ToString();
                    Genre genre = new Genre(name);
                    int genreIndex = genreList.IndexOf(genre);
                    Adaptation adaptation = adaptationList[adaptationList.IndexOf(new Adaptation(adaptionId, "", ""))];
                    adaptation.Genre.Add(genreList[genreIndex]);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

        }


        /* Irrelevant Select
        public List<string>[] Select(string query)
        {
            //Create a list to store the result
            List<string>[] list = new List<string>[3];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[0].Add(dataReader["id"] + "");
                    list[1].Add(dataReader["name"] + "");
                    list[2].Add(dataReader["age"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }*/


    }
}
