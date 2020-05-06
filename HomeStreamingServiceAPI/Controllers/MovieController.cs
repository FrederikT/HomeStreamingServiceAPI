using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeStreamingServiceAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HomeStreamingServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        // GET: api/Movie
        [HttpGet]
        public IEnumerable<string> Get()
        {
            DBConnect conn = new DBConnect();
            List<Movie> movieList = conn.GetMovie();
            List<string> arr = new List<string>();
            foreach (var movie in movieList)
            {
                arr.Add(JsonConvert.SerializeObject(movie));
            }

            return arr.ToArray();
        }

        // GET: api/Movie/5
        [HttpGet("{id}", Name = "GetMovie")]
        public string Get(int id)
        {
            DBConnect conn = new DBConnect();
            List<Movie> movieList = conn.GetMovie();
            Movie m = null;
            foreach (var movie in movieList)
            {
                if (movie.Id == id)
                {
                    m = movie;
                }
            }

            string output = JsonConvert.SerializeObject(m);
            return output;
        }

        // POST: api/Movie
        [HttpPost]
        public string Post()
        {

            DBConnect conn = new DBConnect();
            try
            {
                List<Genre> genreList = new List<Genre>();
                if (Request.Form.ContainsKey("genre"))
                {
                    // Array will be converted in string
                    // name;name;...
                    string genres = Request.Form["genre"];
                    string[] genreArr = genres.Split(';');


                    foreach (var genre in genreArr)
                    {
                        genreList.Add(new Genre(genre));
                    }
                }

             
                Movie movie = new Movie(-5,  Request.Form["title"], genreList, int.Parse(Request.Form["duration"]));

                if (Request.Form.ContainsKey("id"))
                {
                    movie.Id = int.Parse(Request.Form["id"]);
                }
                if (Request.Form.ContainsKey("originalTitle"))
                {
                    movie.OriginalTitle = Request.Form["originalTitle"];
                }
                if (Request.Form.ContainsKey("description"))
                {
                    movie.Description = Request.Form["description"];
                }
                if (Request.Form.ContainsKey("filePath"))
                {
                    movie.FilePath = Request.Form["filePath"];

                }
                if (Request.Form.ContainsKey("franchiseId"))
                {
                    movie.Franchise = conn.GetFranchise().Find(Franchise => Franchise.Id == int.Parse(Request.Form["franchiseId"]));
                }
              

                conn.AddMovie(movie);
            }
            catch (Exception exception)
            {
                          
                return exception.Message;
            }
            
            return "OK";

        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            /*
            DBConnect conn = new DBConnect();
            Movie m = (Movie)JsonConvert.DeserializeObject(value);
            m.Id = id;
            conn.AddMovie(m);
            */
        }

        // DELETE: api/Movie/Delete/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DBConnect conn = new DBConnect();
            conn.DeleteMovie(id);
        }
    }
}
