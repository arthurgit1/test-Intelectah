using System.Collections.Generic;
using System.Linq;
using Intelectah.Data;
using Intelectah.Models;
using Intelectah.Repositories.Interfaces;

namespace Intelectah.Repositories
{
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
            _context.Fabricantes.Update(fabricante);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var fabricante = GetById(id);
            if (fabricante != null)
            {
                fabricante.Ativo = false;
                _context.Fabricantes.Update(fabricante);
                _context.SaveChanges();
            }
        }

        public bool ExistsByName(string nome)
        {
            return _context.Fabricantes.Any(f => f.Nome == nome && f.Ativo);
        }
    }
}