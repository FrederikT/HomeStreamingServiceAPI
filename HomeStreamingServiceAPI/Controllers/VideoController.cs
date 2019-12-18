﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeStreamingServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {

        // return all episodes and movies??
        // GET: api/Video
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Video/5
        [HttpGet("{id}", Name = "GetVideo")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Video
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Video/5
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
