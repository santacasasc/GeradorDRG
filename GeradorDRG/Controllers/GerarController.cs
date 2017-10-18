using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeradorDRG.Data;
using GeradorDRG.Models;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ProjetoDRG.Controllers
{
    public class GerarController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GerarController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            if (_context.Configuracao.FirstOrDefault() == null)
            {
                return RedirectToAction("Index", "Configuracoes");

            }
            else
            {
                return RedirectToAction("Home");
            }

        }

        public async Task<IActionResult> GeracaoXML()
        {
            LoteInternacao subReq = await BuscaXmL();
            XmlSerializer xsSubmit = new XmlSerializer(typeof(LoteInternacao));
            var xml = "";

            using (var sww = new StringWriter())
            {

                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);

                    xml = sww.ToString(); // Your XML

                }
            }

            return Content(xml);
        }


        private static async Task<LoteInternacao> BuscaXmL()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:53812/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(
                                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                string.Format("{0}:{1}", "user", "password"))));

                // New code:
                HttpResponseMessage response = await client.GetAsync("api/GerarDados/");
                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;

                    string result = await content.ReadAsStringAsync();

                    LoteInternacao l = Newtonsoft.Json.JsonConvert.DeserializeObject<LoteInternacao>(result);


                    return l;

                }

            }

            return null;
        }

    }
}