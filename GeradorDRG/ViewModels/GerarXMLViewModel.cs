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
        public DateTime  DataInicio { get; set; }
        [Required]
        public DateTime DataFim { get; set; }
    }
}
