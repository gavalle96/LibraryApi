using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LibraryApi.Models;
using System.Web.Http.Cors;

namespace LibraryApi.Controllers
{
    [EnableCors(origins: "https://ulibrary.azurewebsites.net,http://localhost:4200", headers: "*", methods: "*")]
    public class CheckOutsController : ApiController
    {
        private MyDbContext db = new MyDbContext();

        // GET: api/CheckOuts
        public IQueryable<CheckOut> GetCheckouts()
        {
            return db.Checkouts;
        }

        // GET: api/CheckOuts/5
        [ResponseType(typeof(CheckOut))]
        public IHttpActionResult GetCheckOut(int id)
        {
            CheckOut checkOut = db.Checkouts.Find(id);
            if (checkOut == null)
            {
                return NotFound();
            }

            return Ok(checkOut);
        }

        // PUT: api/CheckOuts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCheckOut(int id, CheckOut checkOut)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != checkOut.Id)
            {
                return BadRequest();
            }

            db.Entry(checkOut).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CheckOutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/CheckOuts
        [ResponseType(typeof(CheckOut))]
        public IHttpActionResult PostCheckOut(CheckOut checkOut)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Checkouts.Add(checkOut);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = checkOut.Id }, checkOut);
        }

        // DELETE: api/CheckOuts/5
        [ResponseType(typeof(CheckOut))]
        public IHttpActionResult DeleteCheckOut(int id)
        {
            CheckOut checkOut = db.Checkouts.Find(id);
            if (checkOut == null)
            {
                return NotFound();
            }

            db.Checkouts.Remove(checkOut);
            db.SaveChanges();

            return Ok(checkOut);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CheckOutExists(int id)
        {
            return db.Checkouts.Count(e => e.Id == id) > 0;
        }
    }
}