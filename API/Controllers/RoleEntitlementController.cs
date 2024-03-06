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
    /// Controller responsible for managing roleentitlement-related operations in the API.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for adding, retrieving, updating, and deleting roleentitlement information.
    /// </remarks>
    [Route("api/roleentitlement")]
    [Authorize]
    public class RoleEntitlementController : ControllerBase
    {
        private readonly Iphone14Context _context;

        public RoleEntitlementController(Iphone14Context context)
        {
            _context = context;
        }

        /// <summary>Adds a new roleentitlement to the database</summary>
        /// <param name="model">The roleentitlement data to be added</param>
        /// <returns>The result of the operation</returns>
        [HttpPost]
        [UserAuthorize("RoleEntitlement",Entitlements.Create)]
        public IActionResult Post([FromBody] RoleEntitlement model)
        {
            _context.RoleEntitlement.Add(model);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Retrieves a list of roleentitlements based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"Property": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <returns>The filtered list of roleentitlements</returns>
        [HttpGet]
        [UserAuthorize("RoleEntitlement",Entitlements.Read)]
        public IActionResult Get([FromQuery] string filters)
        {
            List<FilterCriteria> filterCriteria = null;
            if (!string.IsNullOrEmpty(filters))
            {
                filterCriteria = JsonHelper.Deserialize<List<FilterCriteria>>(filters);
            }

            var query = _context.RoleEntitlement.AsQueryable();
            var result = FilterService<RoleEntitlement>.ApplyFilter(query, filterCriteria);
            return Ok(result);
        }

        /// <summary>Retrieves a specific roleentitlement by its primary key</summary>
        /// <param name="entityId">The primary key of the roleentitlement</param>
        /// <returns>The roleentitlement data</returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [UserAuthorize("RoleEntitlement",Entitlements.Read)]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var entityData = _context.RoleEntitlement.FirstOrDefault(entity => entity.Id == id);
            return Ok(entityData);
        }

        /// <summary>Deletes a specific roleentitlement by its primary key</summary>
        /// <param name="entityId">The primary key of the roleentitlement</param>
        /// <returns>The result of the operation</returns>
        [HttpDelete]
        [UserAuthorize("RoleEntitlement",Entitlements.Delete)]
        [Route("{id:Guid}")]
        public IActionResult DeleteById([FromRoute] Guid id)
        {
            var entityData = _context.RoleEntitlement.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            _context.RoleEntitlement.Remove(entityData);
            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }

        /// <summary>Updates a specific roleentitlement by its primary key</summary>
        /// <param name="entityId">The primary key of the roleentitlement</param>
        /// <param name="updatedEntity">The roleentitlement data to be updated</param>
        /// <returns>The result of the operation</returns>
        [HttpPut]
        [UserAuthorize("RoleEntitlement",Entitlements.Update)]
        [Route("{id:Guid}")]
        public IActionResult UpdateById(Guid id, [FromBody] RoleEntitlement updatedEntity)
        {
            if (id != updatedEntity.Id)
            {
                return BadRequest("Mismatched Id");
            }

            var entityData = _context.RoleEntitlement.FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                return NotFound();
            }

            var propertiesToUpdate = typeof(RoleEntitlement).GetProperties().Where(property => property.Name != "Id").ToList();
            foreach (var property in propertiesToUpdate)
            {
                property.SetValue(entityData, property.GetValue(updatedEntity));
            }

            var returnData = this._context.SaveChanges();
            return Ok(returnData);
        }
    }
}