using System.ComponentModel.DataAnnotations;

namespace SunnyVilla_VallaAPI.Models.Dto
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required] //Validating 
        [MaxLength(30)] //Also validating that name can only contain 30 character
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public double SquarePerFeet { get; set; }
    }
}
