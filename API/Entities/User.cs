using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Iphone14.Entities
{
    /// <summary> 
    /// Represents a user entity with essential details
    /// </summary>
    public class User
    {
        /// <summary>
        /// Foreign key referencing the Tenant to which the User belongs 
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Tenant
        /// </summary>
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Primary key for the User 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the User 
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Required field EmailId of the User 
        /// </summary>
        [Required]
        public string EmailId { get; set; }

        /// <summary>
        /// Required field UserName of the User 
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Required field PasswordHash of the User 
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        /// <summary>
        /// Required field Saltkey of the User 
        /// </summary>
        [Required]
        public string Saltkey { get; set; }
        /// <summary>
        /// DOB of the User 
        /// </summary>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Required field IsSuperAdmin of the User 
        /// </summary>
        [Required]
        public bool IsSuperAdmin { get; set; }
        /// <summary>
        /// CreatedOn of the User 
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the User 
        /// </summary>
        public Guid? CreatedBy { get; set; }
        /// <summary>
        /// UpdatedOn of the User 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the User 
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
        public ICollection<Role>? Roles { get; set; }
    }
}