using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GeradorDRG.Data;

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

			return View();
		}
		public IActionResult Home()
		{
			return View();
		}

	}
}