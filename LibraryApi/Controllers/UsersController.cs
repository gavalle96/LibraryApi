using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using LibraryApi.Models;


namespace LibraryApi.Controllers
{
    [EnableCors(origins: "https://ulibrary.azurewebsites.net", headers: "*", methods: "*")]
    public class UsersController : ApiController
    {
        private MyDbContext db = new MyDbContext();
        // POST: api/Login
        [HttpPost]
        public Reply Login([FromBody] AccessViewModel model)
        {
            Reply reply = new Reply();
            try
            {
                using (MyDbContext db = new MyDbContext())
                {
                    var lst = db.Users.Where(x => x.Email == model.Email && x.Password == model.Password);

                    if (lst.Count() == 1)
                    {
                        reply.result = 1;
                        reply.data = Guid.NewGuid().ToString();
                    }
                    else
                    {
                        reply.message = "Datos incorrectos / Incorrect data";
                    }
                }
            }
            catch (Exception ex)
            {

                reply.message = "Error";
            }
            return reply;
        }

        [HttpGet]
        public Reply isEmailValid(int id, string email)
        {
            Reply reply = new Reply();
            try
            {
                using (MyDbContext db = new MyDbContext())
                {
                    var lst = db.Users.Where(x => x.Email == email && x.Id != id);

                    if (lst.Count() > 0)
                    {
                        reply.result = 1;
                        reply.data = false;
                        reply.message = "El Email no es valido / Email is not valid";
                    }
                    else
                    {
                        reply.result = 1;
                        reply.data = true;
                    }
                }
            }
            catch (Exception ex)
            {

                reply.message = "Error";
            }
            return reply;
        }

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            //validar que no se repita el correo con otro usuario
            using (MyDbContext context = new MyDbContext())
            {
                var usersSameEmail = context.Users.Where(x => x.Id != id && x.Email == user.Email);
                if (usersSameEmail.Any())
                {
                    return BadRequest();
                }
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}