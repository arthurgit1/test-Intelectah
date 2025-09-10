using System;
using Intelectah.Repositories;
using Intelectah.Repositories.Interfaces;
using Intelectah.Services;
using Intelectah.Services.Interfaces;
using Microsoft.Practices.Unity;

namespace Intelectah
{
    public static class UnityConfig
    {
        public static void RegisterComponents(IUnityContainer container)
        {
            // ...existing code...
            container.RegisterType<Intelectah.Repositories.Interfaces.IFabricanteRepository, Intelectah.Repositories.FabricanteRepository>();
            container.RegisterType<IFabricanteService, FabricanteService>();
            // ...existing code...
        }
    }
}