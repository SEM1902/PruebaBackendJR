using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EnterpriseApi.Entities
{
    public class Code
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Navegación a la Empresa Dueña
        [ForeignKey("OwnerId")]
        // [JsonIgnore] para evitar ciclos infinitos de serialización al recuperar la empresa
        [JsonIgnore]
        public Enterprise? Owner { get; set; }
    }
}
