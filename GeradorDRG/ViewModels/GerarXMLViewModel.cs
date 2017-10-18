using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoDRG.ViewModels
{
    public class GerarXMLViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime  DataInicio { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DataFim { get; set; }
    }
}
