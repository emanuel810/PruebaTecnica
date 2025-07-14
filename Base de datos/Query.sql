
USE PruebaTecnica

--TABLA EMPLEADOS

CREATE TABLE Empleados (
    Codigo INT IDENTITY(1,1) PRIMARY KEY,
    Puesto NVARCHAR(50) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    CodigoJefe INT NULL,
    FOREIGN KEY (CodigoJefe) REFERENCES Empleados(Codigo)
);
GO

--CREACION DE INDICES
CREATE INDEX IX_Empleados_CodigoJefe ON Empleados(CodigoJefe);

CREATE INDEX IX_Empleados_Puesto ON Empleados(Puesto);

--PROCEDIMIENTO PARA INSERTAR UN EMPLEADO

CREATE PROCEDURE InsertarEmpleado
    @Puesto NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @CodigoJefe INT = NULL
AS
BEGIN
    INSERT INTO Empleados (Puesto, Nombre, CodigoJefe)
    VALUES (@Puesto, @Nombre, @CodigoJefe);
END;
GO

--PROCEDIMIENTO PARA ACTUALIZAR EL EMPLEADO

CREATE PROCEDURE ActualizarEmpleado
    @Codigo INT,
    @Puesto NVARCHAR(50),
    @Nombre NVARCHAR(100),
    @CodigoJefe INT = NULL
AS
BEGIN
    UPDATE Empleados
    SET Puesto = @Puesto,
        Nombre = @Nombre,
        CodigoJefe = @CodigoJefe
    WHERE Codigo = @Codigo;
END;
GO

--PROCEDIMIENTO PARA ELIMINAR EL EMPLEADO

CREATE PROCEDURE EliminarEmpleado
    @Codigo INT
AS
BEGIN
    DELETE FROM Empleados
    WHERE Codigo = @Codigo;
END;


--PROCEDIMIENTO PARA OBTENER INFORMACION DE UN EMPLEADO POR CODIGO

CREATE PROCEDURE ObtenerEmpleado
    @Codigo INT
AS
BEGIN
    Select Codigo,Puesto,Nombre,CodigoJefe FROM Empleados WHERE Codigo= @Codigo;
END;

--PROCEDIMIENTO PARA SELECCIONAR TODOS LOS EMPLEADOS

CREATE PROCEDURE ObtenerTodoEmpleado
AS
BEGIN
    Select Codigo,Puesto,Nombre,CodigoJefe FROM Empleados;
END;


--PROCEDIMIENTO PARA SELECCIONAR EL JEFE INMEDIATO

CREATE PROCEDURE EmpleadoPuesto @Puesto VARCHAR(64)
AS
BEGIN
SELECT Codigo,Puesto,Nombre,CodigoJefe  FROM Empleados where Puesto = @Puesto 
END
GO

--PROCEDIMIENTO PARA SELECCIONAR LOS SUBORDINADOS

CREATE PROCEDURE EmpleadosSubordinados @CodigoJefe INT
AS
BEGIN
SELECT Codigo,Puesto,Nombre,CodigoJefe From Empleados where CodigoJefe = @CodigoJefe
END
GO

--PROCEDIMIENTO PARA OTORGAR CARGO POR ACTUALIZACION O ELIMINACION

CREATE PROCEDURE EmpleadosOtorgarCargo @CodigoJefeViejo INT, @CodigoJefeNuevo INT
AS
BEGIN
UPDATE Empleados SET CodigoJefe=@CodigoJefeNuevo where CodigoJefe=@CodigoJefeViejo
END
GO
