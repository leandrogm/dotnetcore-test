using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet.UI.Enums
{
    public enum StatusEnum
    {
        [Description("Pendente")]
        Pendente,
        [Description("Em Execução")]
        EmExecucao,
        [Description("Finalizado")]
        Finalizado
    }
}
