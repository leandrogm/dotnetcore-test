using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet.UI.Models
{
    public class Tipo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Int32 Id { get; set; }
        public String Descricao { get; set; }
        public Double Valor { get; set; }

        public virtual ICollection<Servico> Servicos { get; set; }
    }
}
