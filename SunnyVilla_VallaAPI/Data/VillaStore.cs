using SunnyVilla_VallaAPI.Models.Dto;

namespace SunnyVilla_VallaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO{Id = 1, Name = "Lekki Lagoon View", SquarePerFeet = 100, Occupancy = 3},
                new VillaDTO {Id = 2, Name = "Ocean View", SquarePerFeet = 300, Occupancy = 4 }
            };
    }
}
