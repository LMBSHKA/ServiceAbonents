using System.ComponentModel.DataAnnotations;

namespace ServiceAbonents.Models
{
    public class BaseEntity
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
        public DateTimeOffset DateDeleted { get; set; }

        protected BaseEntity() => Id = Guid.NewGuid();
    }
}
