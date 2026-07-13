using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }
        [DisplayName("Display order")]
        [Range(1,100,ErrorMessage ="Display order must be btwn 1 and 100")]
        public int DisplayOrder { get; set; }

    }
}
