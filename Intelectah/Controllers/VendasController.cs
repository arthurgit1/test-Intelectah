using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Intelectah.Data;
using Intelectah.Models;

namespace Intelectah.Controllers
{
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var vendas = _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Veiculo)
                .Include(v => v.Concessionaria);
            return View(await vendas.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Veiculo)
                .Include(v => v.Concessionaria)
                .FirstOrDefaultAsync(m => m.VendaID == id);
            if (venda == null) return NotFound();

            return View(venda);
        }

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "Nome");
            ViewData["VeiculoID"] = new SelectList(_context.Veiculos, "VeiculoID", "Modelo");
            ViewData["ConcessionariaID"] = new SelectList(_context.Concessionarias, "ConcessionariaID", "Nome");
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendaID,ClienteID,VeiculoID,ConcessionariaID,DataVenda,Valor")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "Nome", venda.ClienteID);
            ViewData["VeiculoID"] = new SelectList(_context.Veiculos, "VeiculoID", "Modelo", venda.VeiculoID);
            ViewData["ConcessionariaID"] = new SelectList(_context.Concessionarias, "ConcessionariaID", "Nome", venda.ConcessionariaID);
            return View(venda);
        }

        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var venda = await _context.Vendas.FindAsync(id);
            if (venda == null) return NotFound();
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "Nome", venda.ClienteID);
            ViewData["VeiculoID"] = new SelectList(_context.Veiculos, "VeiculoID", "Modelo", venda.VeiculoID);
            ViewData["ConcessionariaID"] = new SelectList(_context.Concessionarias, "ConcessionariaID", "Nome", venda.ConcessionariaID);
            return View(venda);
        }

        // POST: Vendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendaID,ClienteID,VeiculoID,ConcessionariaID,DataVenda,Valor")] Venda venda)
        {
            if (id != venda.VendaID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.VendaID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteID"] = new SelectList(_context.Clientes, "ClienteID", "Nome", venda.ClienteID);
            ViewData["VeiculoID"] = new SelectList(_context.Veiculos, "VeiculoID", "Modelo", venda.VeiculoID);
            ViewData["ConcessionariaID"] = new SelectList(_context.Concessionarias, "ConcessionariaID", "Nome", venda.ConcessionariaID);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Veiculo)
                .Include(v => v.Concessionaria)
                .FirstOrDefaultAsync(m => m.VendaID == id);
            if (venda == null) return NotFound();

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.VendaID == id);
        }
    }
}