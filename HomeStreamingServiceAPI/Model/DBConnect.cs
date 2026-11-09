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
        private readonly MySqlConnection connection;
        private List<MetaData> metaDataList;
        private List<Genre> genreList;
        private List<Franchise> franchiseList;
        private List<Adaptation> adaptationList;
        private List<Movie> movieList;
        private List<Episode> episodeList;
        private List<Season> seasonList;
        private List<Adaptation> showList;

        public DBConnect()
        {
            const string variable = "ConnectionStrings__StreamingDatabase";
            string connectionString = Environment.GetEnvironmentVariable(variable);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Set {variable} to configure the streaming database.");
            }

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
            ExecuteNonQuery(sql);
        }

        private void ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            if (this.OpenConnection() == true)
            {
                try
                {
                    using var command = new MySqlCommand(sql, connection);
                    command.Parameters.AddRange(parameters);
                    command.ExecuteNonQuery();
                }
                finally
                {
                    CloseConnection();
                }
            }
        }

        public List<MetaData> GetMetaData()
        {
            metaDataList = new List<MetaData>();
            string query = "Select * from metadata";

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
            string query = "Select * from genre";

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
            string query = "Select * from franchise";

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
            string query = "Select * from episode_season";

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
            query = "Select * from episode";

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
                    string filePath = dataReader["filePath"].ToString();
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
                        if (filePath != null)
                        {
                            episode.FilePath = filePath;
                        }
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
            string query = "Select * from adaptation";

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
            string query = "Select * from movie";

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
                    string filePath = dataReader["filePath"].ToString();

                    // NComparator for metadata needed, find element
                    try
                    {
                        int index = adaptationList.IndexOf(new Adaptation(id, "","")); //comparator only checks for id...
                        Adaptation adaptation = adaptationList[index];
                        Movie movie = new Movie(adaptation, duration);
                        if (filePath != null)
                        {
                            movie.FilePath = filePath;
                        }
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

            string query ="Select * from metadata where id NOT in (SELECT id from episode) and id NOT in (SELECT id from adaptation)";

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
            string query = "Select * from genre_adaptation where adaptation = '"+adaptionId+"'";           
            
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
                    name = name.Trim();
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

        private void InTransaction(Action<MySqlTransaction> write)
        {
            connection.Open();
            try
            {
                using var transaction = connection.BeginTransaction();
                write(transaction);
                transaction.Commit();
            }
            finally
            {
                connection.Close();
            }
        }

        private long ExecuteWrite(MySqlTransaction transaction, string sql, params MySqlParameter[] parameters)
        {
            using var command = new MySqlCommand(sql, connection, transaction);
            command.Parameters.AddRange(parameters);
            command.ExecuteNonQuery();
            return command.LastInsertedId;
        }

        private int InsertMetadata(MySqlTransaction transaction, MetaData metadata)
        {
            long insertedId = ExecuteWrite(transaction,
                "INSERT INTO metadata (id, title, originalTitle, description) VALUES (@id, @title, @originalTitle, @description)",
                new MySqlParameter("@id", metadata.Id >= 0 ? (object)metadata.Id : DBNull.Value),
                new MySqlParameter("@title", (object)metadata.Title ?? DBNull.Value),
                new MySqlParameter("@originalTitle", (object)metadata.OriginalTitle ?? DBNull.Value),
                new MySqlParameter("@description", (object)metadata.Description ?? DBNull.Value));
            return metadata.Id >= 0 ? metadata.Id : checked((int)insertedId);
        }

        public void AddSeason(Season season)
        {
            InTransaction(transaction =>
            {
                int id = InsertMetadata(transaction, season);
                ExecuteWrite(transaction, "INSERT INTO season_show (seasonId, showId) VALUES (@id, @showId)",
                    new MySqlParameter("@id", id),
                    new MySqlParameter("@showId", season.Show.Id));
            });
        }

        public void AddEpisode(Episode episode)
        {
            InTransaction(transaction =>
            {
                int id = InsertMetadata(transaction, episode);
                ExecuteWrite(transaction, "INSERT INTO episode (id, duration, filePath) VALUES (@id, @duration, @filePath)",
                    new MySqlParameter("@id", id),
                    new MySqlParameter("@duration", episode.Duration),
                    new MySqlParameter("@filePath", episode.FilePath ?? string.Empty));
                ExecuteWrite(transaction, "INSERT INTO episode_season (episodeId, seasonId) VALUES (@id, @seasonId)",
                    new MySqlParameter("@id", id),
                    new MySqlParameter("@seasonId", episode.Season.Id));
            });
        }

        private int InsertAdaptation(MySqlTransaction transaction, Adaptation adaptation)
        {
            int id = InsertMetadata(transaction, adaptation);
            ExecuteWrite(transaction, "INSERT INTO adaptation (id, franchise) VALUES (@id, @franchise)",
                new MySqlParameter("@id", id),
                new MySqlParameter("@franchise", adaptation.Franchise.Id));

            foreach (var genre in adaptation.Genre)
            {
                ExecuteWrite(transaction, "INSERT INTO genre_adaptation (genre, adaptation) VALUES (@genre, @id)",
                    new MySqlParameter("@genre", genre.Name ?? string.Empty),
                    new MySqlParameter("@id", id));
            }

            return id;
        }

        public void AddAdaptation(Adaptation adaptation)
        {
            InTransaction(transaction => InsertAdaptation(transaction, adaptation));
        }

        public void AddMovie(Movie movie)
        {
            InTransaction(transaction =>
            {
                int id = InsertAdaptation(transaction, movie);
                if (string.IsNullOrEmpty(movie.FilePath))
                {
                    ExecuteWrite(transaction, "INSERT INTO movie (id, duration) VALUES (@id, @duration)",
                        new MySqlParameter("@id", id),
                        new MySqlParameter("@duration", movie.Duration));
                }
                else
                {
                    ExecuteWrite(transaction, "INSERT INTO movie (id, duration, filePath) VALUES (@id, @duration, @filePath)",
                        new MySqlParameter("@id", id),
                        new MySqlParameter("@duration", movie.Duration),
                        new MySqlParameter("@filePath", movie.FilePath));
                }
            });
        }

        public void AddFranchise(int id, string name)
        {
            ExecuteNonQuery("INSERT INTO `franchise` (`id`, `name`) VALUES (@id, @name)",
                new MySqlParameter("@id", id >= 0 ? (object)id : DBNull.Value),
                new MySqlParameter("@name", (object)name ?? DBNull.Value));

        }

        public void AddFranchise(Franchise franchise)
        {
            AddFranchise(franchise.Id, franchise.Name);
        }

        public void AddGenre(string name)
        {
            ExecuteNonQuery("INSERT INTO `genre` (`name`) VALUES (@name)",
                new MySqlParameter("@name", name ?? string.Empty));
        }

        public void AddGenre(Genre genre)
        {
            AddGenre(genre.Name);
        }


        public void DeleteGenre(string name)
        {
            ExecuteNonQuery("DELETE FROM genre_adaptation WHERE genre = @name",
                new MySqlParameter("@name", (object)name ?? DBNull.Value));
            ExecuteNonQuery("DELETE FROM genre WHERE name = @name",
                new MySqlParameter("@name", (object)name ?? DBNull.Value));
        }
        
        public void DeleteGenre(Genre genre)
        {
            DeleteGenre(genre.Name);
        }

        public void DeleteFranchise(int id)
        {
            string sql = "Delete from adaption where franchise = " + id;
            executeSQL(sql);
            sql = "Delete from franchise where id = " + id;
            executeSQL(sql);
        }

        public void DeleteFranchise(Franchise franchise)
        {
            DeleteFranchise(franchise.Id);
        }

        private void DeleteMedia(int id, string typeCondition, params string[] deleteStatements)
        {
            InTransaction(transaction =>
            {
                using var target = new MySqlCommand(
                    "SELECT id FROM metadata WHERE id = @id AND " + typeCondition + " FOR UPDATE",
                    connection, transaction);
                target.Parameters.AddWithValue("@id", id);
                if (target.ExecuteScalar() == null)
                {
                    return;
                }

                foreach (string statement in deleteStatements)
                {
                    ExecuteWrite(transaction, statement, new MySqlParameter("@id", id));
                }

                ExecuteWrite(transaction, "DELETE FROM metadata WHERE id = @id", new MySqlParameter("@id", id));
            });
        }

        public void DeleteShow(int id)
        {
            DeleteMedia(id,
                "EXISTS (SELECT 1 FROM adaptation WHERE id = @id) AND NOT EXISTS (SELECT 1 FROM movie WHERE id = @id)",
                "DELETE FROM season_show WHERE showId = @id",
                "DELETE FROM genre_adaptation WHERE adaptation = @id",
                "DELETE FROM adaptation WHERE id = @id");
        }

        public void DeleteSeason(int id)
        {
            DeleteMedia(id,
                "EXISTS (SELECT 1 FROM season_show WHERE seasonId = @id) " +
                "AND NOT EXISTS (SELECT 1 FROM adaptation WHERE id = @id) " +
                "AND NOT EXISTS (SELECT 1 FROM episode WHERE id = @id)",
                "DELETE FROM episode_season WHERE seasonId = @id",
                "DELETE FROM season_show WHERE seasonId = @id");
        }

        public void DeleteEpisode(int id)
        {
            DeleteMedia(id, "EXISTS (SELECT 1 FROM episode WHERE id = @id)",
                "DELETE FROM episode_season WHERE episodeId = @id",
                "DELETE FROM episode WHERE id = @id");
        }

        public void DeleteMovie(int id)
        {
            DeleteMedia(id, "EXISTS (SELECT 1 FROM movie WHERE id = @id)",
                "DELETE FROM genre_adaptation WHERE adaptation = @id",
                "DELETE FROM movie WHERE id = @id",
                "DELETE FROM adaptation WHERE id = @id");
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
