using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Iphone14.Entities
{
    /// <summary> 
    /// Represents a tenant entity with essential details
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// Primary key for the Tenant 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Required field Code of the Tenant 
        /// </summary>
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// Name of the Tenant 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Address1 of the Tenant 
        /// </summary>
        public string? Address1 { get; set; }
        /// <summary>
        /// Address2 of the Tenant 
        /// </summary>
        public string? Address2 { get; set; }
        /// <summary>
        /// City of the Tenant 
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Pincode of the Tenant 
        /// </summary>
        public int? Pincode { get; set; }
        /// <summary>
        /// CreatedOn of the Tenant 
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Tenant 
        /// </summary>
        public Guid? CreatedBy { get; set; }
        /// <summary>
        /// UpdatedOn of the Tenant 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Tenant 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<UserInRole>? UserInRoles { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<UserToken>? UserTokens { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<RoleEntitlement>? RoleEntitlements { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<Entity>? Entitys { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<User>? Users { get; set; }
        /// <summary>
        /// Collection navigation property representing associated 
        /// </summary>
        public ICollection<Role>? Roles { get; set; }
    }
}