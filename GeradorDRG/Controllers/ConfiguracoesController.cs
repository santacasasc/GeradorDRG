using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeradorDRG.Data;
using GeradorDRG.Models;
using System.Xml.Serialization;
using System.IO;
using System.Xml;


namespace GeradorDRG.Controllers
{
    public class ConfiguracoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Configuracoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Configuracao.Include(c => c.Banco).Include(c => c.Sistema);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Configuracoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracao = await _context.Configuracao
                .Include(c => c.Banco)
                .Include(c => c.Sistema)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (configuracao == null)
            {
                return NotFound();
            }

            return View(configuracao);
        }

        // GET: Configuracoes/Create
        public IActionResult Create()
        {
            ViewData["BancoId"] = new SelectList(_context.Banco, "Id", "Nome");
            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome");
            return View();
        }

        // POST: Configuracoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BancoId,SistemaId,BancoURL,BancoUsuario,BancoSenha,BancoSID,CodDRG,NomeDRG,UtilizaWebService,WebServiceUsuario,WebServiceSenha")] Configuracao configuracao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(configuracao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BancoId"] = new SelectList(_context.Banco, "Id", "Nome", configuracao.BancoId);
            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome", configuracao.SistemaId);
            return View(configuracao);
        }


		public ActionResult ControlePaginas(IList<Banco> banco, IList<Sistema> sistemas)
		{
			if (_context.Configuracao.FirstOrDefault() == null)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return RedirectToAction("Home");
			}

			return View(banco);
		}

		public ActionResult GeracaoXML2()
		{
			LoteInternacao subReq = BuscaXmL();
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

		/*
				public FileResult DownloadXml()
				{
					 FileResult xml = new FileResult();

					return xml;
				}

		*/
		private static LoteInternacao BuscaXmL()
		{
			LoteInternacao l = new LoteInternacao();
			var teste = new LoteInternacao.Internacao();


			teste.Situacao = "3";
			teste.CaraterInternacao = "1";
			teste.NumeroAtendimento = "2";
			teste.DataInternacao = DateTime.Now;

			teste.Medicos.Add(new LoteInternacao.Internacao.Medico { Nome = "Joao", Crm = "12_566" });

			teste.Beneficiarios.Codigo = 1;
			teste.Beneficiarios.Nome = "Jose";

			teste.Operadoras.Codigo = 2;
			teste.Operadoras.Nome = "DRM";


			l.Internacoes.Add(teste);
			return l;
		}


		// GET: Configuracoes/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracao = await _context.Configuracao.SingleOrDefaultAsync(m => m.Id == id);
            if (configuracao == null)
            {
                return NotFound();
            }
            ViewData["BancoId"] = new SelectList(_context.Banco, "Id", "Nome", configuracao.BancoId);
            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome", configuracao.SistemaId);
            return View(configuracao);
        }

        // POST: Configuracoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BancoId,SistemaId,BancoURL,BancoUsuario,BancoSenha,BancoSID,CodDRG,NomeDRG,UtilizaWebService,WebServiceUsuario,WebServiceSenha")] Configuracao configuracao)
        {
            if (id != configuracao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(configuracao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConfiguracaoExists(configuracao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BancoId"] = new SelectList(_context.Banco, "Id", "Nome", configuracao.BancoId);
            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome", configuracao.SistemaId);
            return View(configuracao);
        }

        // GET: Configuracoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var configuracao = await _context.Configuracao
                .Include(c => c.Banco)
                .Include(c => c.Sistema)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (configuracao == null)
            {
                return NotFound();
            }

            return View(configuracao);
        }

        // POST: Configuracoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var configuracao = await _context.Configuracao.SingleOrDefaultAsync(m => m.Id == id);
            _context.Configuracao.Remove(configuracao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConfiguracaoExists(int id)
        {
            return _context.Configuracao.Any(e => e.Id == id);
        }
    }
}
