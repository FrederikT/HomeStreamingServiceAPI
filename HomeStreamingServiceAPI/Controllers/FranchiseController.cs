﻿using System;
using System.Collections.Generic;
using HomeStreamingServiceAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HomeStreamingServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FranchiseController : ControllerBase
    {
        // GET: api/Franchise
        [HttpGet]
        public IEnumerable<string> Get()
        {
            DBConnect conn = new DBConnect();
            List<Franchise> franchiseList = conn.GetFranchise();
            List<string> arr = new List<string>();
            foreach (var franchise in franchiseList)
            {
                arr.Add(JsonConvert.SerializeObject(franchise));
            }

            return arr.ToArray();
        }

        // GET: api/Franchise/5
        [HttpGet("{id}", Name = "GetFranchise")]
        public string Get(int id)
        {
            DBConnect conn = new DBConnect();
            List<Franchise> franchiseList = conn.GetFranchise();
            Franchise f = null;
            foreach (var franchise in franchiseList)
            {
                if (franchise.Id == id)
                {
                    f = franchise;
                }
            }

            string output = JsonConvert.SerializeObject(f);
            return output;
        }

        // POST: api/Franchise
        [HttpPost]
        public string Post()
        {

            DBConnect conn = new DBConnect();
            try
            {
                Franchise franchise = new Franchise(Request.Form["name"], -5);
                if (Request.Form.ContainsKey("id"))
                {
                    franchise.Id = int.Parse(Request.Form["id"]);
                }
                conn.AddFranchise(franchise);
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return "OK";

        }

        // PUT: api/Franchise/5
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
            Franchise f = (Franchise)JsonConvert.DeserializeObject(value);
            f.Id = id;
            conn.AddFranchise(f);
            */

        }

        // DELETE: api/Franchise/Delete/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            DBConnect conn = new DBConnect();
            conn.DeleteFranchise(id);
        }
    }
}
