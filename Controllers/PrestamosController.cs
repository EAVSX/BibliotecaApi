using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibliotecaApi.Datos;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    // Este controlador agrupa todas las operaciones
    // relacionadas con el préstamo y devolución de libros.
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        // Contexto que nos da acceso a la base de datos
        private readonly BibliotecaContext _context;

        // Recibimos el contexto por inyección de dependencias
        public PrestamosController(BibliotecaContext context)
        {
            this._context = context;
        }

        // ---------------------------------------------
        // 1) Listar todos los préstamos registrados
        // ---------------------------------------------
        // Cuando el cliente llama a GET /api/prestamos,
        // queremos devolver la lista completa de préstamos
        // almacenados en la base de datos.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Prestamo> listaPrestamos = await this._context.Prestamos.ToListAsync();
            return Ok(listaPrestamos);
        }

        // ------------------------------------------------------------
        // 2) Listar sólo los préstamos de un usuario concreto
        // ------------------------------------------------------------
        // GET /api/prestamos/usuario/{usuarioId}
        // A veces sólo interesa ver lo que un usuario en particular
        //tiene prestado. Aquí recuperamos todo y filtramos manualmente.
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuario(int usuarioId)
        {
            // Traemos todos los préstamos
            List<Prestamo> todos = await this._context.Prestamos.ToListAsync();

            // Filtramos en un bucle
            List<Prestamo> prestamosUsuario = new List<Prestamo>();
            for (int i = 0; i < todos.Count; i = i + 1)
            {
                Prestamo actual = todos[i];
                if (actual.UsuarioId == usuarioId)
                {
                    prestamosUsuario.Add(actual);
                }
            }

            return Ok(prestamosUsuario);
        }

        // ---------------------------------------------
        // 3) Crear un nuevo préstamo
        // ---------------------------------------------
        // POST /api/prestamos
        // Recibe el ID de libro y el ID de usuario, asigna
        // la fecha actual y guarda el préstamo en la BD.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Prestamo prestamo)
        {
           

            // 2) Validaciones manuales
            List<Libro> libros = await _context.Libros.ToListAsync();
            bool libroExiste = false;
            for (int i = 0; i < libros.Count; i = i + 1)
            {
                if (libros[i].Id == prestamo.LibroId)
                {
                    libroExiste = true;
                    break;
                }
            }
            if (!libroExiste)
            {
                ModelState.AddModelError("LibroId", "El libro indicado no existe");
            }

            List<Usuario> usuarios = await _context.Usuarios.ToListAsync();
            bool usuarioExiste = false;
            for (int i = 0; i < usuarios.Count; i = i + 1)
            {
                if (usuarios[i].Id == prestamo.UsuarioId)
                {
                    usuarioExiste = true;
                    break;
                }
            }
            if (!usuarioExiste)
            {
                ModelState.AddModelError("UsuarioId", "El usuario indicado no existe");
            }

            List<Prestamo> activos = await _context.Prestamos.ToListAsync();
            for (int i = 0; i < activos.Count; i = i + 1)
            {
                Prestamo p = activos[i];
                if (p.LibroId == prestamo.LibroId && p.FechaDevolucion == null)
                {
                    ModelState.AddModelError("LibroId", "El libro ya está prestado y no ha sido devuelto");
                    break;
                }
            }

            if (prestamo.FechaPrestamo > DateTime.Now)
            {
                ModelState.AddModelError("FechaPrestamo", "La fecha de préstamo no puede ser futura");
            }

            if (prestamo.FechaDevolucion.HasValue && prestamo.FechaDevolucion.Value < prestamo.FechaPrestamo)
            {
                ModelState.AddModelError("FechaDevolucion",
                    "La fecha de devolución no puede ser anterior a la de préstamo");
            }

            // 3) ¡AHORA compruebo TODO el ModelState, ANTES de guardar!
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 4) Sólo si no hay ningún error, guardo el préstamo
            this._context.Prestamos.Add(prestamo);
            await this._context.SaveChangesAsync();

            // Devolvemos un mensaje
            return Ok("Prestamo realizado con éxito");
        }


        // ---------------------------------------------
        // 4) Registrar la devolución de un libro
        // ---------------------------------------------
        // POST /api/prestamos/devoluciones
        // Busca el préstamo activo (sin fecha de devolución)
        // para el libro y usuario indicados, y le pone la fecha.
        [HttpPost("devoluciones")]
        public async Task<IActionResult> Devolver([FromBody] Prestamo datos)
        {
            // Leemos todos los préstamos para buscar el correcto
            List<Prestamo> lista = await this._context.Prestamos.ToListAsync();
            Prestamo encontrado = null;

            // Recorremos hasta encontrar el préstamo activo
            for (int i = 0; i < lista.Count; i = i + 1)
            {
                Prestamo p = lista[i];
                if (p.UsuarioId == datos.UsuarioId &&
                    p.LibroId == datos.LibroId &&
                    p.FechaDevolucion == null)
                {
                    encontrado = p;
                    break;
                }
            }

            // Si no existe préstamo así, devolvemos un 404
            if (encontrado == null)
            {
                return NotFound("No existe préstamo activo para ese usuario y libro");
            }

            // Si lo encontramos, establecemos la fecha de devolución
            encontrado.FechaDevolucion = DateTime.Now;

            // Persistimos el cambio
            await this._context.SaveChangesAsync();

            // Devolvemos el objeto actualizado
            return Ok(encontrado);
        }
    }
}
