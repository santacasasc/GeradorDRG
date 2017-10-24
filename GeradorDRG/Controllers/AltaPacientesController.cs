using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GeradorDRG.Data;
using GeradorDRG.Models;

namespace GeradorDRG.Controllers
{
    public class AltaPacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AltaPacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AltaPacientes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AltaPaciente.Include(a => a.Configuracao);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AltaPacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var altaPaciente = await _context.AltaPaciente
                .Include(a => a.Configuracao)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (altaPaciente == null)
            {
                return NotFound();
            }

            return View(altaPaciente);
        }

        // GET: AltaPacientes/Create
        public IActionResult Create()
        {
            ViewData["ConfiguracaoId"] = new SelectList(_context.Configuracao, "Id", "Id");
            return View();
        }

        // POST: AltaPacientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ConfiguracaoId,CodigoMotivo,MotivoAlta,Tipo")] AltaPaciente altaPaciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(altaPaciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ConfiguracaoId"] = new SelectList(_context.Configuracao, "Id", "Id", altaPaciente.ConfiguracaoId);
            return View(altaPaciente);

        }

        // GET: AltaPacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var altaPaciente = await _context.AltaPaciente.SingleOrDefaultAsync(m => m.Id == id);
            if (altaPaciente == null)
            {
                return NotFound();
            }
            ViewData["ConfiguracaoId"] = new SelectList(_context.Configuracao, "Id", "Id", altaPaciente.ConfiguracaoId);
            return View(altaPaciente);
        }

        // POST: AltaPacientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ConfiguracaoId,CodigoMotivo,MotivoAlta,Tipo")] AltaPaciente altaPaciente)
        {
            if (id != altaPaciente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(altaPaciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AltaPacienteExists(altaPaciente.Id))
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
            ViewData["ConfiguracaoId"] = new SelectList(_context.Configuracao, "Id", "Id", altaPaciente.ConfiguracaoId);
            return View(altaPaciente);
        }

        // GET: AltaPacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var altaPaciente = await _context.AltaPaciente
                .Include(a => a.Configuracao)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (altaPaciente == null)
            {
                return NotFound();
            }

            return View(altaPaciente);
        }

        // POST: AltaPacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var altaPaciente = await _context.AltaPaciente.SingleOrDefaultAsync(m => m.Id == id);
            _context.AltaPaciente.Remove(altaPaciente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AltaPacienteExists(int id)
        {
            return _context.AltaPaciente.Any(e => e.Id == id);
        }
    }
}
