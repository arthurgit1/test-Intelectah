using System.Collections.Generic;
using Intelectah.Models;

namespace Intelectah.Repositories.Interfaces
{
    public interface IFabricanteRepository
    {
        IEnumerable<Fabricante> GetAll();
        Fabricante GetById(int id);
        void Add(Fabricante fabricante);
        void Update(Fabricante fabricante);
        void Delete(int id); // Deleção lógica
        bool ExistsByName(string nome);
    }
}