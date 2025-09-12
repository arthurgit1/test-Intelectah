using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Intelectah.Data;
using Intelectah.Models;
using Microsoft.AspNetCore.Authorization;

namespace Intelectah.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConcessionariasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Intelectah.Services.ViaCepService _viaCepService;

        public ConcessionariasController(ApplicationDbContext context, Intelectah.Services.ViaCepService viaCepService)
        {
            _context = context;
            _viaCepService = viaCepService;
        }
        // GET: Concessionarias/BuscarEnderecoPorCep?cep=01001000
        [HttpGet]
        public async Task<IActionResult> BuscarEnderecoPorCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return Json(new { erro = true, mensagem = "CEP não informado." });
            var endereco = await _viaCepService.BuscarEnderecoPorCepAsync(cep);
            if (endereco == null || endereco.Erro == "true")
                return Json(new { erro = true, mensagem = "CEP não encontrado." });
            return Json(new
            {
                erro = false,
                logradouro = endereco.Logradouro,
                bairro = endereco.Bairro,
                cidade = endereco.Localidade,
                uf = endereco.Uf
            });
        }

        // GET: Concessionarias
        public async Task<IActionResult> Index()
        {
            // Exibir apenas registros ativos
            return View(await _context.Concessionarias.Where(c => c.IsAtivo).ToListAsync());
        }

        // GET: Concessionarias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var concessionaria = await _context.Concessionarias
                .FirstOrDefaultAsync(m => m.ConcessionariaID == id);
            if (concessionaria == null) return NotFound();

            return View(concessionaria);
        }

        // GET: Concessionarias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Concessionarias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcessionariaID,Nome,Endereco,Telefone,Email,IsAtivo")] Concessionaria concessionaria)
        {
            if (_context.Concessionarias.Any(c => c.Nome == concessionaria.Nome && c.IsAtivo))
            {
                ModelState.AddModelError("Nome", "Já existe uma concessionária ativa com este nome.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(concessionaria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(concessionaria);
        }

        // GET: Concessionarias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria == null) return NotFound();
            return View(concessionaria);
        }

        // POST: Concessionarias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcessionariaID,Nome,Endereco,Telefone,Email,IsAtivo")] Concessionaria concessionaria)
        {
            if (id != concessionaria.ConcessionariaID) return NotFound();
            if (_context.Concessionarias.Any(c => c.Nome == concessionaria.Nome && c.ConcessionariaID != id && c.IsAtivo))
            {
                ModelState.AddModelError("Nome", "Já existe uma concessionária ativa com este nome.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concessionaria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcessionariaExists(concessionaria.ConcessionariaID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(concessionaria);
        }

        // GET: Concessionarias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var concessionaria = await _context.Concessionarias
                .FirstOrDefaultAsync(m => m.ConcessionariaID == id);
            if (concessionaria == null) return NotFound();

            return View(concessionaria);
        }

        // POST: Concessionarias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concessionaria = await _context.Concessionarias.FindAsync(id);
            if (concessionaria != null)
            {
                // Deleção lógica
                concessionaria.IsAtivo = false;
                _context.Concessionarias.Update(concessionaria);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ConcessionariaExists(int id)
        {
            return _context.Concessionarias.Any(e => e.ConcessionariaID == id);
        }
    }
}