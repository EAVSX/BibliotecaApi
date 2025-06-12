using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibliotecaApi.Datos;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    // Este controlador maneja todas las operaciones relacionadas con los libros
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        // El constructor recibe el contexto para poder usar la base de datos
        public LibrosController(BibliotecaContext context)
        {
            this._context = context;
        }

        // GET /api/libros
        // lista todos los libros que estén guardados en la base de datos
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Consulto la tabla de libros y paso el resultado a una lista
            List<Libro> lista = await this._context.Libros.ToListAsync();
            // Devuelvo la lista con código 200 OK
            return Ok(lista);
        }

        // GET /api/libros/{id}
        // Busca un libro específico por su identificador
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Intento encontrar el libro con el id dado
            Libro libro = await this._context.Libros.FindAsync(id);
            if (libro == null)
            {
                // Si no existe, devuelvo un mensaje claro de error
                return NotFound("No se encontró el libro solicitado");
            }
            // Si lo encuentra, lo devuelvo con código 200 OK
            return Ok(libro);
        }

        // POST /api/libros
        // Crea un nuevo libro en la base de datos
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Libro libro)
        {
            // Verifico que no exista otro libro con el mismo ISBN
            List<Libro> todos = await this._context.Libros.ToListAsync();
            for (int i = 0; i < todos.Count; i = i + 1)
            {
                if (todos[i].ISBN == libro.ISBN)
                {
                    // Si encuentro un ISBN igual, agrego un error al modelo
                    ModelState.AddModelError("ISBN", "Ya existe un libro con ese ISBN");
                    break;
                }
            }

            // Aquí compruebo si los datos cumplen las reglas de validación
            if (ModelState.IsValid == false)
            {
                // Si hay algún problema, devuelvo un 400 con los detalles
                return BadRequest(ModelState);
            }

            // Si todo está bien, agrego el libro y guardo los cambios
            this._context.Libros.Add(libro);
            await this._context.SaveChangesAsync();

            // Devuelvo 201 Created indicando dónde encontrar el recurso nuevo
            return CreatedAtAction("GetById",
                new { id = libro.Id }, libro);
        }

        // PUT /api/libros/{id}
        // Actualiza los datos de un libro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Libro actualizado)
        {
            // Primero veo si los datos enviados cumplen las reglas
            if (ModelState.IsValid == false)
            {
                // Si algo no es válido, aviso con un 400 Bad Request
                return BadRequest(ModelState);
            }

            // Busco el libro original en la base
            Libro original = await this._context.Libros.FindAsync(id);
            if (original == null)
            {
                // Si no lo encuentro, devuelvo un mensaje claro
                return NotFound("No se encontró el libro a actualizar");
            }

            // Si está, actualizo cada campo con los nuevos valores
            original.Titulo = actualizado.Titulo;
            original.Autor = actualizado.Autor;
            original.AnioPublicacion = actualizado.AnioPublicacion;
            original.ISBN = actualizado.ISBN;

            // Guardo los cambios en la base
            await this._context.SaveChangesAsync();
            // Devuelvo 204 No Content para indicar que todo salió bien
            return NoContent();
        }

        // DELETE /api/libros/{id}
        // Elimina un libro, pero solo si no tiene préstamos activos
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // 1) Busco el libro según el id
            Libro libro = await this._context.Libros.FindAsync(id);
            if (libro == null)
            {
                // Si no existe, informo con un mensaje claro
                return NotFound("No se encontró el libro a eliminar");
            }

            // 2) Reviso todos los préstamos para ver si alguno está activo
            List<Prestamo> prestamos = await this._context.Prestamos.ToListAsync();
            for (int i = 0; i < prestamos.Count; i = i + 1)
            {
                Prestamo p = prestamos[i];
                // Si encuentro un préstamo sin fecha de devolución, no dejo borrar
                if (p.LibroId == libro.Id && p.FechaDevolucion == null)
                {
                    return BadRequest("No se puede eliminar un libro con préstamos activos");
                }
            }

            // 3) Si no hay impedimentos, elimino el libro y guardo cambios
            this._context.Libros.Remove(libro);
            await this._context.SaveChangesAsync();

            // 4) Devuelvo 204 No Content para indicar éxito
            return NoContent();
        }
    }
}
