using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models
{
    // Esta clase representa a una persona que puede tomar prestados libros de la biblioteca
    public class Usuario
    {
        
        private int _id;
       
        private string _nombre;
       
        private string _email;
        
        private List<Prestamo> _prestamos;

        [Key]
        // Identificador único del usuario en la base de datos
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Required]
        [StringLength(100)]
        // Nombre completo del usuario, este dato es obligatorio y no puede superar los 100 caracteres
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        [Required]
        [StringLength(200)]
        [EmailAddress]
        // Correo electrónico válido del usuario, obligatorio y con máximo de 200 caracteres
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        // Lista de préstamos que ha realizado este usuario
        // Se completa desde el controlador cuando es necesario mostrar los detalles
        public List<Prestamo> Prestamos
        {
            get { return _prestamos; }
            set { _prestamos = value; }
        }
    }
}
