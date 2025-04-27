using System;
using System.Collections.Generic;
using System.IO;

namespace GestorDeTareas
{
    // Enum para el tipo de tarea
    public enum TipoTarea
    {
        Persona, // Tarea relacionada con lo personal
        Trabajo, // Tarea relacionada con el trabajo
        Ocio     // Tarea relacionada con el ocio
    }

    // Clase para representar una tarea
    public class Tarea
    {
        public int Id { get; set; }             // Propiedad para el ID único de la tarea
        public string Nombre { get; set; }      // Propiedad para el nombre de la tarea
        public string Descripcion { get; set; } // Propiedad para la descripción de la tarea
        public TipoTarea Tipo { get; set; }     // Propiedad para el tipo de tarea (que es un Enum)
        public bool Prioridad { get; set; }     // Propiedad para la prioridad de la tarea (alta o baja)

        // Constructor que inicializa las propiedades de la tarea
        public Tarea(int id, string nombre, string descripcion, TipoTarea tipo, bool prioridad)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Tipo = tipo;
            Prioridad = prioridad;
        }

        // Método sobrescrito ToString para mostrar la tarea en formato legible
        public override string ToString()
        {
            // El método ToString devuelve una cadena de texto con la información de la tarea
            return $"{Id} - {Nombre} | {Descripcion} | {Tipo} | {(Prioridad ? "Alta" : "Baja")}";
        }
    }

    // Clase principal del programa
    class Program
    {
        // Lista para almacenar las tareas creadas
        private static List<Tarea> tareas = new List<Tarea>();

        // Variable para llevar el control del siguiente ID disponible
        private static int nextId = 1; 

        static void Main(string[] args)
        {
            // Al iniciar el programa, cargamos las tareas desde un archivo (si existe)
            CargarTareasDesdeFichero();

            // Bucle principal para mostrar el menú y permitir al usuario interactuar
            while (true)
            {
                MostrarMenu(); // Mostrar el menú de opciones
                var opcion = Console.ReadLine(); // Leer la opción seleccionada por el usuario

                // Ejecutar la opción seleccionada
                switch (opcion)
                {
                    case "1":
                        CrearTarea(); // Opción para crear una tarea nueva
                        break;
                    case "2":
                        BuscarTareasPorTipo(); // Opción para buscar tareas por tipo
                        break;
                    case "3":
                        EliminarTarea(); // Opción para eliminar una tarea por ID
                        break;
                    case "4":
                        ExportarTareas(); // Opción para exportar las tareas a un archivo
                        break;
                    case "5":
                        ImportarTareas(); // Opción para importar tareas desde un archivo
                        break;
                    case "0":
                        return; // Salir del programa
                    default:
                        Console.WriteLine("Opción no válida."); // Mensaje si la opción no es válida
                        break;
                }
            }
        }

        // Mostrar el menú de opciones
        static void MostrarMenu()
        {
            Console.Clear(); // Limpiar la pantalla
            Console.WriteLine("Gestor de Tareas Personales");
            Console.WriteLine("1. Crear tarea");
            Console.WriteLine("2. Buscar tareas por tipo");
            Console.WriteLine("3. Eliminar tarea");
            Console.WriteLine("4. Exportar tareas");
            Console.WriteLine("5. Importar tareas");
            Console.WriteLine("0. Salir");
            Console.Write("Elige una opción: ");
        }

        // Método para crear una nueva tarea
        static void CrearTarea()
        {
            Console.Clear();
            // Solicitar al usuario los detalles de la nueva tarea
            Console.Write("Nombre de la tarea: ");
            string nombre = Console.ReadLine(); // Leer el nombre

            Console.Write("Descripción de la tarea: ");
            string descripcion = Console.ReadLine(); // Leer la descripción

            // Solicitar el tipo de tarea y validar que sea uno de los tipos definidos en el enum
            Console.Write("Tipo de tarea (persona, trabajo, ocio): ");
            TipoTarea tipo;
            while (!Enum.TryParse(Console.ReadLine(), true, out tipo) || !Enum.IsDefined(typeof(TipoTarea), tipo))
            {
                Console.WriteLine("Tipo no válido, debe ser 'persona', 'trabajo' o 'ocio'. Intenta nuevamente.");
                Console.Write("Tipo de tarea (persona, trabajo, ocio): ");
            }

            // Solicitar la prioridad de la tarea (true o false)
            Console.Write("Prioridad (true para alta, false para baja): ");
            bool prioridad;
            while (!bool.TryParse(Console.ReadLine(), out prioridad))
            {
                Console.WriteLine("Valor no válido. Por favor, ingresa 'true' o 'false'.");
                Console.Write("Prioridad (true para alta, false para baja): ");
            }

            // Crear la tarea y agregarla a la lista
            var tarea = new Tarea(nextId++, nombre, descripcion, tipo, prioridad);
            tareas.Add(tarea); // Añadir la tarea a la lista
            Console.WriteLine("Tarea creada correctamente.");
            Console.ReadLine(); // Esperar a que el usuario presione una tecla
        }

        // Método para buscar tareas por tipo
        static void BuscarTareasPorTipo()
        {
            Console.Clear();
            // Solicitar el tipo de tarea para buscar
            Console.Write("Introduce el tipo de tarea para buscar (persona, trabajo, ocio): ");
            TipoTarea tipo;
            while (!Enum.TryParse(Console.ReadLine(), true, out tipo) || !Enum.IsDefined(typeof(TipoTarea), tipo))
            {
                Console.WriteLine("Tipo no válido. Intenta nuevamente.");
                Console.Write("Tipo de tarea (persona, trabajo, ocio): ");
            }

            // Filtrar las tareas de la lista según el tipo seleccionado
            var tareasFiltradas = tareas.FindAll(t => t.Tipo == tipo);

            // Mostrar las tareas encontradas
            Console.WriteLine($"Tareas de tipo {tipo}:");
            foreach (var tarea in tareasFiltradas)
            {
                Console.WriteLine(tarea); // Llamar al método ToString de la clase Tarea
            }

            Console.ReadLine(); // Esperar a que el usuario presione una tecla
        }

        // Método para eliminar una tarea por ID
        static void EliminarTarea()
        {
            Console.Clear();
            Console.Write("Introduce el id de la tarea a eliminar: ");
            int id;
            // Validar que el ID ingresado sea válido y exista en la lista de tareas
            while (!int.TryParse(Console.ReadLine(), out id) || tareas.Find(t => t.Id == id) == null)
            {
                Console.WriteLine("ID no válido o tarea no encontrada. Intenta nuevamente.");
                Console.Write("Introduce el id de la tarea a eliminar: ");
            }

            // Buscar la tarea y eliminarla de la lista
            var tareaAEliminar = tareas.Find(t => t.Id == id);
            tareas.Remove(tareaAEliminar);
            Console.WriteLine("Tarea eliminada correctamente.");
            Console.ReadLine(); // Esperar a que el usuario presione una tecla
        }

        // Método para exportar las tareas a un archivo de texto
        static void ExportarTareas()
        {
            Console.Clear();
            string filePath = "tareas.txt"; // Ruta del archivo
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                // Escribir cada tarea en una línea del archivo
                foreach (var tarea in tareas)
                {
                    sw.WriteLine($"{tarea.Id};{tarea.Nombre};{tarea.Descripcion};{tarea.Tipo};{tarea.Prioridad}");
                }
            }
            Console.WriteLine($"Tareas exportadas a {filePath}");
            Console.ReadLine(); // Esperar a que el usuario presione una tecla
        }

        // Método para importar tareas desde un archivo de texto
        static void ImportarTareas()
        {
            Console.Clear();
            string filePath = "tareas.txt"; // Ruta del archivo
            if (File.Exists(filePath))
            {
                // Si el archivo existe, leer las tareas
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    // Leer cada línea del archivo
                    while ((line = sr.ReadLine()) != null)
                    {
                        var parts = line.Split(';'); // Separar los valores de cada tarea por ';'
                        if (parts.Length == 5)
                        {
                            int id = int.Parse(parts[0]);  // Convertir el ID
                            string nombre = parts[1];       // Obtener el nombre
                            string descripcion = parts[2];  // Obtener la descripción
                            TipoTarea tipo = (TipoTarea)Enum.Parse(typeof(TipoTarea), parts[3]); // Obtener el tipo
                            bool prioridad = bool.Parse(parts[4]); // Obtener la prioridad

                            // Crear la tarea y agregarla a la lista
                            var tarea = new Tarea(id, nombre, descripcion, tipo, prioridad);
                            tareas.Add(tarea);
                            nextId = id + 1; // Asegurar que el siguiente ID sea único
                        }
                    }
                }
                Console.WriteLine("Tareas importadas correctamente.");
            }
            else
            {
                Console.WriteLine("El fichero de tareas no existe.");
            }
            Console.ReadLine(); // Esperar a que el usuario presione una tecla
        }

        // Método para cargar las tareas desde el archivo al iniciar el programa
        static void CargarTareasDesdeFichero()
        {
            string filePath = "tareas.txt"; // Ruta del archivo
            if (File.Exists(filePath)) // Comprobar si el archivo existe
            {
                // Si el archivo existe, leer las tareas
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var parts = line.Split(';'); // Separar los valores por ';'
                        if (parts.Length == 5)
                        {
                            int id = int.Parse(parts[0]);
                            string nombre = parts[1];
                            string descripcion = parts[2];
                            TipoTarea tipo = (TipoTarea)Enum.Parse(typeof(TipoTarea), parts[3]);
                            bool prioridad = bool.Parse(parts[4]);

                            var tarea = new Tarea(id, nombre, descripcion, tipo, prioridad);
                            tareas.Add(tarea);
                            nextId = id + 1; // Asegurarse de que el siguiente ID esté actualizado
                        }
                    }
                }
            }
        }
    }
}
