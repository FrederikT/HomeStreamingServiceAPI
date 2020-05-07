using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeStreamingServiceAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration;
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
        public string Post()
        {

            DBConnect conn = new DBConnect();
            try
            {
                Adaptation show = conn.GetShow().Find(Show => Show.Id == int.Parse(Request.Form["showId"]));
                Season season = new Season(-5, show, Request.Form["title"]);
                if (Request.Form.ContainsKey("id"))
                {
                    season.Id = int.Parse(Request.Form["id"]);
                }
                if (Request.Form.ContainsKey("originalTitle"))
                {
                    season.OriginalTitle = Request.Form["originalTitle"];
                }
                if (Request.Form.ContainsKey("description"))
                {
                    season.Description = Request.Form["description"];
                }
                conn.AddSeason(season);
            }
            catch (Exception exception)
            {          
                return exception.Message;
            }

            return "OK";

        }

        // PUT: api/Season/5
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
           /* DBConnect conn = new DBConnect();
            Season s = (Season)JsonConvert.DeserializeObject(value);
            s.Id = id;
            conn.AddSeason(s);*/
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
