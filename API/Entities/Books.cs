using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Iphone14.Entities
{
    /// <summary> 
    /// Represents a books entity with essential details
    /// </summary>
    public class Books
    {
        /// <summary>
        /// Initializes a new instance of the Books class.
        /// </summary>
        public Books()
        {
            Price = 100;
            Quantity = 1;
        }

        /// <summary>
        /// Primary key for the Books 
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }
        /// <summary>
        /// Title of the Books 
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// Foreign key referencing the Author to which the Books belongs 
        /// </summary>
        public Guid? AuthorId { get; set; }

        /// <summary>
        /// Navigation property representing the associated Author
        /// </summary>
        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }
        /// <summary>
        /// Genre of the Books 
        /// </summary>
        public string? Genre { get; set; }
        /// <summary>
        /// Publication of the Books 
        /// </summary>
        public string? Publication { get; set; }
        /// <summary>
        /// PublishDate of the Books 
        /// </summary>
        public DateTime? PublishDate { get; set; }
        /// <summary>
        /// Price of the Books 
        /// </summary>
        public int? Price { get; set; }
        /// <summary>
        /// Quantity of the Books 
        /// </summary>
        public int? Quantity { get; set; }
        /// <summary>
        /// TenantId of the Books 
        /// </summary>
        public Guid? TenantId { get; set; }
        /// <summary>
        /// CreatedOn of the Books 
        /// </summary>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// CreatedBy of the Books 
        /// </summary>
        public Guid? CreatedBy { get; set; }
        /// <summary>
        /// UpdatedOn of the Books 
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
        /// <summary>
        /// UpdatedBy of the Books 
        /// </summary>
        public Guid? UpdatedBy { get; set; }
    }
}