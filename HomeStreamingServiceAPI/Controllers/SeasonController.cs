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
    public class SeasonController : ControllerBase
    {


        // GET: api/Season
        [HttpGet]
        public IEnumerable<string> Get()
        {
            
            DBConnect conn = new DBConnect();
            List<Season> seasonList = conn.GetSeason();
            List<string> arr = new List<string>();
            foreach (var season in seasonList)
            {
                arr.Add(JsonConvert.SerializeObject(season));
            }

            return arr.ToArray();
        }

        // GET: api/Season/5
        [HttpGet("{id}", Name = "GetSeason")]
        public string Get(int id)
        {
            DBConnect conn = new DBConnect();
            List<Season> seasonList = conn.GetSeason();
            Season s = null;
            foreach (var season in seasonList)
            {
                if (season.Id == id)
                {
                    s = season;
                }
            }

            string output = JsonConvert.SerializeObject(s);
            return output;
        }

        // POST: api/Season
        [HttpPost]
        public void Post([FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Season s= (Season)JsonConvert.DeserializeObject(value);
            conn.AddSeason(s);
        }

        // PUT: api/Season/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Season s = (Season)JsonConvert.DeserializeObject(value);
            s.Id = id;
            conn.AddSeason(s);
        }

        // DELETE: api/Season/Delete/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DBConnect conn = new DBConnect();
            conn.DeleteSeason(id);
        }
    }
}
