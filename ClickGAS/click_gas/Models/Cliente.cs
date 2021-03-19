namespace click_gas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cliente")]
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            Pedidoes = new HashSet<Pedido>();
        }

        public int contribuinte { get; set; }

        [Required]
        [StringLength(70)]
        public string nome { get; set; }

        [Required]
        [StringLength(40)]
        public string palavraPasse { get; set; }

        [Key]
        [StringLength(50)]
        public string email { get; set; }

        public int contacto { get; set; }

        [Column(TypeName = "date")]
        public DateTime dataNasc { get; set; }

        [Required]
        [StringLength(70)]
        public string rua { get; set; }

        public int numero { get; set; }

        [Required]
        [StringLength(8)]
        public string codPostal { get; set; }

        [Required]
        [StringLength(20)]
        public string freguesia { get; set; }

        [Required]
        [StringLength(20)]
        public string concelho { get; set; }

        [StringLength(10)]
        public string role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> Pedidoes { get; set; }
    }
}
