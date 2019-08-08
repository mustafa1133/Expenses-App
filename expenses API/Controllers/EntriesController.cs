using expenses_API.Data;
using expenses_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace expenses_API.Controllers
{
    [EnableCors("*","*","*")] // opens our controller to be used by 3rd party applications Angular
    public class EntriesController : ApiController
    {

        public IHttpActionResult GetEntry(int id)
        {
            try
            {
                // try block to make sure we can connect ot a database
                using (var context = new AppDbContext()) // using The DbContext class in the entity framework to connect the entities to the databse
                                                         // key word using automatically implements Idispoable to rid of an object ( rather then use a try catch, finally block)
                {
                    var entry = context.Entries.FirstOrDefault(n=>n.Id == id); // gets only 1 entry in the database
                    if (entry == null) return NotFound();
                    return Ok(entry); // ok method  returns the entrires to the users ok method return http respon 200 which means success
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // if connection fails
            }

        }
        public IHttpActionResult GetEntries()
        {
            try
            {
                // try block to make sure we can connect ot a database
                using (var context = new AppDbContext()) // using The DbContext class in the entity framework to connect the entities to the databse
                                                         // key word using automatically implements Idispoable to rid of an object ( rather then use a try catch, finally block)
                {
                    var entries = context.Entries.ToList(); // gets all the entries in the database
                    return Ok(entries); // ok method  returns the entrires to the users ok method return http respon 200 which means success
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); // if connection fails
            }

        }
        [HttpPost]
        public IHttpActionResult PostEntry([FromBody] Entry entry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            try
            {
                using (var context = new AppDbContext())
                {
                    context.Entries.Add(entry);
                    context.SaveChanges();

                    return Ok("Entry was created");
                }

            }
            catch (Exception ex)
            { 

                return BadRequest(ex.Message);
            }


        }

        [HttpPut]
        public IHttpActionResult UpdateEntry(int id, [FromBody]Entry entry)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != entry.Id) return BadRequest();


            try
            {
                using (var context = new AppDbContext())
                {
                    var oldEntry = context.Entries.FirstOrDefault(n => n.Id == id);
                    if (oldEntry == null) return NotFound();

                    oldEntry.Description = entry.Description;
                    oldEntry.IsExpense = entry.IsExpense;
                    oldEntry.Value = entry.Value;

                    context.SaveChanges();
                    return Ok("Entry Updated");

                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

            [HttpDelete]
        public IHttpActionResult DeleteEntry(int id)
            {
            try
            {
                using (var context= new AppDbContext())
                {
                    var entry = context.Entries.FirstOrDefault(n => n.Id == id);
                    if (entry == null) return NotFound(); // if entry is nulla nd not found return an exception
                    context.Entries.Remove(entry);
                    context.SaveChanges();

                    return Ok("Entry deleted");

                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            }
           
    }

}
