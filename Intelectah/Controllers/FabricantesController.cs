using Intelectah.Models;
using Intelectah.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Intelectah.Controllers
{
    public class FabricantesController : Controller
    {
        private readonly IFabricanteRepository _repository;

        public FabricantesController(IFabricanteRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var fabricantes = _repository.GetAll();
            return View(fabricantes);
        }

        public IActionResult Details(int id)
        {
            var fabricante = _repository.GetById(id);
            if (fabricante == null) return NotFound();
            return View(fabricante);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Fabricante fabricante)
        {
            if (_repository.ExistsByName(fabricante.Nome))
                ModelState.AddModelError("Nome", "Já existe um fabricante com esse nome.");

            if (ModelState.IsValid)
            {
                _repository.Add(fabricante);
                return RedirectToAction(nameof(Index));
            }
            return View(fabricante);
        }

        public IActionResult Edit(int id)
        {
            var fabricante = _repository.GetById(id);
            if (fabricante == null) return NotFound();
            return View(fabricante);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Fabricante fabricante)
        {
            if (id != fabricante.FabricanteID) return NotFound();

            if (ModelState.IsValid)
            {
                _repository.Update(fabricante);
                return RedirectToAction(nameof(Index));
            }
            return View(fabricante);
        }

        public IActionResult Delete(int id)
        {
            var fabricante = _repository.GetById(id);
            if (fabricante == null) return NotFound();
            return View(fabricante);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}