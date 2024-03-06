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
    /// Controller responsible for managing role-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting role information.
    /// </remarks>
    [Route("api/role")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly Iphone14Context _context;

        public RoleController(Iphone14Context context)
        {
            _context = context;
        }

        /// <summary>Adds a new role to the database</summary>
        /// <param name="model">The role data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("Role",Entitlements.Create)]
        public IActionResult Post([FromBody] Role model)
        {
            _context.Role.Add(model);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Retrieves a list of roles based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"Property": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <returns>The filtered list of roles</returns>
        [HttpGet]
        [UserAuthorize("Role",Entitlements.Read)]
        public IActionResult Get([FromQuery] string filters)
        {
            List<FilterCriteria> filterCriteria = null;
            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.Role.AsQueryable();
            var result = FilterService<Role>.ApplyFilter(query, filterCriteria);
            return Ok(result);
        }

        /// <summary>Retrieves a specific role by its primary key</summary>
        /// <param name="entityId">The primary key of the role</param>
        /// <returns>The role data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("Role",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.Role.FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific role by its primary key</summary>
        /// <param name="entityId">The primary key of the role</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("Role",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.Role.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.Role.Remove(entityData);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Updates a specific role by its primary key</summary>
        /// <param name="entityId">The primary key of the role</param>
        /// <param name="updatedEntity">The role data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("Role",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] Role updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var entityData = _context.Role.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            var propertiesToUpdate = typeof(Role).GetProperties().Where(property => property.Name != "Id").ToList();
            foreach (var property in propertiesToUpdate)
            {
                property.SetValue(entityData, property.GetValue(updatedEntity));
            }

            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }
    }
}