using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReviewApp.Models
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int databaseId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
