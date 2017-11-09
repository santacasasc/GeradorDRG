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
using System.Net;
using System.Web;
using GeradorDRG.Helpers;
using Microsoft.EntityFrameworkCore;
using GeradorDRG.Services;

namespace ProjetoDRG.Controllers
{
    public class GerarController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly Configuracao configuracao;

        public GerarController(ApplicationDbContext context)
        {
            _context = context;
            configuracao = _context.Configuracao.Include("Banco").Include("Sistema").FirstOrDefault();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GeracaoXML(GerarXMLViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (configuracao != null)
                {
                    if (configuracao.Banco.Nome == "Oracle" && configuracao.Sistema.Nome == "MV")
                    {
                        var xml = "";

                        LoteInternacao subReq = await BuscaXmL(model,configuracao.BancoURL);

                        Filtro(subReq,configuracao.CodDRG,configuracao.NomeDRG);

                        xml = SerializeXML(subReq);

                        if (configuracao.UtilizaWebService == true)
                        {
                            ViewBag.UtilizaWebService = true;

                            var usuario = configuracao.WebServiceUsuario;
                            var senha = configuracao.WebServiceSenha;

                            var response = await EnviarXmlWebService(xml, usuario, senha);

                            return View("Index", model);
                        }
                        else
                        {
                            return File(Encoding.UTF8.GetBytes(xml), "text/xml", "repasse.xml");
                        }
                    }

                }


            }

            return View("Index", model);

        }

        private static string SerializeXML(LoteInternacao subReq)
        {
            string xml;

            XmlSerializer xsSubmit = new XmlSerializer(typeof(LoteInternacao));

            using (var sww = new Utf8StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                //Add an empty namespace and empty value
                ns.Add("", "");


                var settings = new XmlWriterSettings
                {
                    Encoding = Encoding.UTF8,
                    Indent = true,
                    OmitXmlDeclaration = true
                };

                using (XmlWriter writer = XmlWriter.Create(sww, settings))
                {
                    xsSubmit.Serialize(writer, subReq, ns);

                    xml = sww.ToString(); // Your XML

                }

            }

            return xml;
        }

        private static async Task<LoteInternacao> BuscaXmL(GerarXMLViewModel model,string URLBase)
        {

            string DataInicio = model.DataInicio.ToString("yyyy-MM-dd");
            string DataFim = model.DataFim.ToString("yyyy-MM-dd");

            string URL = $"api/GerarDados?DataInicio={DataInicio}&DataFim={DataFim}";

            return await WebApiService.GetResponseAsync<LoteInternacao>(URLBase,URL);

        }
        //Retira os pacientes teste e prestador teste
        //Se paciente remove todo o atendimento
        public void Filtro(LoteInternacao subreq,string CodDRG, string NomeDRG)
        {
            var prestadoresTeste = configuracao.Prestadores.Select(m => m.CodPrestador).ToList();
            var PacientesTeste = configuracao.Pacientes.Select(c => c.CodPaciente).ToList();
            var conteudoPacientes = subreq.Internacoes.Where(a => (PacientesTeste).Contains(a.NumeroRegistro)).ToList();

            for (int i = conteudoPacientes.Count - 1; i >= 0; i--)
            {
                conteudoPacientes.RemoveAt(i);
            }

            foreach (var i in subreq.Internacoes)
            {
                var alta = configuracao.MotivosAlta.Where(m => m.CodigoMotivo == i.CdMotAlt).FirstOrDefault();

                if(alta == null)
                {
                    i.CondicaoAlta = "X";
                }
                else
                {
                    i.CondicaoAlta = alta.TipoDRG;
                }

                var internacao = configuracao.TiposInternacao.Where(m => m.CodigoTipo == i.CdTipoInternacao).FirstOrDefault();

                if (internacao == null)
                {
                    i.CaraterInternacao = "X";
                }
                else
                {
                    i.CaraterInternacao = internacao.TipoDRG;
                }

                i.Hospital.Codigo = CodDRG;
                i.Hospital.Nome = NomeDRG;

                var conteudoMedicos = i.Medicos.Where(m => (prestadoresTeste).Contains(m.Nome)).ToList();
                conteudoMedicos.RemoveAll(m => (prestadoresTeste).Any());

            }
        }

        private async Task<string> EnviarXmlWebService(string xml, string usuario, string senha)
        {

            string xmlEnvio;

            xmlEnvio = SOAP(xml, senha, usuario);

            using (var client = new HttpClient())
            {
                var content = new StringContent(xmlEnvio, Encoding.UTF8, "text/xml"); ;

                var response = await client.PostAsync("https://iagwebservice.sigquali.com.br/iagwebservice/enviaDadosPaciente", content);

                if(response.StatusCode == HttpStatusCode.OK)
                {
                    ViewBag.Mensagem = "OK";
                }

                return await response.Content.ReadAsStringAsync();
            }

        }

        private static string SOAP(string xml, string senha, string usuario)
        {
            return $@"<?xml version=""1.0"" encoding=""UTF-8""?><soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ser=""http://service.iagwebservice.sigquali.com.br/"">
                <soapenv:Header/>
                <soapenv:Body>
                <ser:enviaDadosInternacao>
                <!--Optional:-->
                <xml><![CDATA[
                {xml}
                ]]></xml>
                <usuarioIAG>{usuario}</usuarioIAG>
                <senhaIAG>{senha}</senhaIAG>
                </ser:enviaDadosInternacao>
                </soapenv:Body>
                </soapenv:Envelope>";
        }

    }
}