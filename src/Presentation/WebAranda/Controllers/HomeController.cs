using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAranda.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISystemGatherDataService service;
        private readonly IGenericRepositoryAsync<Audit> repository;
        public HomeController(ILogger<HomeController> logger,
                              ISystemGatherDataService service,
                              IGenericRepositoryAsync<Audit> repository)
        {
            _logger = logger;
            this.service = service;
            this.repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("MostrarDatos")]
        public async Task<IActionResult> MostrarDatosAsync()
        {
            var listaDatos = new List<string>();
            listaDatos.Add($"Sistema operativo : {service.GetOperatingSystem()}");
            listaDatos.Add($"Nombre de la maquina : {service.GetMachineName()}");
            foreach (var item in service.GetIPAdress())
            {
                listaDatos.Add($"Dirección IP : {item}");
            }
            foreach (var disk in service.GetHardDiskCapacity())
            {
                listaDatos.Add($"Disco duro : {disk}");
            }
            listaDatos.Add($"RAM : {service.GetRAMCapacity()}");
            listaDatos.Add($"Procesador : {service.GetCpuName()}");
            listaDatos.Add($"Fecha reporte : {DateTime.Now.ToString("dd-MMMM-yyyy HH:mm:ss")}");

            var audit = new Audit()
            {
                Info = JsonSerializer.Serialize(listaDatos),
                CreatedBy = Environment.UserName,
                CreatedOn = DateTime.Now
            };
            await repository.AddAsync(audit);


            return Json(new { id = audit.Id, info = listaDatos });
        }

        [HttpPost, ActionName("GenerarReporte")]
        public async Task<IActionResult> GenerarReporteAsync(int id)
        {
            var data = await repository.GetByIdAsync(id);
            if (data == null)
            {
                return Json(new { ok = false });
            }
            var Datos = JsonSerializer.Deserialize<List<string>>(data.Info);

            string rutaBase = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = $"aplicativo_aranda_{DateTime.Now.ToString("dd_MMMM_yyyy_HHmmss")}.txt";
            string filePath = Path.Combine(rutaBase, nombreArchivo);
            using var stream = new FileStream(filePath, FileMode.CreateNew);
            using var writer = new StreamWriter(stream);
            foreach (var dato in Datos)
            {
                writer.WriteLine(dato);
            }

            return Json(new { ok = true, ruta = rutaBase, nombre = nombreArchivo });
        }
    }
}
