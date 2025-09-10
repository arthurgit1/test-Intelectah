using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Intelectah.Models;
using Intelectah.Repositories.Interfaces;

namespace Intelectah
{
    public class Fabricante
    {
        public int FabricanteID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string PaisOrigem { get; set; }

        [Required]
        [Range(1800, 2100, ErrorMessage = "Ano de fundação deve ser válido e no passado.")]
        public int AnoFundacao { get; set; }

        [Url]
        [StringLength(255)]
        public string Website { get; set; }

        public bool Ativo { get; set; } = true; // Deleção lógica
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public DbSet<Fabricante> Fabricantes { get; set; }
    }

    public class FabricanteRepository : IFabricanteRepository
    {
        private readonly ApplicationDbContext _context;

        public FabricanteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Fabricante> GetAll()
        {
            return _context.Fabricantes.Where(f => f.Ativo).ToList();
        }

        public Fabricante GetById(int id)
        {
            return _context.Fabricantes.FirstOrDefault(f => f.FabricanteID == id && f.Ativo);
        }

        public void Add(Fabricante fabricante)
        {
            _context.Fabricantes.Add(fabricante);
            _context.SaveChanges();
        }

        public void Update(Fabricante fabricante)
        {
            _context.Entry(fabricante).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var fabricante = GetById(id);
            if (fabricante != null)
            {
                fabricante.Ativo = false;
                _context.Entry(fabricante).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public bool ExistsByName(string nome)
        {
            return _context.Fabricantes.Any(f => f.Nome == nome && f.Ativo);
        }
    }

    public class FabricantesController : Controller
    {
        private readonly IFabricanteRepository _repository;

        public FabricantesController(IFabricanteRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            var fabricantes = _repository.GetAll();
            return View(fabricantes);
        }

        public ActionResult Details(int id)
        {
            var fabricante = _repository.GetById(id);
            if (fabricante == null) return HttpNotFound();
            return View(fabricante);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fabricante fabricante)
        {
            if (_repository.ExistsByName(fabricante.Nome))
                ModelState.AddModelError("Nome", "Já existe um fabricante com esse nome.");

            if (ModelState.IsValid)
            {
                _repository.Add(fabricante);
                return RedirectToAction("Index");
            }
            return View(fabricante);
        }

        public ActionResult Edit(int id)
        {
            var fabricante = _repository.GetById(id);
            if (fabricante == null) return HttpNotFound();
            return View(fabricante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fabricante fabricante)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(fabricante);
                return RedirectToAction("Index");
            }
            return View(fabricante);
        }

        public ActionResult Delete(int id)
        {
            var fabricante = _repository.GetById(id);
            if (fabricante == null) return HttpNotFound();
            return View(fabricante);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}