using System;
using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Models
{
    // Representa un libro que está en la biblioteca
    public class Libro
    {
        
        private int _id;
       
        private string _titulo;
        
        private string _autor;
        
        private int _anioPublicacion;
       
        private string _isbn;

        [Key]
        // Identificador único del libro en la base de datos
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "El título debe tener entre 3 y 200 caracteres")]
        
        public string Titulo
        {
            get { return _titulo; }
            set { _titulo = value; }
        }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(100,
            ErrorMessage = "El autor no debe exceder 100 caracteres")]
        
        public string Autor
        {
            get { return _autor; }
            set { _autor = value; }
        }

        [Required(ErrorMessage = "El año de publicación es obligatorio")]
        [Range(1500, 2100,
            ErrorMessage = "El año debe estar entre 1500 y 2100")]
        // Año en que se publicó el libro; debe estar en el rango aceptable
        public int AnioPublicacion
        {
            get { return _anioPublicacion; }
            set { _anioPublicacion = value; }
        }

        [Required(ErrorMessage = "El ISBN es obligatorio")]
        [StringLength(13, ErrorMessage = "El ISBN debe tener exactamente 13 dígitos")]
        [RegularExpression("^(978|979)\\d{10}$",
            ErrorMessage = "El ISBN debe comenzar con 978 o 979 y contener solo dígitos")]
        // Código ISBN del libro; inicia con 978 o 979 y tiene 13 dígitos
        public string ISBN
        {
            get { return _isbn; }
            set { _isbn = value; }
        }
    }
}
