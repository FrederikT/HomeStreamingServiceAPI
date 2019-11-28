using System;
using System.Collections.Generic;
using System.Linq;
using HomeStreamingServiceAPI.Model;
using Microsoft.AspNetCore.Mvc;
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
            List<MetaData> list = conn.GetMetaData();
            List<string> arr = new List<string>();
            foreach (var meta in list)
            {
                arr.Add(meta.Title);
            }

            return arr.ToArray();
        }

        // GET: api/Episode/5
        [HttpGet("{id}", Name = "GetEpisode")]
        public string Get(int id)
        {

            Episode e = null;
            string output = JsonConvert.SerializeObject(e);
            return output;
        }

        // POST: api/Episode
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Episode/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
