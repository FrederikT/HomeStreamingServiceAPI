﻿using System;
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
        public string Post()
        {

            DBConnect conn = new DBConnect();
            try
            {
                conn.AddGenre(Request.Form["genre"]);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return "OK";

        }

        // PUT: api/Genre/Action
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] string value)
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
            Genre g = (Genre)JsonConvert.DeserializeObject(value);
            g.Name = name;
            conn.AddGenre(g);*/

        }

        // DELETE: api/Genre/Delete/Action
        [HttpDelete("{name}")]
        public void Delete(string name)
        {
            DBConnect conn = new DBConnect();
            conn.DeleteGenre(name);
        }
    }
}
