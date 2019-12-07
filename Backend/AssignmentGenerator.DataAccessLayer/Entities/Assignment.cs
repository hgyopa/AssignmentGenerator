namespace AssignmentGenerator.DataAccessLayer.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Assignment : Entity
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreationDate { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
