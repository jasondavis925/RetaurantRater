using RetaurantRater.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RetaurantRater.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();
        // POST (create)
        // api/Reataurant
        [HttpPost]
        public async Task<IHttpActionResult> CreateRestaurant([FromBody] Restaurant model)
        {
            if (model is null)
            {
                return BadRequest("Your request body cannot be empty.");
            }

            // If the model is valid
            if (ModelState.IsValid)
            {
                // Store the model in the database

                _context.Restaurants.Add(model);
                int changeCount = await _context.SaveChangesAsync();
                return Ok("Your restaurant was created!");
            }

            // The model is not valide, go ahead and reject it
            return BadRequest(ModelState);
        }

        // GET ALL
        // api/Restauraunt
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> restaurants = await _context.Restaurants.ToListAsync();
            return Ok(restaurants);
        }

        // GET BY ID
        // api/Restaurant/{id}
        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant != null)
            {
                return Ok(restaurant);
            }

            return NotFound();
        }

        // PUT (update)
        // api/Restaurant/{id}
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri] int id, [FromBody] Restaurant updatedRestaurant)
        {
            // Check the ids if they match
            if(id != updatedRestaurant.Id)
            {
                return BadRequest("Ids do not match.");
            }

            // Check the ModelIsState
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Find the restaurant in the database
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            // If the restaurant does not exist then do something
            if (restaurant is null)
                return NotFound();

            // Update the properties
            restaurant.Name = updatedRestaurant.Name;
            restaurant.Address = updatedRestaurant.Address;
            //restaurant.Rating = updatedRestaurant.Rating;

            // Save the changes
            await _context.SaveChangesAsync();

            return Ok("The restaurant was updated!");
        }

        // DELETE (delete)
        // api/Restaurant/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurant([FromUriAttribute] int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant is null)
                return NotFound();

            _context.Restaurants.Remove(restaurant);

            if (await _context.SaveChangesAsync() == 1)
            {
                return Ok("The restaurant was deleted.");
            }

            return InternalServerError();
        }
    }
}
