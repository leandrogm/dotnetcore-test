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
    public class Tag
    {
        public Int32 Id { get; set; }
        public String Key { get; set; }
        public String Value { get; set; }
    }


}
