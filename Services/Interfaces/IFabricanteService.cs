using System.Collections.Generic;
using Intelectah.Models;

namespace Intelectah.Services.Interfaces
{
    public interface IFabricanteService
    {
        IEnumerable<Fabricante> ListarTodos();
        Fabricante BuscarPorId(int id);
        void Criar(Fabricante fabricante);
        void Atualizar(Fabricante fabricante);
        void Remover(int id);
        bool NomeExiste(string nome);
    }
}