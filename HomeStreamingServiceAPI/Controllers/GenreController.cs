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
    public class GenreController : ControllerBase
    {

        // GET: api/Genre
        [HttpGet]
        public IEnumerable<string> Get()
        {
            DBConnect conn = new DBConnect();
            List<Genre> genreList = conn.GetGenre();
            List<string> arr = new List<string>();
            foreach (var genre in genreList)
            {
                arr.Add(JsonConvert.SerializeObject(genre));
            }

            return arr.ToArray();
        }

        // GET: api/Genre/Action
        [HttpGet("{name}", Name = "GetGenre")]
        public string Get(string name)
        {
            DBConnect conn = new DBConnect();
            List<Genre> genreList = conn.GetGenre();
            Genre g = null;
            foreach (var genre in genreList)
            {
                if (genre.Name == name)
                {
                    g = genre;
                }
            }

            string output = JsonConvert.SerializeObject(g);
            return output;
        }

        // POST: api/Genre
        [HttpPost]
        public void Post([FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Genre g = (Genre)JsonConvert.DeserializeObject(value);
            conn.addGenre(g);
        }

        // PUT: api/Genre/Action
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Genre g = (Genre)JsonConvert.DeserializeObject(value);
            g.Name = name;
            conn.addGenre(g);

        }

        // DELETE: api/Genre/Delete/Action
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            DBConnect conn = new DBConnect();
            conn.deleteGenre(name);
        }
    }
}
