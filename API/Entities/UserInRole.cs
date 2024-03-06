using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Iphone14.Entities
{
    /// <summary> 
    /// Represents a userinrole entity with essential details
    /// </summary>
    public class UserInRole
    {
        /// <summary>
        /// Foreign key referencing the Tenant to which the UserInRole belongs 
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Tenant
        /// </summary>
        [ForeignKey("TenantId")]
        public Tenant? Tenant { get; set; }

        /// <summary>
        /// Primary key for the UserInRole 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Foreign key referencing the Role to which the UserInRole belongs 
        /// </summary>
        public Guid? RoleId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Role
        /// </summary>
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the UserInRole belongs 
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("UserId")]
        public User? User { get; set; }
        /// <summary>
        /// CreatedOn of the UserInRole 
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the UserInRole belongs 
        /// </summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }
        /// <summary>
        /// UpdatedOn of the UserInRole 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// Foreign key referencing the User to which the UserInRole belongs 
        /// </summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property representing the associated User
        /// </summary>
        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }
    }
}