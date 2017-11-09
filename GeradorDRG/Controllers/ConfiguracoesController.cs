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
using Microsoft.AspNetCore.Authorization;
using GeradorDRG.Services;

namespace GeradorDRG.Controllers
{
    [Authorize]
    public class ConfiguracoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Inicial()
        {
            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome");

            @ViewBag.Ativo = "disabled";

            return View("Configuracao",new Configuracao());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Inicial([Bind("Id,BancoId,SistemaId,BancoURL,BancoUsuario,BancoSenha,BancoSID,CodDRG,NomeDRG,UtilizaWebService,WebServiceUsuario,WebServiceSenha,PacienteTeste,PrestadorTeste,MotivosAlta,TiposInterncao")] Configuracao configuracao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(configuracao);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Gerar");
            }

            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome", configuracao.SistemaId);

            @ViewBag.Ativo = "activade";

            return View("Configuracao",configuracao);
        }



        // GET: Configuracoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                id = _context.Configuracao.FirstOrDefault().Id;
            }

            var configuracao = await _context.Configuracao.Include(m => m.Prestadores).Include(m => m.Pacientes).Include(m => m.MotivosAlta).Include(m => m.TiposInternacao).Include(m => m.MotivosAlta).SingleOrDefaultAsync(m => m.Id == id);

            if (configuracao == null)
            {
                return NotFound();
            }

            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome", configuracao.SistemaId);

            @ViewBag.Ativo = "activade";

            return View("Configuracao",configuracao);
        }

        // POST: Configuracoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BancoId,SistemaId,BancoURL,BancoUsuario,BancoSenha,BancoSID,CodDRG,NomeDRG,UtilizaWebService,WebServiceUsuario,WebServiceSenha,PacienteTeste,PrestadorTeste,MotivosAlta,TiposInterncao")] Configuracao configuracao)
        {
            if (id != configuracao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var configuracaoAntiga = await _context.Configuracao.Include(m => m.Prestadores).Include(m => m.Pacientes).Include(m => m.MotivosAlta).SingleOrDefaultAsync(m => m.Id == id);
                    if (configuracao != null)
                    {
                        _context.PacienteTeste.RemoveRange(configuracaoAntiga.Pacientes);
                        _context.Entry(configuracaoAntiga).State = EntityState.Modified;


                        configuracaoAntiga.BancoId = configuracao.BancoId;
                        configuracaoAntiga.Banco = configuracao.Banco;
                        configuracaoAntiga.BancoSenha = configuracao.BancoSenha;
                        configuracaoAntiga.BancoSID = configuracao.BancoSID;
                        configuracaoAntiga.BancoURL = configuracao.BancoURL;
                        configuracaoAntiga.BancoUsuario = configuracao.BancoUsuario;
                        configuracaoAntiga.CodDRG = configuracao.CodDRG;
                        configuracaoAntiga.Id = configuracao.Id;
                        configuracaoAntiga.MotivosAlta = configuracao.MotivosAlta;
                        configuracaoAntiga.NomeDRG = configuracao.NomeDRG;
                        configuracaoAntiga.Pacientes = configuracao.Pacientes;
                        configuracaoAntiga.Prestadores = configuracao.Prestadores;
                        configuracaoAntiga.TiposInternacao = configuracao.TiposInternacao;
                        configuracaoAntiga.Sistema = configuracao.Sistema;
                        configuracaoAntiga.SistemaId = configuracao.SistemaId;
                        configuracaoAntiga.UtilizaWebService = configuracao.UtilizaWebService;
                        configuracaoAntiga.WebServiceSenha = configuracao.WebServiceSenha;
                        configuracaoAntiga.WebServiceUsuario = configuracao.WebServiceUsuario;

                        _context.Entry(configuracaoAntiga).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index","Gerar");
                    }


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
            }

            ViewData["SistemaId"] = new SelectList(_context.Sistema, "Id", "Nome", configuracao.SistemaId);

            @ViewBag.Ativo = "activade";

            return View("Configuracao",configuracao);
        }

        private bool ConfiguracaoExists(int id)
        {
            return _context.Configuracao.Any(e => e.Id == id);
        }

        [AllowAnonymous]
        public IActionResult _Prestador(int id)
        {
            ViewBag.Inicial = id;

            return PartialView(new PrestadorTeste());

        }

        [AllowAnonymous]
        public IActionResult _Paciente(int id)
        {
            ViewBag.Inicial = id;

            return PartialView(new PacienteTeste());

        }

        // GET
        [AllowAnonymous]
        public async Task<IActionResult> BuscaBanco(int? id)
        {
            var bancos = await _context.SistemaBanco.Where(m => m.SistemaId == id).Select(m => m.Banco).ToListAsync();

            return Json(bancos);

        }

        [AllowAnonymous]
        public async Task<IActionResult> _MotivoAlta(string URLBase, int banco, int sistema)
        {
            var b = await _context.Banco.Where(m => m.Id == banco).FirstOrDefaultAsync();

            var s = await _context.Sistema.Where(m => m.Id == sistema).FirstOrDefaultAsync();

            if(b != null && s!= null)
            {
                if(b.Id == 1 && b.Padrao && s.Id == 1 && s.Padrao)
                {
                    string URL = $"api/MotivoAlta";

                    var motivos = await WebApiService.GetResponseAsync<List<MotivoAlta>>(URLBase, URL);

                    return PartialView(motivos);
                }
            }

            return PartialView(new List<MotivoAlta>());

        }

        [AllowAnonymous]
        public async Task<IActionResult> _TipoInternacao(string URLBase, int banco, int sistema)
        {
            var b = await _context.Banco.Where(m => m.Id == banco).FirstOrDefaultAsync();

            var s = await _context.Sistema.Where(m => m.Id == sistema).FirstOrDefaultAsync();

            if (b != null && s != null)
            {
                if (b.Id == 1 && b.Padrao && s.Id == 1 && s.Padrao)
                {
                    string URL = $"api/TipoInternacao";

                    var tipos = await WebApiService.GetResponseAsync<List<TipoInternacao>>(URLBase, URL);

                    return PartialView(tipos);
                }
            }

            return PartialView(new List<TipoInternacao>());

        }

        [AllowAnonymous]
        public async Task<string> _VerificaConexao(string URLBase, int banco, int sistema)
        {
            var b = await _context.Banco.Where(m => m.Id == banco).FirstOrDefaultAsync();

            var s = await _context.Sistema.Where(m => m.Id == sistema).FirstOrDefaultAsync();

            if (b != null && s != null && URLBase != null)
            {
                if (b.Id == 1 && b.Padrao && s.Id == 1 && s.Padrao)
                {
                    string URL = $"api/VerificaConexao";

                    var conexao = await WebApiService.GetResponseAsync<object>(URLBase, URL);

                    return conexao.ToString();
                }
            }

            return "N";

        }

    }
}
