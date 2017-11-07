using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorDRG.Helpers
{
    public static class TipoDRG
    {
        public static List<SelectListItem> AltaTipos = new List<SelectListItem>
        {
            new SelectListItem() { Text = "Selecione o Tipo de Alta"},
            new SelectListItem() { Text = "Casa/Auto-Cuidado", Value = "A" },
            new SelectListItem() { Text = "Instituição de longa permanência", Value = "I" },
            new SelectListItem() { Text = "Atenção domiciliar", Value = "D" },
            new SelectListItem() { Text = "Transferido para hospital de curta permanência", Value = "C" },
            new SelectListItem() { Text = "Transferido para hospital de longa permanência (unidade de cuidados crônicos, reabilitação)", Value = "L" },
            new SelectListItem() { Text = "Óbito", Value = "O" },
            new SelectListItem() { Text = "Evasão", Value = "E" }
        };

        public static List<SelectListItem> InternacaoTipos = new List<SelectListItem>
        {
            new SelectListItem() { Text = "Selecione o Tipo de Internação"},
            new SelectListItem() { Text = "Eletivo", Value = "1" },
            new SelectListItem() { Text = "Urgência", Value = "2" },
            new SelectListItem() { Text = "Emergência", Value = "3" },
            new SelectListItem() { Text = "Trauma", Value = "4" },
            new SelectListItem() { Text = "Não Definido", Value = "9" }
        };
    }
}
