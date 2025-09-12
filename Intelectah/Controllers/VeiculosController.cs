using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intelectah.Data;
using Intelectah.Models;
using Microsoft.AspNetCore.Authorization;

namespace Intelectah.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            var veiculos = _context.Veiculos.Include(v => v.Fabricante);
            return View(await veiculos.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("VeiculoID,Modelo,Cor,Ano,Placa,Preco,FabricanteID")] Veiculo veiculo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(veiculo);
                await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Edit(int id, [Bind("VeiculoID,Modelo,Cor,Ano,Placa,Preco,FabricanteID")] Veiculo veiculo)
        {
            if (id != veiculo.VeiculoID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.VeiculoID))
                        return NotFound();
                    else
                        throw;
                }
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
                _context.Veiculos.Remove(veiculo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.VeiculoID == id);
        }
    }
}