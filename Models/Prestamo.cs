using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models
{
    // Guarda cada vez que un usuario pide prestado un libro
    // y cuándo lo devuelve (si ya lo hizo).
    public class Prestamo
    {
        private int _id;
        private int _libroId;
        private int _usuarioId;
        private DateTime _fechaPrestamo;
        private DateTime? _fechaDevolucion;

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Required]
        // Clave foránea al libro prestado.
        public int LibroId
        {
            get { return _libroId; }
            set { _libroId = value; }
        }

        [Required]
        // Clave foránea al usuario que tomó el libro.
        public int UsuarioId
        {
            get { return _usuarioId; }
            set { _usuarioId = value; }
        }

        [Required]
        // Fecha en que se realiza el préstamo.
        public DateTime FechaPrestamo
        {
            get { return _fechaPrestamo; }
            set { _fechaPrestamo = value; }
        }

        // Si ya devolvió el libro, queda registrada aquí.
        public DateTime? FechaDevolucion
        {
            get { return _fechaDevolucion; }
            set { _fechaDevolucion = value; }
        }
    }
}
