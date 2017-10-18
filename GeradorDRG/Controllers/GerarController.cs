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
using System.Text;
using ProjetoDRG.ViewModels;

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
            return View();
        }

        public async Task<IActionResult> GeracaoXML(GerarXMLViewModel model)
        {
            if (ModelState.IsValid)
            {
                LoteInternacao subReq = await BuscaXmL(model);
                XmlSerializer xsSubmit = new XmlSerializer(typeof(LoteInternacao));
                var xml = "";

                using (var sww = new Utf8StringWriter())
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                    //Add an empty namespace and empty value
                    ns.Add("", "");


                    var settings = new XmlWriterSettings
                    {
                        Encoding = Encoding.UTF8,
                        Indent = true
                    };

                    using (XmlWriter writer = XmlWriter.Create(sww, settings))
                    {
                        xsSubmit.Serialize(writer, subReq, ns);

                        xml = sww.ToString(); // Your XML

                    }

                    return File(Encoding.UTF8.GetBytes(xml), "text/xml", "repasse.xml");
                } 
            }

            return null;

        }

        public sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }

        private static async Task<LoteInternacao> BuscaXmL(GerarXMLViewModel model)
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

                string DataInicio = model.DataInicio.ToString("yyyy-MM-dd");
                string DataFim = model.DataFim.ToString("yyyy-MM-dd");
                // New code:
                HttpResponseMessage response = await client.GetAsync($"api/GerarDados?DataInicio={DataInicio}&DataFim={DataFim}");
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