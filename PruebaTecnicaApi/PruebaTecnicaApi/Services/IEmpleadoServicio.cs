using PruebaTecnicaModelo.Modelos;

namespace PruebaTecnicaApi.Services
{
    public interface IEmpleadoServicio
    {
        public Task<List<Empleado>> Obtener();
        public Task<List<Empleado>> ObtenerPorPuesto(string Puesto);
        public Task<List<Empleado>> ObtenerSubordinados(int CodigoJefe);
        public Task<Empleado> ObtenerPorId(int Codigo);
        public Task Insertar(Empleado empleado);
        public Task Actualizar(Empleado empleado);
        public Task OtorgarCargo(int CodigoJefeViejo, int CodigoJefeNuevo);
        public Task EliminarPorId(int Codigo);

    }
}
