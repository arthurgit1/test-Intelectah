using System.Collections.Generic;
using Intelectah.Models;
using Intelectah.Repositories.Interfaces;
using Intelectah.Services.Interfaces;

namespace Intelectah.Services
{
    public class FabricanteService : IFabricanteService
    {
        private readonly IFabricanteRepository _repo;
        public FabricanteService(IFabricanteRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<Fabricante> ListarTodos()
        {
            return _repo.GetAll();
        }

        public Fabricante BuscarPorId(int id)
        {
            return _repo.GetById(id);
        }

        public void Criar(Fabricante fabricante)
        {
            if (_repo.ExistsByName(fabricante.Nome))
                throw new System.Exception("Nome de fabricante já existe.");
            _repo.Add(fabricante);
        }

        public void Atualizar(Fabricante fabricante)
        {
            _repo.Update(fabricante);
        }

        public void Remover(int id)
        {
            _repo.Delete(id);
        }

        public bool NomeExiste(string nome)
        {
            return _repo.ExistsByName(nome);
        }
    }
}