using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElevenNote.Data.Entities
{
    public class NoteEntity
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [ForeignKey(nameof(Owner))]
        public int OwnerID { get; set; }
        public UserEntity? Owner { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public DateTimeOffset CreatedUtc { get; set; }
        public DateTimeOffset? ModifiedUtc { get; set; }
    }
}