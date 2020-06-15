using AutoMapper.Execution;
using DotNet.UI.Enums;
using DotNet.UI.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DotNet.UI.Models
{
    public class Servico
    {
        public Guid Id { get; set; }
        public String Cliente { get; set; }
        public DateTime DataAgendamento { get; set; }
        public String Observacao { get; set; }
        public Int32 TipoId { get; set; }

        public Tipo Tipo { get; set; }
        public ApplicationUser User { get; set; }
        public String UserId { get; set; }

        public StatusEnum Status { get; set; }

        [NotMapped]
        public String NomeStatus { get { return EnumUtil.GetDescription(Status); } }


    }


}
