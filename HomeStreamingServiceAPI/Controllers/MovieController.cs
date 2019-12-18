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
        public void Post([FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Movie m = (Movie)JsonConvert.DeserializeObject(value);
            conn.AddMovie(m);
        }

        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Movie m = (Movie)JsonConvert.DeserializeObject(value);
            m.Id = id;
            conn.AddMovie(m);
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
