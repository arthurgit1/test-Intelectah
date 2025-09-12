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
    public class FabricantesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public FabricantesController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Fabricantes
        public async Task<IActionResult> Index()
        {
            const string cacheKey = "fabricantes_ativos";
            if (!_cache.TryGetValue(cacheKey, out List<Fabricante>? fabricantes))
            {
                fabricantes = await _context.Fabricantes.Where(f => f.Ativo).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(cacheKey, fabricantes, cacheEntryOptions);
            }
            return View(fabricantes);
        }

        // GET: Fabricantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var fabricante = await _context.Fabricantes
                .FirstOrDefaultAsync(m => m.FabricanteID == id);
            if (fabricante == null) return NotFound();

            return View(fabricante);
        }

        // GET: Fabricantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fabricantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FabricanteID,Nome,PaisOrigem,AnoFundacao,Website,Ativo")] Fabricante fabricante)
        {
            if (_context.Fabricantes.Any(f => f.Nome == fabricante.Nome && f.Ativo))
            {
                ModelState.AddModelError("Nome", "Já existe um fabricante ativo com este nome.");
            }
            if (fabricante.AnoFundacao > DateTime.Now.Year)
            {
                ModelState.AddModelError("AnoFundacao", "Ano de fundação deve ser no passado.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(fabricante);
                await _context.SaveChangesAsync();
                _cache.Remove("fabricantes_ativos");
                TempData["SuccessMessage"] = "Fabricante cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(fabricante);
        }

        // GET: Fabricantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null) return NotFound();
            return View(fabricante);
        }

        // POST: Fabricantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FabricanteID,Nome,PaisOrigem,AnoFundacao,Website,Ativo")] Fabricante fabricante)
        {
            if (id != fabricante.FabricanteID) return NotFound();
            if (_context.Fabricantes.Any(f => f.Nome == fabricante.Nome && f.FabricanteID != id && f.Ativo))
            {
                ModelState.AddModelError("Nome", "Já existe um fabricante ativo com este nome.");
            }
            if (fabricante.AnoFundacao > DateTime.Now.Year)
            {
                ModelState.AddModelError("AnoFundacao", "Ano de fundação deve ser no passado.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabricante);
                    await _context.SaveChangesAsync();
                    _cache.Remove("fabricantes_ativos");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricanteExists(fabricante.FabricanteID))
                        return NotFound();
                    else
                        throw;
                }
                TempData["SuccessMessage"] = "Fabricante editado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(fabricante);
        }

        // GET: Fabricantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var fabricante = await _context.Fabricantes
                .FirstOrDefaultAsync(m => m.FabricanteID == id);
            if (fabricante == null) return NotFound();

            return View(fabricante);
        }

        // POST: Fabricantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante != null)
            {
                fabricante.Ativo = false;
                _context.Fabricantes.Update(fabricante);
                await _context.SaveChangesAsync();
                _cache.Remove("fabricantes_ativos");
            }
            TempData["SuccessMessage"] = "Fabricante excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool FabricanteExists(int id)
        {
            return _context.Fabricantes.Any(e => e.FabricanteID == id);
        }
    }
}