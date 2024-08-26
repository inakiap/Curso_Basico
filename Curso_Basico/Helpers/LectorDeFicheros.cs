using System;
using System.IO;

namespace Curso_Basico.Helpers
{
    public class LectorFicheros
    {
        private const string EXCEPCION_FICHERO_NO_EXISTE = "No existe fichero y no se puede leer. Obvio!";

        private bool _Existe_Fichero(string ruta)
        {
            return File.Exists(ruta);
        }

        /// <summary>
        /// Leer un archivo y devuelve por consola si existe el contenido del fichero o mensaje de excepción.
        /// </summary>
        /// <param name="filePath"></param>
        public void Leer(string filePath)
        {

            // Constantes
            const string EXCEPCION_FICHERO_SIN_CONTENIDO = "Existe pero no se puede leer el contenido.";

            // 0. Inicializar variables
            string resultado = null;

            // 1. Comprobar si el fichero existe
            bool existe = _Existe_Fichero(filePath);

            // 2. Leerlo
            if (existe)
            {
                try
                {
                    resultado = File.ReadAllText(filePath);
                }
                catch (Exception ex) { }
            }

            // 3. Construir el resultado
            resultado = (existe) ? (resultado == null) ? EXCEPCION_FICHERO_SIN_CONTENIDO : resultado : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.Write("La función devuelve: " + resultado);

        }

        /// <summary>
        /// Comprueba el tamaño del archivo y devuelve en consola el tamaño o mensaje de excepción.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        public void Tamaño(string rutaArchivo)
        {
            // Constantes
            const string EXCEPCION_FICHERO_SIN_TAMAÑO = "El fichero existe pero hubo un problema para obtener su tamaño.";

            // 0. Inicializar variables
            FileInfo fileInfo = null;

            // 1. Comprobar si el fichero existe
            bool existe = _Existe_Fichero(rutaArchivo);

            // 2. Obtener la información
            if (existe)
            {
                try
                {
                    fileInfo = new FileInfo(rutaArchivo);
                }
                catch (Exception ex)
                {

                }
            }

            // 3. Formatear la salida
            string resultado = (existe) ? (fileInfo == null) ? EXCEPCION_FICHERO_SIN_TAMAÑO : $"El tamaño del archivo \"{rutaArchivo}\" es {fileInfo.Length} bytes." : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }

        /// <summary>
        /// Lee un archivo para conocer el MIME y devuelve en consola el MIME o mensaje de excepción.
        /// </summary>
        /// <param name="filePath"></param>
        internal void LeerMIME(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
