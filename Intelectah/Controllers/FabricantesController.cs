using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Intelectah.Data;
using Intelectah.Models;
using Microsoft.AspNetCore.Authorization;

namespace Intelectah.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FabricantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FabricantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fabricantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fabricantes.ToListAsync());
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
            if (ModelState.IsValid)
            {
                _context.Add(fabricante);
                await _context.SaveChangesAsync();
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fabricante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FabricanteExists(fabricante.FabricanteID))
                        return NotFound();
                    else
                        throw;
                }
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
                _context.Fabricantes.Remove(fabricante);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FabricanteExists(int id)
        {
            return _context.Fabricantes.Any(e => e.FabricanteID == id);
        }
    }
}