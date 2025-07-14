using Microsoft.Data.SqlClient;
using PruebaTecnicaApi.Class;
using PruebaTecnicaModelo.Modelos;

namespace PruebaTecnicaApi.Services
{
    public class EmpleadoServicio : IEmpleadoServicio
    {
        private readonly string conexion;

        public EmpleadoServicio(IConfiguration configuracion)
        {
            this.conexion = configuracion.GetConnectionString("DefaultConnection");
        }

        private Empleado Mapeo(SqlDataReader reader)
        {
            return new Empleado()
            {
                Codigo = (int)reader["Codigo"],
                Puesto = reader["Puesto"].ToString(),
                Nombre = reader["Nombre"].ToString(),
                CodigoJefe = reader["CodigoJefe"] == DBNull.Value ? null : Convert.ToInt32(reader["CodigoJefe"])
            };
        }


        public async Task Actualizar(Empleado empleado)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_ActualizarEmpleado, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Codigo", empleado.Codigo));
                        cmd.Parameters.Add(new SqlParameter("@Puesto", empleado.Puesto));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", empleado.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@CodigoJefe", empleado.CodigoJefe));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar empleado: {ex.Message}");
            }
        }

        public async Task OtorgarCargo(int CodigoJefeViejo, int CodigoJefeNuevo)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_EmpleadosOtorgarCargo, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CodigoJefeViejo", CodigoJefeViejo));
                        cmd.Parameters.Add(new SqlParameter("@CodigoJefeNuevo", CodigoJefeNuevo));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al otorgar el cargo: {ex.Message}");
            }
        }

        public async Task EliminarPorId(int Codigo)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_EliminarEmpleado, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Codigo", Codigo));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar empleado: {ex.Message}");
            }
        }

        public async Task Insertar(Empleado empleado)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_InsertarEmpleado, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Puesto", empleado.Puesto));
                        cmd.Parameters.Add(new SqlParameter("@Nombre", empleado.Nombre));
                        cmd.Parameters.Add(new SqlParameter("@CodigoJefe", empleado.CodigoJefe));
                        await sql.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar empleado: {ex.Message}");
            }
        }

        public async Task<List<Empleado>> Obtener()
        {
            var response = new List<Empleado>();
            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_ObtenerTodoEmpleado, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(Mapeo(reader));
                            }
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los empleados: {ex.Message}");
            }
            return response;
        }

        public async Task<List<Empleado>> ObtenerPorPuesto(string Puesto)
        {
            var response = new List<Empleado>();
            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_ObtenerEmpleadoPuesto, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Puesto", Puesto));
                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(Mapeo(reader));
                            }
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener los empleados por sus puestos: {ex.Message}");
            }
            return response;
        }
        public async Task<List<Empleado>> ObtenerSubordinados(int CodigoJefe)
        {
            var response = new List<Empleado>();
            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_ObtenerSubordinados, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@CodigoJefe", CodigoJefe));
                        await sql.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.Add(Mapeo(reader));
                            }
                        }
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todos los subordinados: {ex.Message}");
            }
            return response;
        }

        public async Task<Empleado> ObtenerPorId(int Codigo)
        {
            Empleado response = new Empleado();

            try
            {
                using (SqlConnection sql = new SqlConnection(conexion))
                {
                    using (SqlCommand cmd = new SqlCommand(CatalogoConstantes.SP_ObtenerEmpleado, sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Codigo", Codigo));

                        await sql.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = Mapeo(reader);
                            }
                        }
                        return response;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el empleado: {ex.Message}");
            }
            return response;
        }
    }
}
