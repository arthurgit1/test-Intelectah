using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Intelectah.Data;
using Intelectah.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Intelectah.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConcessionariasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Intelectah.Services.ViaCepService _viaCepService;
        private readonly IMemoryCache _cache;

        public ConcessionariasController(ApplicationDbContext context, Intelectah.Services.ViaCepService viaCepService, IMemoryCache cache)
        {
            _context = context;
            _viaCepService = viaCepService;
            _cache = cache;
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
            const string cacheKey = "concessionarias_ativas";
            if (!_cache.TryGetValue(cacheKey, out List<Concessionaria>? concessionarias))
            {
                concessionarias = await _context.Concessionarias.Where(c => c.IsAtivo).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(cacheKey, concessionarias, cacheEntryOptions);
            }
            return View(concessionarias);
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
                _cache.Remove("concessionarias_ativas");
                TempData["SuccessMessage"] = "Concessionária cadastrada com sucesso!";
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
                    _cache.Remove("concessionarias_ativas");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcessionariaExists(concessionaria.ConcessionariaID))
                        return NotFound();
                    else
                        throw;
                }
                TempData["SuccessMessage"] = "Concessionária editada com sucesso!";
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
                _cache.Remove("concessionarias_ativas");
            }
            TempData["SuccessMessage"] = "Concessionária excluída com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool ConcessionariaExists(int id)
        {
            return _context.Concessionarias.Any(e => e.ConcessionariaID == id);
        }
    }
}