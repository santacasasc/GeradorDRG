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
				var xml = "";
				LoteInternacao subReq = await BuscaXmL(model);
				xml = SerializeXML(subReq);
				return File(Encoding.UTF8.GetBytes(xml), "text/xml", "repasse.xml");
			}

			return null;

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
					Indent = true
				};

				using (XmlWriter writer = XmlWriter.Create(sww, settings))
				{
					xsSubmit.Serialize(writer, subReq, ns);

					xml = sww.ToString(); // Your XML

				}


			}

			return xml;
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

		public  async void  EnviarXmlWebService()
		{
			var xml = "";
			var model = new GerarXMLViewModel { DataInicio=DateTime.Now.AddDays(-10), DataFim=DateTime.Now};
			LoteInternacao subReq = await BuscaXmL(model);
			xml = SerializeXML(subReq);
			
						HttpWebRequest request = HttpWebRequest.Create("https://iagwebservice.sigquali.com.br/iagwebservice/enviaDadosPaciente?usuarioIAG=357_FEHosp-T&senhaIAG=ewH69$1") as HttpWebRequest;
						request.Method = "POST";
						request.ContentType = "application/x-www-form-urlencoded";

						Encoding e = Encoding.GetEncoding("iso-8859-1");
						XmlDocument doc = new XmlDocument();
						doc.LoadXml(xml);
						string rawXml = doc.OuterXml;

						string requestText = string.Format("enviaDadosPaciente={0}", HttpUtility.UrlEncode(rawXml,e));

						Stream requestStream = request.GetRequestStream();
						StreamWriter requestWriter = new StreamWriter(requestStream, e);
						requestWriter.Write(requestText);
						requestWriter.Close();
						
			/*HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://iagwebservice.sigquali.com.br/iagwebservice/enviaDadosPaciente?usuarioIAG=357_FEHosp-T&senhaIAG=ewH69$1");
			byte[] bytes;
			bytes = System.Text.Encoding.ASCII.GetBytes(xml);
			request.ContentType = "text/xml; encoding='utf-8'" ;
			request.ContentLength = bytes.Length;
			request.Method = "POST";
			Stream requestStream = request.GetRequestStream();
			requestStream.Write(bytes,0,bytes.Length);
			requestStream.Close();
			HttpWebResponse response;
			response = (HttpWebResponse)request.GetResponse();
			if (response.StatusCode==HttpStatusCode.OK)
			{
				Stream responseStream = response.GetResponseStream();
				string responseStr = new StreamReader(responseStream).ReadToEnd();
				return responseStr;

			}
			return null;
			*/
		

			
		}



	}
}