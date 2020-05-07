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
        public string Post()
        {
            DBConnect conn = new DBConnect();
            try
            {
                Season season = conn.GetSeason().Find(Season => Season.Id == int.Parse(Request.Form["seasonId"]));
                Episode episode = new Episode(-5, season, Request.Form["title"],int.Parse(Request.Form["duration"]));
                if (Request.Form.ContainsKey("id"))
                {
                    episode.Id = int.Parse(Request.Form["id"]);
                }
                if (Request.Form.ContainsKey("originalTitle"))
                {
                    episode.OriginalTitle = Request.Form["originalTitle"];
                }
                if (Request.Form.ContainsKey("description"))
                {
                    episode.Description = Request.Form["description"];
                }

                if (Request.Form.ContainsKey("filePath"))
                {
                    episode.FilePath = Request.Form["filePath"];

                }
                conn.AddEpisode(episode);                               
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return "OK";

        }

        // PUT: api/Episode/5
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
            Episode e = (Episode)JsonConvert.DeserializeObject(value);
            e.Id = id;
            conn.AddEpisode(e);*/
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
