using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYMNETIC.Core.Models
{
    public class Notifications
    {
        [Key]
        public required string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Titulo { get; set; }
        public required string Mensagem { get; set; }
        public required int Tipo { get; set; }
        public required int UserId { get; set; } // Alterado para int

        [ForeignKey("UserId")]
        public required Customer User { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool Lida { get; set; } = false;
        public DateTime? DataLida { get; set; }
    }
}