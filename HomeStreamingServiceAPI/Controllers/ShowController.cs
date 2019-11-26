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
            return new string[] { "value1", "value2" };
        }

        // GET: api/Show/5
        [HttpGet("{id}", Name = "GetShow")]
        public string Get(int id)
        {
           Adaptation a = new Adaptation(5, "seven deadly sins","nanatsu no taizai");
           return a.ToString();
        }

        // POST: api/Show
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Show/5
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
