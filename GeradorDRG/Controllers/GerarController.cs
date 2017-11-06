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
                var configuracao = _context.Configuracao.Include("Banco").Include("Sistema").FirstOrDefault();

                if (configuracao != null)
                {
                    if (configuracao.Banco.Nome == "Oracle" && configuracao.Sistema.Nome == "MV")
                    {
                        var xml = "";

                        LoteInternacao subReq = await BuscaXmL(model,configuracao.BancoURL);
                        xml = SerializeXML(subReq);

                        if (configuracao.UtilizaWebService == true)
                        {

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

        private static async Task<LoteInternacao> BuscaXmL(GerarXMLViewModel model,string URL)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                Convert.ToBase64String(
                                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                                string.Format("{0}:{1}", "user", "password"))));

                string DataInicio = model.DataInicio.ToString("yyyy-MM-dd");
                string DataFim = model.DataFim.ToString("yyyy-MM-dd");

                HttpResponseMessage response = await client.GetAsync($"api/GerarDados?DataInicio={DataInicio}&DataFim={DataFim}");
                if (response.IsSuccessStatusCode)
                {
                    HttpContent content = response.Content;

                    string result = await content.ReadAsStringAsync();

                    LoteInternacao l = Newtonsoft.Json.JsonConvert.DeserializeObject<LoteInternacao>(result);

                    return l;

                }

            }

            return new LoteInternacao();

        }
        //Retira os pacientes teste e prestador teste
        //Se paciente remove todo o atendimento
        public void Filtro(LoteInternacao subreq)
        {

            var medicosLista = _context.PrestadorTeste.Select(m => m.NomePrestador).ToList();
            var atendimentoLista = _context.PacienteTeste.Select(c => c.CodPaciente).ToList();
            var conteudoPacientes = subreq.Internacoes.Where(a => (atendimentoLista).Contains(a.NumeroRegistro)).ToList();

            for (int i = conteudoPacientes.Count - 1; i >= 0; i--)
            {
                conteudoPacientes.RemoveAt(i);
            }

            foreach (var i in subreq.Internacoes)
            {

                var conteudoMedicos = i.Medicos.Where(m => (medicosLista).Contains(m.Nome)).ToList();
                conteudoMedicos.RemoveAll(m => (medicosLista).Any());

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


            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://iagwebservice.sigquali.com.br/iagwebservice/enviaDadosPaciente");
            //byte[] bytes;
            //bytes = System.Text.Encoding.ASCII.GetBytes(xmlEnvio);
            //request.ContentType = "text/xml; encoding='utf-8'";
            //request.ContentLength = bytes.Length;
            //request.Method = "POST";
            //Stream requestStream = request.GetRequestStream();
            //requestStream.Write(bytes, 0, bytes.Length);
            //requestStream.Close();
            //HttpWebResponse response;
            //try
            //{

            //    response = (HttpWebResponse)request.GetResponse();
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        Stream responseStream = response.GetResponseStream();
            //        string responseStr = new StreamReader(responseStream).ReadToEnd();
            //        return responseStr;
            //    }
            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
            //return null;



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