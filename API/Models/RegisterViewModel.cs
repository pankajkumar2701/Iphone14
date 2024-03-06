using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Iphone14.Models
{
    /// <summary> 
    /// Represents a registerviewmodel entity with essential details
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// Required field Name of the RegisterViewModel 
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Required field UserName of the RegisterViewModel 
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Required field Email of the RegisterViewModel 
        /// </summary>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Required field Password of the RegisterViewModel 
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Required field ConfirmPassword of the RegisterViewModel 
        /// </summary>
        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Required field TenantId of the RegisterViewModel 
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }
    }
}