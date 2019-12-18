using System;
using System.Collections.Generic;
using System.Linq;
using HomeStreamingServiceAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;


namespace HomeStreamingServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        // GET: api/Episode
        [HttpGet]
        public IEnumerable<string> Get()
        {
            DBConnect conn = new DBConnect();
            List<Episode> episodeList  = conn.GetEpisode();
            List<string> arr = new List<string>();
            foreach (var episode in episodeList)
            {
                arr.Add(JsonConvert.SerializeObject(episode));
            }

            return arr.ToArray();
        }

        // GET: api/Episode/5
        [HttpGet("{id}", Name = "GetEpisode")]
        public string Get(int id)
        {
            DBConnect conn = new DBConnect();
            List<Episode> episodeList = conn.GetEpisode();
            Episode e = null;
            foreach (var episode in episodeList)
            {
                if (episode.Id == id)
                {
                    e = episode;
                }
            }
            
            string output = JsonConvert.SerializeObject(e);
            return output;
        }

        // POST: api/Episode
        [HttpPost]
        public void Post([FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Episode e = (Episode) JsonConvert.DeserializeObject(value);
            conn.AddEpisode(e);
        }

        // PUT: api/Episode/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            DBConnect conn = new DBConnect();
            Episode e = (Episode)JsonConvert.DeserializeObject(value);
            e.Id = id;
            conn.AddEpisode(e);
        }

        // DELETE: api/Episode/Delete/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DBConnect conn = new DBConnect();
            conn.DeleteEpisode(id);
        }
    }
}
