namespace click_gas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Pedido")]
    public partial class Pedido
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pedido()
        {
            Pedido_tem_Servico = new HashSet<Pedido_tem_Servico>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        public double custoEstimado { get; set; }

        public double duracaoEstimada { get; set; }

        public bool estado { get; set; }

        [Required]
        [StringLength(50)]
        public string emailCliente { get; set; }

        [Required]
        [StringLength(50)]
        public string emailFuncionario { get; set; }

        [Column(TypeName = "date")]
        public DateTime data { get; set; }

        public double hora { get; set; }

        [StringLength(300)]
        public string observacoes { get; set; }

        public double? duracaoReal { get; set; }

        public double? custoReal { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Funcionario Funcionario { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido_tem_Servico> Pedido_tem_Servico { get; set; }
    }
}
