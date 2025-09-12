using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Intelectah;
using Intelectah.Controllers;
using Intelectah.Data;
using Intelectah.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Caching.Memory;

namespace Intelectah.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Substitui o banco real por InMemory para o teste
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<Data.ApplicationDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);
                    services.AddDbContext<Data.ApplicationDbContext>(options =>
                        options.UseInMemoryDatabase("TestDbIntegration"));
                });
            });
        }

        [Fact]
        public async Task Get_Clientes_Index_DeveRetornar200()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Clientes");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }

    public class ClientesControllerTests
    {
        private ApplicationDbContext GetDbContextWithData()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            var context = new ApplicationDbContext(options);
            context.Clientes.AddRange(new List<Cliente>
            {
                new Cliente { ClienteID = 1, Nome = "Ativo", CPF = "12345678909", Endereco = "Rua 1", Telefone = "1111", Email = "a@a.com", Ativo = true },
                new Cliente { ClienteID = 2, Nome = "Inativo", CPF = "98765432100", Endereco = "Rua 2", Telefone = "2222", Email = "b@b.com", Ativo = false }
            });
            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task Index_DeveRetornarApenasClientesAtivos()
        {
            // Arrange
            var context = GetDbContextWithData();
            var mockCache = new Mock<IMemoryCache>();
            var controller = new ClientesController(context, mockCache.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Cliente>>(viewResult.Model);
            Assert.Single(model);
            Assert.Equal("Ativo", model.First().Nome);
        }
    }
}
