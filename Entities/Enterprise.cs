using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnterpriseApi.Entities
{
    public class Enterprise
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public long? Nit { get; set; }

        [Required]
        public long Gln { get; set; }

        // Navegación (Relación Uno a Muchos)
        public ICollection<Code> Codes { get; set; } = new List<Code>();
    }
}
