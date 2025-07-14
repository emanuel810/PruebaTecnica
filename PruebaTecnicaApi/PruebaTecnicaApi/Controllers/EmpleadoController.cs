using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaApi.Services;
using PruebaTecnicaModelo.Modelos;

namespace PruebaTecnicaApi.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly IEmpleadoServicio empleadoServicio;

        public EmpleadoController(IEmpleadoServicio empleadoServicio)
        {
            this.empleadoServicio = empleadoServicio;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> ObtenerTodoEmpleado()
        {
            try
            {
                return await empleadoServicio.Obtener();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los empleados en el controlador: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("Subordinados/{codigoJefe}")]
        public async Task<ActionResult<IEnumerable<Empleado>>> ObtenerSubordinados(int codigoJefe)
        {
            try
            {
                return await empleadoServicio.ObtenerSubordinados(codigoJefe);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los subordinados en el controlador: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }

        [HttpGet("Puesto/{puesto}")]
        public async Task<ActionResult<IEnumerable<Empleado>>> ObtenerEmpleadoPuesto(string puesto)
        {
            try
            {
                return await empleadoServicio.ObtenerPorPuesto(puesto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los empleados en el controlador: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> ObtenerEmpleado(int id)
        {
            try
            {
                var estadoEntity = await empleadoServicio.ObtenerPorId(id);

                if (estadoEntity.Codigo == 0)
                {
                    return NotFound("No se encontro el empleado");
                }
                return estadoEntity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el empleado en el controlador: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> ActualizarEmpleado( Empleado empleado)
        {
            try
            {

                await empleadoServicio.Actualizar(empleado);
                return StatusCode(204, "Se actualizo exitosamente:");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el empleado en el controlador: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }

        [HttpPut("{CodigoJefeViejo}/{CodigoJefeNuevo}")]
        public async Task<IActionResult> OtorgarCargo(int CodigoJefeViejo, int CodigoJefeNuevo)
        {
            try
            {

                await empleadoServicio.OtorgarCargo(CodigoJefeViejo, CodigoJefeNuevo);
                return StatusCode(204, "Se actualizo exitosamente:");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el empleado en el controlador: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Empleado>> AgregarEmpleado(Empleado empleado)
        {
            try
            {
                await empleadoServicio.Insertar(empleado);

                return StatusCode(201, "Se creo exitosamente");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el empleado en el controlador: {ex.Message}");
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }

        [HttpDelete("{Codigo}")]
        public async Task<IActionResult> EliminarEmpleado(int Codigo)
        {
            try
            {
                var estadoEntity = await empleadoServicio.ObtenerPorId(Codigo);
                if (estadoEntity == null)
                {
                    return NotFound("No se encontro el empleado");
                }


                await empleadoServicio.EliminarPorId(Codigo);

                return StatusCode(204, "Se elimino exitosamente:");
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Ocurrió un error interno del servidor: " + ex.Message);
            }
        }


    }
}
