# Biblioteca API - Galería de Validaciones y Pruebas

Este README agrupa y describe las capturas de pantalla que muestran distintos estados del proyecto de la API de Biblioteca, enfocándose en validaciones de ISBN, pruebas de Postman y lógica de eliminación de préstamos. Para cada imagen, se incluye una breve explicación antes de insertarla.

---

## 1. Código sin validaciones de ISBN

Antes de mostrar cómo se ve la validación en tiempo de ejecución, revisa el fragmento de código donde está sin las reglas de validación para el ISBN.

![CÓDIGO ISBN SIN VALIDACIÓN](https://github.com/user-attachments/assets/3cbbd7a4-1911-4a73-8658-17207b4e5b19)

## 2. Validación de ISBN corregida

Aquí se muestra el código tras agregar las reglas de formato y unicidad de ISBN.

![Codigo ISBN validado](https://github.com/user-attachments/assets/0623437c-7874-4476-be11-6be5a3eb1115)

## 3. Código de eliminación y validación de préstamos

Este paso incluye la lógica que nos permitía borrar libros con préstamos activos.

![Codigo eliminar sin validad](https://github.com/user-attachments/assets/b887e274-19c6-4516-acf1-324080f393eb)

Y ahora podemos ver la lógica que impide borrar libros con préstamos activos.

![Código para eliminar validado](https://github.com/user-attachments/assets/5dc66ab5-8518-42a6-81a8-eee06e6d12a8)

## 4. Pruebas en Postman: Eliminación de libro sin validación

En esta captura se ejecuta la llamada DELETE antes de aplicar validaciones.
![Eliminar sin validar postman](https://github.com/user-attachments/assets/08f612db-17bd-4942-9789-ff531fb50430)


## 5. Pruebas en Postman: Eliminación de libro con préstamos activos

Aquí vemos el error que impide eliminar un libro con un préstamo aún sin devolver.

![Validación postman Pretamo ELiminar](https://github.com/user-attachments/assets/3ea7dd10-3cbe-4a85-9113-09175b679615)


## 6. Inserción de libro correcto en Postman

Ejemplo de petición POST que agrega un libro con todos los campos válidos.

![Ingreso Libro Correcto](https://github.com/user-attachments/assets/f48fc706-2b88-4b5c-8192-6ee467083458)


## 7. Manejo de ISBN en formato incorrecto

Captura de cómo responde la API cuando el ISBN no cumple con el patrón (longitud, prefijo o caracteres no válidos).

![ISBN mal formato](https://github.com/user-attachments/assets/92c0927b-a9e5-4758-8376-43f3c0975aa3)


## 8. Validación de ISBN no corregida

Ejemplo donde el mensaje de error no aparece aún cuando debería.

![POSTMAN ISBN SIN VALIDACIÓN](https://github.com/user-attachments/assets/d9cbb35f-b469-45c4-8ec6-25dae0e7506f)


## 9. Prueba exitosa de préstamo en Postman

Demostración de la llamada POST /api/prestamos al asignar un libro a un usuario.

![Postman Con exito prestamo](https://github.com/user-attachments/assets/95f97b33-22a3-4398-88cc-f99c8ae526fd)

Y posteriomente aquí está su código con las validaciones
![Codigo validacion prestamo 1](https://github.com/user-attachments/assets/8abbeb8d-9bc8-4669-8caa-121254f13777)

![Codigo validacion 2](https://github.com/user-attachments/assets/b1b5fcb4-df7a-4ae5-b8cd-b2fac6f53618)


## 10. Prueba con errores de préstamo en Postman

Resumen de los errores que se encontraban en préstamos.

![Postman Prestamos sin validaciones](https://github.com/user-attachments/assets/adea7e5f-9d33-4686-a9b3-9202259a24ec)
png)

Posteriormente tenemos aquí su correción
![Validaciones postman correctas](https://github.com/user-attachments/assets/db47b46b-f60e-4c10-97ce-6ebe1c571ad3)

---

Cada sección agrupa una etapa clave del flujo de trabajo: la implementación del código, la corrección de validaciones y las pruebas en Postman. Este readme nos ayuda a entender cómo evolucionó el proyecto hasta asegurar que no se omita ninguna regla de negocio.
