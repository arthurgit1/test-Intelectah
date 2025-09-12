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

    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public ClientesController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            const string cacheKey = "clientes_ativos";
            if (!_cache.TryGetValue(cacheKey, out List<Cliente>? clientes))
            {
                clientes = await _context.Clientes.Where(c => c.Ativo).ToListAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));
                _cache.Set(cacheKey, clientes, cacheEntryOptions);
            }
            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteID == id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteID,Nome,CPF,Endereco,Telefone,Email")] Cliente cliente)
        {
            if (_context.Clientes.Any(c => c.CPF == cliente.CPF && c.Ativo))
            {
                ModelState.AddModelError("CPF", "Já existe um cliente ativo com este CPF.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                _cache.Remove("clientes_ativos"); // Invalida cache
                TempData["SuccessMessage"] = "Cliente cadastrado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteID,Nome,CPF,Endereco,Telefone,Email,Ativo")] Cliente cliente)
        {
            if (id != cliente.ClienteID) return NotFound();
            if (_context.Clientes.Any(c => c.CPF == cliente.CPF && c.ClienteID != id && c.Ativo))
            {
                ModelState.AddModelError("CPF", "Já existe um cliente ativo com este CPF.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                    _cache.Remove("clientes_ativos"); // Invalida cache
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteID))
                        return NotFound();
                    else
                        throw;
                }
                TempData["SuccessMessage"] = "Cliente editado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteID == id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                cliente.Ativo = false;
                _context.Clientes.Update(cliente);
                await _context.SaveChangesAsync();
                _cache.Remove("clientes_ativos"); // Invalida cache
            }
            TempData["SuccessMessage"] = "Cliente excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteID == id);
        }
    }
}