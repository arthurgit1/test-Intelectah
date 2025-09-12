using System.Globalization;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Intelectah.Data;
using Microsoft.EntityFrameworkCore;

namespace Intelectah.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Vendas por mês (últimos 12 meses)
            var vendasPorMes = await _context.Vendas
                .Where(v => v.DataVenda > DateTime.Now.AddMonths(-12))
                .GroupBy(v => new { v.DataVenda.Year, v.DataVenda.Month })
                .Select(g => new
                {
                    Mes = g.Key.Month,
                    Ano = g.Key.Year,
                    Total = g.Count()
                })
                .OrderBy(g => g.Ano).ThenBy(g => g.Mes)
                .ToListAsync();

            // Vendas por tipo de veículo
            var vendasPorTipo = await _context.Vendas
                .Include(v => v.Veiculo)
                .GroupBy(v => v.Veiculo.Tipo)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            // Vendas por fabricante
            var vendasPorFabricante = await _context.Vendas
                .Include(v => v.Veiculo)
                .ThenInclude(ve => ve.Fabricante)
                .GroupBy(v => v.Veiculo.Fabricante.Nome)
                .Select(g => new
                {
                    Fabricante = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            // Vendas por concessionária
            var vendasPorConcessionaria = await _context.Vendas
                .Include(v => v.Concessionaria)
                .GroupBy(v => v.Concessionaria.Nome)
                .Select(g => new
                {
                    Concessionaria = g.Key,
                    Total = g.Count()
                })
                .ToListAsync();

            // Total de clientes
            var totalClientes = await _context.Clientes.CountAsync();

            ViewBag.VendasPorMes = vendasPorMes;
            ViewBag.VendasPorTipo = vendasPorTipo;
            ViewBag.VendasPorFabricante = vendasPorFabricante;
            ViewBag.VendasPorConcessionaria = vendasPorConcessionaria;
            ViewBag.TotalClientes = totalClientes;

            return View();
        }

        // GET: Dashboard/ExportExcel?mes=9&ano=2025
        [HttpGet]
        public async Task<IActionResult> ExportExcel(int mes, int ano)
        {
            // Filtra vendas do mês/ano
            var vendas = await _context.Vendas
                .Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                .Include(v => v.Concessionaria)
                .Where(v => v.DataVenda.Month == mes && v.DataVenda.Year == ano)
                .ToListAsync();

            // Agregações
            var totalVendas = vendas.Count;
            var vendasPorTipo = vendas.GroupBy(v => v.Veiculo.Tipo)
                .Select(g => new { Tipo = g.Key.ToString(), Total = g.Count() }).ToList();
            var vendasPorFabricante = vendas.GroupBy(v => v.Veiculo.Fabricante.Nome)
                .Select(g => new { Fabricante = g.Key, Total = g.Count() }).ToList();
            var vendasPorConcessionaria = vendas.GroupBy(v => v.Concessionaria.Nome)
                .Select(g => new { Concessionaria = g.Key, Total = g.Count() }).ToList();

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Relatório Mensal de Vendas");
            ws.Cell(1, 1).Value = $"Relatório Mensal de Vendas - {mes:D2}/{ano}";
            ws.Cell(2, 1).Value = $"Total de Vendas: {totalVendas}";

            int row = 4;
            ws.Cell(row++, 1).Value = "Vendas por Tipo de Veículo";
            foreach (var item in vendasPorTipo)
                ws.Cell(row++, 1).Value = $"{item.Tipo}: {item.Total}";

            row++;
            ws.Cell(row++, 1).Value = "Vendas por Fabricante";
            foreach (var item in vendasPorFabricante)
                ws.Cell(row++, 1).Value = $"{item.Fabricante}: {item.Total}";

            row++;
            ws.Cell(row++, 1).Value = "Vendas por Concessionária";
            foreach (var item in vendasPorConcessionaria)
                ws.Cell(row++, 1).Value = $"{item.Concessionaria}: {item.Total}";

            using var stream = new System.IO.MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            string fileName = $"RelatorioMensalVendas_{mes:D2}_{ano}.xlsx";
            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        // GET: Dashboard/ExportPdf?mes=9&ano=2025
        [HttpGet]
        public async Task<IActionResult> ExportPdf(int mes, int ano)
        {
            var vendas = await _context.Vendas
                .Include(v => v.Veiculo).ThenInclude(ve => ve.Fabricante)
                .Include(v => v.Concessionaria)
                .Where(v => v.DataVenda.Month == mes && v.DataVenda.Year == ano)
                .ToListAsync();

            var totalVendas = vendas.Count;
            var vendasPorTipo = vendas.GroupBy(v => v.Veiculo.Tipo)
                .Select(g => new { Tipo = g.Key.ToString(), Total = g.Count() }).ToList();
            var vendasPorFabricante = vendas.GroupBy(v => v.Veiculo.Fabricante.Nome)
                .Select(g => new { Fabricante = g.Key, Total = g.Count() }).ToList();
            var vendasPorConcessionaria = vendas.GroupBy(v => v.Concessionaria.Nome)
                .Select(g => new { Concessionaria = g.Key, Total = g.Count() }).ToList();

            var pdf = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"Relatório Mensal de Vendas - {mes:D2}/{ano}").FontSize(18).Bold();
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Total de Vendas: {totalVendas}").FontSize(14);
                        col.Item().Text("");
                        col.Item().Text("Vendas por Tipo de Veículo:").Bold();
                        foreach (var item in vendasPorTipo)
                            col.Item().Text($"- {item.Tipo}: {item.Total}");
                        col.Item().Text("");
                        col.Item().Text("Vendas por Fabricante:").Bold();
                        foreach (var item in vendasPorFabricante)
                            col.Item().Text($"- {item.Fabricante}: {item.Total}");
                        col.Item().Text("");
                        col.Item().Text("Vendas por Concessionária:").Bold();
                        foreach (var item in vendasPorConcessionaria)
                            col.Item().Text($"- {item.Concessionaria}: {item.Total}");
                    });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            string fileName = $"RelatorioMensalVendas_{mes:D2}_{ano}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}