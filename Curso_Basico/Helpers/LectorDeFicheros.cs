using System;
using System.IO;
using System.Net.Mime;
using System.Collections.Generic;
using System.Globalization;


namespace Curso_Basico.Helpers
{
    public class LectorFicheros
    {
        private const string EXCEPCION_FICHERO_NO_EXISTE = "No existe fichero y no se puede leer. Obvio!";

        private bool _ExisteFichero(string rutaArchivo)
        {
            return File.Exists(rutaArchivo);
        }

        /// <summary>
        /// Leer un archivo y devuelve por consola si existe el contenido del fichero o mensaje de excepción.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        public void Leer(string rutaArchivo)
        {

            // Constantes
            const string EXCEPCION_FICHERO_SIN_CONTENIDO = "Existe pero no se puede leer el contenido.";

            // 0. Inicializar variables
            string resultado = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Leerlo
            if (existe)
            {
                try
                {
                    resultado = File.ReadAllText(rutaArchivo);
                }
                catch (Exception ex) { }
            }

            // 3. Construir el resultado
            resultado = (existe) ? resultado ?? EXCEPCION_FICHERO_SIN_CONTENIDO : EXCEPCION_FICHERO_NO_EXISTE;

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
            string resultado = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

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
            // fileInfo.Leng es un long
            if (fileInfo != null)
            {
                ulong miTamaño = (ulong)fileInfo.Length;
                resultado = _FormatearTamaño(miTamaño, 1);
            }

            //Tener en cuenta el tipo de operación de bus entrada/salida, de memoria

             resultado = (existe) ? (fileInfo == null) ? EXCEPCION_FICHERO_SIN_TAMAÑO : $"El tamaño del archivo \"{rutaArchivo}\" es {resultado}." : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }

        //Mejor versión, optimizada, no es exponencial
        public string _FormatearTamaño(ulong tamaño, uint decimales = 2)
        {
            //Variables               
            string[] S_SUFIJOS = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" }; //Esto se podría modificar, debería ser inmutable.
            uint indice = 0;
            decimal tamaño_tmp = tamaño;

            //Calculo del tamaño
            if (tamaño > 0)
            {
                indice = (uint)Math.Log(tamaño, 1024);
                tamaño_tmp /= (1L << ((int)indice * 10));
                if (Math.Round(tamaño_tmp, (int)decimales) >= 1000)
                {
                    indice += 1;
                    tamaño_tmp /= 1024;
                }
            }

            //Devuelvo
            return string.Format(CultureInfo.InvariantCulture, "{0:n" + decimales + "} {1}", tamaño_tmp, S_SUFIJOS[indice]);
        }

        //Solución buena pero que al contener un while en realidad es exponencial
        public string _FormatearTamaño2(ulong tamaño, uint decimales = 2)
        {
            //Variables
            string[] S_SUFIJOS = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int indice = 0;
            decimal tamaño_tmp = tamaño;

            //Calulo del tamaño
            if (tamaño > 0)
            {
                while (Math.Round(tamaño_tmp / 1024, (int)decimales) >= 1)
                {
                    tamaño_tmp /= 1024;
                    indice++;
                }
            }
            //Devuelvo
            return string.Format(CultureInfo.InvariantCulture, "{0:n" + decimales + "} {1}", tamaño_tmp, S_SUFIJOS[indice]);
        }

        //mi intento sin acabar...
        private string FormatearTamaño(ulong miTamaño)
        {
            string resultado = miTamaño.ToString() + " bytes.";
            //TODO pasar el resultado a la medida más adecuada, si son 10000 pues darlo en MB, etc... Según el número de bytes que obtenga como tamaño del archivo escribir el tamaño en la unidad más adecuada: Bytes, KBytes, MBytes, etc.
            //¿Cómo hacer el cálculo para obtener la unidad más adecuada según el tamaño?

            if (miTamaño >= 1024)
            {
                miTamaño /= 1024;
            
                if (miTamaño < 1024 )
                {
                    resultado = miTamaño.ToString() + " KBtytes.";
                }
            }

            //Si el número es inferior a 1024 escribes en Bytes
            //Si el número es superior a 1024 dividir entre 1024 -> KB
            //Si el resultado es inferior a 1024 escribir MB
            //Si el resultado es superior a 1024 dividir entre 1024 -> GB
            //y sucesivamente hasta el máximo
            //Devuelve un string con número redondeado a la unidad correspondiente con dos decimales
            return resultado;
        }

        //TODO
        /// <summary>
        /// Lee un archivo para conocer su MIME type y devuelve en consola el MIME o mensaje de excepción.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        internal void LeerMIME(string rutaArchivo)
        {
            // Constantes
            const string EXCEPCION_FICHERO_SIN_MIME = "El fichero existe pero su tipo MIME es desconocido.";

            // 0. Inicializar variables
            string resultado = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Obtener la información
            if (existe)
            {
                //resultado = _ObtenerMIMEDelContenido(rutaArchivo);
                //if (resultado == null)
                //{
                //    resultado = _ObtenerMIMEDeLaExtension(rutaArchivo);
                //}
            }

            // 3. Formatear la salida
            resultado = (existe) ? (resultado == null) ? EXCEPCION_FICHERO_SIN_MIME : $"El tipo de MIME del archivo \"{rutaArchivo}\" es {resultado}." : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }
    }
}
