using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.VisualBasic.CompilerServices;
using Org.BouncyCastle.Math.EC.Endo;

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
        private List<Season> seasonList;
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
                connection.Close();
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


            if (metaDataList == null || metaDataList.Count == 0)
            {
                metaDataList = GetMetaData();
            }
            if (seasonList == null || seasonList.Count == 0)
            {
                seasonList = GetSeason();
            }

            Dictionary<int, int> episodeSeason = new Dictionary<int, int>();
            string query = "Select * from Episode_Season";

            if (this.OpenConnection() == true)
            {

                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();



                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int seasonId = int.Parse(dataReader["seasonId"].ToString());
                    int episodeId = int.Parse(dataReader["episodeId"].ToString());
                    episodeSeason.Add(episodeId, seasonId);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            episodeList = new List<Episode>();
            query = "Select * from Episode";

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
                    int duration = int.Parse(dataReader["duration"].ToString());

                    // NComparator for metadata needed, find element
                    try
                    {
                        int index = metaDataList.IndexOf(new MetaData(id, "")); //comparator only checks for id...
                        MetaData meta = metaDataList[index];
                        int seasonId = episodeSeason[id];
                        Season yeSeason = new Season(new MetaData(seasonId, " "), null);
                        int seasonIndex = seasonList.IndexOf(yeSeason);
                        Season season = seasonList[seasonIndex];
                        Episode episode = new Episode(meta, season, duration);
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

            if (metaDataList == null || metaDataList.Count == 0)
            {
                metaDataList = GetMetaData();
            }

            if (franchiseList == null || franchiseList.Count == 0)
            {
                GetFranchise();
            }

            adaptationList = new List<Adaptation>();
            string query = "Select * from Adaptation";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();


                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int id = int.Parse(dataReader["ID"].ToString());
                    int franchise = -1; // id cannot be negative -> -1 is not initialized (=NULL, but int cant be null)
                    if (!dataReader.IsDBNull(1))
                    {
                        franchise = int.Parse(dataReader["franchise"].ToString());
                    }
                            
                   
                    try
                    {
                        int index = metaDataList.IndexOf(new MetaData(id, "")); //comparator only checks for id...
                        MetaData meta = metaDataList[index];
                        Franchise adaptationFranchise = null;
                        if (franchise != -1)
                        {
                            index = franchiseList.IndexOf(new Franchise("", franchise));
                            adaptationFranchise = franchiseList[index];
                        }

                        Adaptation adaptation = new Adaptation(meta, adaptationFranchise);
                        adaptationList.Add(adaptation);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("There was no metadata or franchise found for this adaptation:" + id);
                    }

                }

                foreach (var adaption in adaptationList)
                {
                    GetGenreForAdaptation(adaption.Id);
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

            if (adaptationList == null || adaptationList.Count == 0)
            {
                GetAdaptation();
            }

            movieList = new List<Movie>();
            string query = "Select * from Movie";

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
                    int duration = int.Parse(dataReader["duration"].ToString());

                    // NComparator for metadata needed, find element
                    try
                    {
                        int index = adaptationList.IndexOf(new Adaptation(id, "","")); //comparator only checks for id...
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
        public List<Adaptation> GetShow()
        {

            if (adaptationList == null || adaptationList.Count == 0)
            {
                GetAdaptation();
            }

            if (movieList == null ||movieList.Count == 0)
            {
                GetMovie();
            }

            showList = new List<Adaptation>();
            
            
            foreach (var adaptation in adaptationList)
            {
                if (! movieList.Contains(adaptation))
                {
                    showList.Add(adaptation);
                }
            }

            return showList;
        }


        // get season -> metadata w/o child episode or adaptation

        public List<Season> GetSeason()
        {
            seasonList = new List<Season>();
            var seasonListWithoutShows = new List<MetaData>();
          
            if (metaDataList == null || metaDataList.Count == 0)
            {
                GetMetaData();
            }
            if (showList == null || showList.Count == 0)
            {
                GetShow();
            }

            string query ="Select * from metadata where ID NOT in (SELECT id from episode) and id NOT in (SELECT id from adaptation)";

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
                    meta.Description = dataReader["description"].ToString();
                    seasonListWithoutShows.Add(meta);

                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
            }



            query = "Select * from season_show";

            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    int seasonId = int.Parse(dataReader["seasonId"].ToString());
                    int showId = int.Parse(dataReader["showId"].ToString());
                    int indexOfSeasonInList = seasonListWithoutShows.IndexOf(new MetaData(seasonId, " "));
                    foreach (var show in showList)
                    {
                        if (show.Id == showId)
                        {
                            seasonList.Add(new Season(seasonListWithoutShows[indexOfSeasonInList], show));
                        }
                    }

                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return seasonList;
        }


        private void GetGenreForAdaptation(int adaptionId)
        {
            string query = "Select * from Genre_Adaptation where adaptation = '"+adaptionId+"'";

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
                    string name = dataReader["genre"].ToString();
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

        private int getLastID(string tableName)
        {
            int id = -1;
            string sql = "Select Max(id) from " + tableName;
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store
                while (dataReader.Read())
                {
                    id = int.Parse(dataReader["id"].ToString());
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();
            }

            return id;
        }

        public void AddMetadata(int id, string title, string originalTitle, string description)
        {
            String sql = "INSERT INTO `metadata` (`ID`, `title`, `originalTitle`, `description`) VALUES(";
            if (id >= 0)
            {
                sql += "'"+id + "', ";
            }else
            {
                sql += "NULL, ";
            }
            if (title != null)
            {
                sql += "'" + title + "',";
            }
            else
            {
                sql += "NULL, ";
            }
            if (originalTitle != null)
            {
                sql += "'" + originalTitle + "', ";
            }
            else
            {
                sql += "NULL, ";
            }
            if (description!= null)
            {
                sql += "'" + description + "')"; 
            }
            else
            {
                sql += "NULL)";
            }
            executeSQL(sql);
        }

        public void AddMetadata(MetaData meta)
        {
            AddMetadata(meta.Id, meta.Title, meta.OriginalTitle, meta.Description);
        }

        public void AddSeason(int id, string title, string originalTitle, string description, int showId)
        {
            AddMetadata(id, title, originalTitle, description);

            if (id >= 0)
            {

            }
            else
            {
                id = getLastID("metadata");
            }

            string sql = "INSERT INTO `season_show` (`seasonID`, `showID`) Values('" + id + "', '" + showId + "')";
            executeSQL(sql);
        }

        public void AddSeason(int id, string title, string originalTitle, string description, Adaptation show)
        {
            AddSeason(id,title,originalTitle,description,show.Id);
        }
        
        public void AddSeason(Season season)
        { 
            AddSeason(season.Id, season.Title, season.OriginalTitle, season.Description, season.Show.Id);
        }

        public void AddEpisode(int id, string title, string originalTitle, string description, int duration, Season season)
        {
           
            AddMetadata(id, title, originalTitle, description);
            
            if ( id>=0)
            {
               
            }
            else
            {
                 id = getLastID("metadata");
            }

            string sql = "INSERT INTO `episode` (`ID`, `duration`) Values('"+id+"', '"+duration+"')";
            executeSQL(sql);
            

            sql = "Insert into episode_season values('" + id + "', '" + season.Id + "')";
            executeSQL(sql);

        }

        public void AddEpisode(Episode episode)
        {
            MetaData meta = new MetaData(episode.Id, episode.Title, episode.OriginalTitle, episode.Description);
            AddMetadata(meta);
            int id;

            if (episode.Id >= 0)
            {
                id = episode.Id;
            }
            else
            {
                id = getLastID("metadata");
            }
            string sql = "INSERT INTO `episode` (`ID`, `duration`) Values('" + id + "', '" + episode.Duration + "')";
            executeSQL(sql);

            sql = "Insert into episode_season values('" + id + "', '" + episode.Season.Id + "')";
            executeSQL(sql);
        }

        public void addAdaptation(int id, string title, string originalTitle, string description, Franchise franchise, List<Genre> genreList)
        {
            int mId;
            AddMetadata(id, title, originalTitle, description);
            if (id >= 0)
            {
                mId = id;
            }
            else
            {
                mId = getLastID("metadata");
            }
            string sql = "INSERT INTO `adaptation` (`ID`, `franchise`) Values('" + mId + "', '" + franchise.Id + "')";
            executeSQL(sql);

            foreach (var genre in genreList)
            {
                sql = "INSERT INTO `genre_adaptation` (`genre`, `adaptation`) Values('" + genre.Name + "', '" + mId + "')";
                executeSQL(sql);
            }


        }

        public void addAdaptation(Adaptation adaptation)
        {
            addAdaptation(adaptation.Id, adaptation.Title, adaptation.OriginalTitle, adaptation.Description, adaptation.Franchise, adaptation.Genre);
            
        }

        public void addMovie(int id, string title, string originalTitle, string description, Franchise franchise, List<Genre> genreList, int duration)
        {
            int mId;
            addAdaptation(id, title, originalTitle, description, franchise, genreList);
            if (id >= 0)
            {
                mId = id;
            }
            else
            {
                mId = getLastID("metadata");
            }
            string sql = "INSERT INTO `movie` (`ID`, `duration`) Values('" + mId + "', '" + duration + "')";
            executeSQL(sql);

        }

        public void addMovie(Movie movie)
        {
            int mId;
            addAdaptation(movie.Id, movie.Title, movie.OriginalTitle, movie.Description, movie.Franchise, movie.Genre);
            if (movie.Id >= 0)
            {
                mId = movie.Id;
            }
            else
            {
                mId = getLastID("metadata");
            }
            string sql = "INSERT INTO `movie` (`ID`, `duration`) Values('" + mId + "', '" + movie.Duration + "')";
            executeSQL(sql);

        }

        public void addFranchise(int id, string name)
        {
            String sql = "INSERT INTO `franchise` (`ID`, `name`) VALUES(";
            if (id >= 0)
            {
                sql += "'" + id + "', ";
            }
            else
            {
                sql += "NULL, ";
            }
            if (name != null)
            {
                sql += "'" + name + "')";
            }
            else
            {
                sql += "NULL)";
            }
            executeSQL(sql);

        }

        public void addFranchise(Franchise franchise)
        {
            addFranchise(franchise.Id, franchise.Name);
        }

        public void addGenre(string name)
        {
            string sql = "INSERT INTO `genre` (`name`) Values('" + name + "')";
            executeSQL(sql);
        }

        public void addGenre(Genre genre)
        {
            addGenre(genre.Name);
        }


        public void deleteGenre(string name)
        {
            string sql = "Delete from genre_adaption where genre = " + name;
            executeSQL(sql);
            sql = "Delete from genre where name = "+name;
            executeSQL(sql);
        }
        
        public void deleteGenre(Genre genre)
        {
            deleteGenre(genre.Name);
        }

        public void deleteFranchise(int id)
        {
            string sql = "Delete from adaption where franchise = " + id;
            executeSQL(sql);
            sql = "Delete from franchise where id = " + id;
            executeSQL(sql);
        }

        public void deleteFranchise(Franchise franchise)
        {
            deleteFranchise(franchise.Id);
        }

        public void deleteMetaData(int id)
        {
            string sql = "Delete from episode_season where episodeId = " + id;
            executeSQL(sql);
            sql = "Delete from episode_season where seasonId = " + id;
            executeSQL(sql);
            sql = "Delete from episode where id = " + id;
            executeSQL(sql);
            sql = "Delete from movie where id = " + id;
            executeSQL(sql);
            sql = "Delete from genre_adaptation where adaptation = " + id;
            executeSQL(sql);
            sql = "Delete from season_show where seasonId = " + id;
            executeSQL(sql);
            sql = "Delete from season_show where showId = " + id;
            executeSQL(sql);
            sql = "Delete from adaptation where id = " + id;
            executeSQL(sql);
            sql = "Delete from metadata where id = " + id;
            executeSQL(sql);

        }

        public void deleteMetadata(MetaData meta)
        {
            deleteMetaData(meta.Id);
        }

        /**
         * Just to be sure that no constraints are violated the other deletes will call deleteMetaData
         * This might be unnessesary but can not damage the data in the database.
         * Since all other classes have MetaData as a base class this is a safe procedure.
         * This also automatically gets rid of errors in the database, e.g. an movie thats also a episode without causing to much trouble
         */

        //Todo: only delete when item is part of class (deleteEpisode will not deleteMovie if no episode with that id exists)


        public void deleteShow(int id)
        {
            deleteMetaData(id);
        }

        public void deleteShow(Adaptation show)
        {
            deleteMetaData(show.Id);
        }

        public void deleteSeason(int id)
        {
            deleteMetaData(id);
        }
        public void deleteSeason(Season season)
        {
            deleteMetaData(season.Id);
        }

        public void deleteEpisode(int id)
        {
            deleteMetaData(id);
        }
        public void deleteEpisode(Episode episode)
        {
            deleteMetaData(episode.Id);
        }

        public void deleteMovie(int id)
        {
            deleteMetaData(id);
        }
        public void deleteMovie(Movie movie)
        {
            deleteMetaData(movie.Id);
        }

        public void deleteAdaptation(int id)
        {
            deleteMetaData(id);
        }
        public void deleteAdaptation(Adaptation adaptation)
        {
            deleteMetaData(adaptation.Id);
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
