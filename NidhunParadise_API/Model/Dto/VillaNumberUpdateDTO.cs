using System.ComponentModel.DataAnnotations;

namespace NidhunParadise_API.Model.Dto
{
    public class VillaNumberUpdateDTO
    {
        [Required]
        public int VillaNo { get; set; }
        [Required]
        public int VillaID { get; set; }
        public string SpecialDetails { get; set;}
    }
}
