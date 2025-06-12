using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BibliotecaApi.Datos;
using BibliotecaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Controllers
{
    // Este controlador maneja y tiene todo lo relacionado con usuarios:
    // creación de cuentas, listado y consulta de sus préstamos.
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly BibliotecaContext _context; // Para acceder a Usuarios y Préstamos

        // Recibimos el contexto por inyección para poder usar EF Core
        public UsuariosController(BibliotecaContext context)
        {
            this._context = context;
        }

        // ---------------------------------------------
        // 1) Listar todos los usuarios
        // ---------------------------------------------
        // GET /api/usuarios
        // Devuelve la lista completa de registros de usuario.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Usuario> lista = await this._context.Usuarios.ToListAsync();
            return Ok(lista);
        }

        // ---------------------------------------------
        // 2) Crear un usuario nuevo
        // ---------------------------------------------
        // POST /api/usuarios
        // Solo necesita nombre y correo; validamos y guardamos.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Usuario usuario)
        {
            if (ModelState.IsValid == false)
            {
                // Si el correo no es válido o falta nombre, devolvemos un 400
                return BadRequest(ModelState);
            }

            this._context.Usuarios.Add(usuario);
            await this._context.SaveChangesAsync();

            // Devolvemos 201 Created con el usuario recién creado
            return CreatedAtAction("GetAll", null, usuario);
        }

        // ---------------------------------------------
        // 3) Ver los préstamos de un usuario
        // ---------------------------------------------
        // GET /api/usuarios/{id}/prestamos
        // Nos permite saber qué libros tiene prestados el usuario.
        [HttpGet("{id}/prestamos")]
        public async Task<IActionResult> GetPrestamosByUsuario(int id)
        {
            Usuario user = await this._context.Usuarios.FindAsync(id);
            if (user == null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Leemos todos los préstamos y filtramos en bucle
            List<Prestamo> todos = await this._context.Prestamos.ToListAsync();
            List<Prestamo> resultado = new List<Prestamo>();
            for (int i = 0; i < todos.Count; i = i + 1)
            {
                Prestamo p = todos[i];
                if (p.UsuarioId == user.Id)
                {
                    resultado.Add(p);
                }
            }

            return Ok(resultado);
        }
    }
}
