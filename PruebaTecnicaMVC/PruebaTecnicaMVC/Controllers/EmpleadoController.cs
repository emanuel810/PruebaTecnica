using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PruebaTecnicaModelo.Modelos;
using System.Text;

namespace PruebaTecnicaMVC.Controllers
{
    public class EmpleadoController : Controller
    {

        private readonly string baseUrl;

        public EmpleadoController(IConfiguration configuration)
        {
            baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var httpCliente = new HttpClient();
            var json = await httpCliente.GetStringAsync($"{baseUrl}");
            var empleadosPlanos = JsonConvert.DeserializeObject<List<Empleado>>(json);

            var empleadosJerarquicos = ConstruirJerarquia(empleadosPlanos);
            ViewBag.BaseUrl = baseUrl;
            return View(empleadosJerarquicos);
        }

        private List<Empleado> ConstruirJerarquia(List<Empleado> empleados)
        {
            var diccionario = empleados.ToDictionary(e => e.Codigo);
            var raiz = new List<Empleado>();

            foreach (var emp in empleados)
            {
                if (emp.CodigoJefe.HasValue && diccionario.ContainsKey(emp.CodigoJefe.Value))
                {
                    diccionario[emp.CodigoJefe.Value].Subordinados.Add(emp);
                }
                else
                {
                    raiz.Add(emp);
                }
            }

            return raiz;
        }

        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.BaseUrl = baseUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Empleado empleado)
        {
            var httpClient = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(empleado), Encoding.UTF8, "application/json");

            var respuesta = await httpClient.PostAsync($"{baseUrl}", contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Creado"] = true;
                return RedirectToAction("Index");
            }
            else
                return View(empleado);
        }


        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            TempData["EmpleadoId"] = id;
            ViewBag.BaseUrl = baseUrl;
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($"{baseUrl}/{id}");
            var empleado = JsonConvert.DeserializeObject<Empleado>(json);
            return View(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Empleado empleado)
        {
            var httpClient = new HttpClient();
            var contenido = new StringContent(JsonConvert.SerializeObject(empleado), Encoding.UTF8, "application/json");

            var respuesta = await httpClient.PutAsync($"{baseUrl}", contenido);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Editado"] = true;
                return RedirectToAction("Index");
            }
            else
                return View(empleado); 
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            ViewBag.BaseUrl = baseUrl;
            var httpClient = new HttpClient();
  
            var respuestaSub = await httpClient.GetAsync($"{baseUrl}/subordinados/{id}");
            var subordinadosJson = await respuestaSub.Content.ReadAsStringAsync();
            var subordinados = JsonConvert.DeserializeObject<List<Empleado>>(subordinadosJson);

            if (subordinados != null && subordinados.Any())
            {
                TempData["Subordinados"] = subordinadosJson;
                TempData["EmpleadoId"] = id;
                return RedirectToAction("Index", new { tieneSubordinados = true });
            }
            var eliminarResp = await httpClient.DeleteAsync($"{baseUrl}/{id}");
            if (eliminarResp.IsSuccessStatusCode)
                return RedirectToAction("Index", new { eliminado = true });

            return Content("Error al eliminar");
        }


    }
}
