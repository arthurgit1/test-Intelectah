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
            container.RegisterType<IFabricanteRepository, FabricanteRepository>();
            container.RegisterType<IFabricanteService, FabricanteService>();
            // ...existing code...
        }
    }
}