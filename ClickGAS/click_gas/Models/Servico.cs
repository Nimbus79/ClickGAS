namespace click_gas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Servico")]
    public partial class Servico
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Servico()
        {
            Pedido_tem_Servico = new HashSet<Pedido_tem_Servico>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [StringLength(15)]
        public string tipo { get; set; }

        [Required]
        [StringLength(30)]
        public string descricao { get; set; }

        public double preco { get; set; }

        public double duracaoMedia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido_tem_Servico> Pedido_tem_Servico { get; set; }
    }
}
