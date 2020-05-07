using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DTM_ExtracaoOFertasCSDPontual.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using DTM_Repository.Data.SqlServer.Repository;
using DTM_Repository.Data.SqlServer.Extensions;
using CsvHelper;

namespace DTM_ExtracaoOFertasCSDPontual.Controllers
{
    public class HomeController : Controller
    {
        private static dynamic builder = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configuration = builder.Build();
        DTM_SqlServerRepository repo = new DTM_SqlServerRepository();
        List<OfertaModel> retorno = new List<OfertaModel>();

        public IActionResult CSD()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        [Route("/Home/BuscaOferta/{codigoOferta}")]
        public List<OfertaModel> buscaOferta(string codigoOferta)
        {

            try
            {
                repo.setConnectionString(configuration.GetConnectionString("SqlServer.Host"), configuration.GetConnectionString("SqlServer.User"), configuration.GetConnectionString("SqlServer.Password"), configuration.GetConnectionString("SqlServer.BaseSqlServer"));
                retorno = repo.ExecuteQuery<OfertaModel>($"select * from [dbo].[EXPORT_OFERTAS_REPORT] where codigoOferta = @codigo", new { codigo = codigoOferta }).ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
            return retorno;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
