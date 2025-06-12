using Microsoft.EntityFrameworkCore;
using BibliotecaApi.Models;

namespace BibliotecaApi.Datos
{
    // Contexto principal de Entity Framework Core para la biblioteca.
    // Aquí definimos las tablas Libros, Usuarios y Prestamos.
    public class BibliotecaContext : DbContext
    {
        private DbSet<Libro> _libros;
        private DbSet<Usuario> _usuarios;
        private DbSet<Prestamo> _prestamos;

        // Recibe las opciones de conexión desde Startup.cs
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {
        }

        // Representa la tabla de libros en la base de datos.
        public DbSet<Libro> Libros
        {
            get { return _libros; }
            set { _libros = value; }
        }

        // Representa la tabla de usuarios en la base de datos.
        public DbSet<Usuario> Usuarios
        {
            get { return _usuarios; }
            set { _usuarios = value; }
        }

        // Representa la tabla de préstamos en la base de datos.
        public DbSet<Prestamo> Prestamos
        {
            get { return _prestamos; }
            set { _prestamos = value; }
        }
    }
}
