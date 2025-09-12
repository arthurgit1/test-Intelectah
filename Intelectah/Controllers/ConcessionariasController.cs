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

        public ConcessionariasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Concessionarias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Concessionarias.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("ConcessionariaID,Nome,Endereco,Telefone,Email")] Concessionaria concessionaria)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("ConcessionariaID,Nome,Endereco,Telefone,Email")] Concessionaria concessionaria)
        {
            if (id != concessionaria.ConcessionariaID) return NotFound();

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
                _context.Concessionarias.Remove(concessionaria);
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