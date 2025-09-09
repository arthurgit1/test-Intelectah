using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Intelectah.Models;
using Intelectah.Services.Interfaces;
using Intelectah.ViewModels;

namespace Intelectah.Controllers
{
    public class FabricantesController : Controller
    {
        private readonly IFabricanteService _service;

        public FabricantesController(IFabricanteService service)
        {
            _service = service;
        }

        // GET: Fabricantes
        public ActionResult Index()
        {
            var fabricantes = _service.ListarTodos();
            return View(fabricantes);
        }

        // GET: Fabricantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var fabricante = _service.BuscarPorId(id.Value);
            if (fabricante == null)
                return HttpNotFound();
            return View(fabricante);
        }

        // GET: Fabricantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fabricantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FabricanteViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var fabricante = new Fabricante
                {
                    Nome = vm.Nome,
                    PaisOrigem = vm.PaisOrigem,
                    AnoFundacao = vm.AnoFundacao,
                    Website = vm.Website
                };
                try
                {
                    _service.Criar(fabricante);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(vm);
        }

        // GET: Fabricantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var fabricante = _service.BuscarPorId(id.Value);
            if (fabricante == null)
                return HttpNotFound();
            var vm = new FabricanteViewModel
            {
                FabricanteID = fabricante.FabricanteID,
                Nome = fabricante.Nome,
                PaisOrigem = fabricante.PaisOrigem,
                AnoFundacao = fabricante.AnoFundacao,
                Website = fabricante.Website
            };
            return View(vm);
        }

        // POST: Fabricantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FabricanteViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var fabricante = _service.BuscarPorId(vm.FabricanteID.Value);
                if (fabricante == null)
                    return HttpNotFound();
                fabricante.Nome = vm.Nome;
                fabricante.PaisOrigem = vm.PaisOrigem;
                fabricante.AnoFundacao = vm.AnoFundacao;
                fabricante.Website = vm.Website;
                _service.Atualizar(fabricante);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Fabricantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var fabricante = _service.BuscarPorId(id.Value);
            if (fabricante == null)
                return HttpNotFound();
            return View(fabricante);
        }

        // POST: Fabricantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _service.Remover(id);
            return RedirectToAction("Index");
        }
    }
}