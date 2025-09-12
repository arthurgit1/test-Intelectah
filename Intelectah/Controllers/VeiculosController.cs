using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intelectah.Data;
using Intelectah.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace Intelectah.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public VeiculosController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            const string cacheKey = "veiculos_ativos";
            if (!_cache.TryGetValue(cacheKey, out List<Veiculo>? veiculos))
            {
                veiculos = await _context.Veiculos.Include(v => v.Fabricante).Where(v => v.Ativo).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(cacheKey, veiculos, cacheEntryOptions);
            }
            return View(veiculos);
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var veiculo = await _context.Veiculos
                .Include(v => v.Fabricante)
                .FirstOrDefaultAsync(m => m.VeiculoID == id);
            if (veiculo == null) return NotFound();

            return View(veiculo);
        }

        // GET: Veiculos/Create
        public IActionResult Create()
        {
            ViewData["FabricanteID"] = new SelectList(_context.Fabricantes, "FabricanteID", "Nome");
            return View();
        }

        // POST: Veiculos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VeiculoID,Modelo,Cor,Ano,Placa,Preco,Tipo,FabricanteID,Ativo")] Veiculo veiculo)
        {
            if (veiculo.Ano > DateTime.Now.Year)
            {
                ModelState.AddModelError("Ano", "Ano de fabricação não pode ser no futuro.");
            }
            if (veiculo.Preco <= 0)
            {
                ModelState.AddModelError("Preco", "O preço deve ser um valor positivo.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
                _cache.Remove("veiculos_ativos");
                TempData["SuccessMessage"] = "Veículo cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["FabricanteID"] = new SelectList(_context.Fabricantes, "FabricanteID", "Nome", veiculo.FabricanteID);
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return NotFound();
            ViewData["FabricanteID"] = new SelectList(_context.Fabricantes, "FabricanteID", "Nome", veiculo.FabricanteID);
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VeiculoID,Modelo,Cor,Ano,Placa,Preco,Tipo,FabricanteID,Ativo")] Veiculo veiculo)
        {
            if (id != veiculo.VeiculoID) return NotFound();
            if (veiculo.Ano > DateTime.Now.Year)
            {
                ModelState.AddModelError("Ano", "Ano de fabricação não pode ser no futuro.");
            }
            if (veiculo.Preco <= 0)
            {
                ModelState.AddModelError("Preco", "O preço deve ser um valor positivo.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                    _cache.Remove("veiculos_ativos");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.VeiculoID))
                        return NotFound();
                    else
                        throw;
                }
                TempData["SuccessMessage"] = "Veículo editado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["FabricanteID"] = new SelectList(_context.Fabricantes, "FabricanteID", "Nome", veiculo.FabricanteID);
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var veiculo = await _context.Veiculos
                .Include(v => v.Fabricante)
                .FirstOrDefaultAsync(m => m.VeiculoID == id);
            if (veiculo == null) return NotFound();

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo != null)
            {
                veiculo.Ativo = false;
                _context.Veiculos.Update(veiculo);
                await _context.SaveChangesAsync();
                _cache.Remove("veiculos_ativos");
            }
            TempData["SuccessMessage"] = "Veículo excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.VeiculoID == id);
        }
    }
}