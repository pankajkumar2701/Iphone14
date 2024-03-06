using Microsoft.AspNetCore.Mvc;
using Iphone14.Models;
using Iphone14.Data;
using Iphone14.Filter;
using Iphone14.Entities;
using Iphone14.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Iphone14.Controllers
{
    /// <summary>
    /// Controller responsible for managing books-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting books information.
    /// </remarks>
    [Route("api/books")]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly Iphone14Context _context;

        public BooksController(Iphone14Context context)
        {
            _context = context;
        }

        /// <summary>Adds a new books to the database</summary>
        /// <param name="model">The books data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Books",Entitlements.Create)]
        public IActionResult Post([FromBody] Books model)
        {
            _context.Books.Add(model);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Retrieves a list of bookss based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"Property": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <returns>The filtered list of bookss</returns>
        [HttpGet]
        [UserAuthorize("Books",Entitlements.Read)]
        public IActionResult Get([FromQuery] string filters)
        {
            List<FilterCriteria> filterCriteria = null;
            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.Books.AsQueryable();
            var result = FilterService<Books>.ApplyFilter(query, filterCriteria);
            return Ok(result);
        }

        /// <summary>Retrieves a specific books by its primary key</summary>
        /// <param name="entityId">The primary key of the books</param>
        /// <returns>The books data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Books",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.Books.FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific books by its primary key</summary>
        /// <param name="entityId">The primary key of the books</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Books",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.Books.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.Books.Remove(entityData);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Updates a specific books by its primary key</summary>
        /// <param name="entityId">The primary key of the books</param>
        /// <param name="updatedEntity">The books data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Books",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] Books updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var entityData = _context.Books.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            var propertiesToUpdate = typeof(Books).GetProperties().Where(property => property.Name != "Id").ToList();
            foreach (var property in propertiesToUpdate)
            {
                property.SetValue(entityData, property.GetValue(updatedEntity));
            }

            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }
    }
}