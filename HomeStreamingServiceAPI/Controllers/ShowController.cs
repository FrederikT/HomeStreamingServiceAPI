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
    public class ShowController : ControllerBase
    {
        // GET: api/Show
        [HttpGet]
        public IEnumerable<string> Get()
        {

            DBConnect conn = new DBConnect();
            List<Adaptation> showList = conn.GetShow();
            List<string> arr = new List<string>();
            foreach (var show in showList)
            {
                arr.Add(JsonConvert.SerializeObject(show));
            }
            return arr.ToArray();
        }

        // GET: api/Show/5
        [HttpGet("{id}", Name = "GetShow")]
        public string Get(int id)
        {
            DBConnect conn = new DBConnect();
            List<Adaptation> showList = conn.GetShow();
            Adaptation s = null;
            foreach (var show in showList)
            {
                if (show.Id == id)
                {
                    s = show;
                }
            }

            string output = JsonConvert.SerializeObject(s);
            return output;
        }

        // POST: api/Show
        [HttpPost]
        public string Post()
        {
          
            DBConnect conn = new DBConnect();
            try
            {
                
                Adaptation adaptation = new Adaptation(-5, Request.Form["title"], "");
                if (Request.Form.ContainsKey("id"))
                {
                    adaptation.Id = int.Parse(Request.Form["id"]);
                }
                if (Request.Form.ContainsKey("originalTitle"))
                {
                    adaptation.OriginalTitle = Request.Form["originalTitle"];
                }
                if (Request.Form.ContainsKey("description"))
                {
                    adaptation.Description = Request.Form["description"];
                }
                if (Request.Form.ContainsKey("franchiseId"))
                {
                   adaptation.Franchise = conn.GetFranchise().Find(Franchise => Franchise.Id == int.Parse(Request.Form["franchiseId"]));
                }

                if (Request.Form.ContainsKey("genre"))
                {
                    // Array will be converted in string
                    // name;name;...
                    string genres = Request.Form["genre"];
                    string[] genreArr = genres.Split(';');
                    List<Genre> genreList = new List<Genre>();

                    foreach (var genre in genreArr)
                    {
                        genreList.Add(new Genre(genre));
                    }

                    adaptation.Genre = genreList;

                }


                conn.AddAdaptation(adaptation);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return "OK";

        }

        // PUT: api/Show/5
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
            Adaptation s = (Adaptation)JsonConvert.DeserializeObject(value);
            s.Id = id;
            conn.AddAdaptation(s);
            */
        }

        // DELETE: api/Show/Delete/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DBConnect conn = new DBConnect();
            conn.DeleteShow(id);
        }

       
    }
}
