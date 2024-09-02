using System;
using System.IO;
using MimeDetective;
using FileSignatures;
using TwentyDevs.MimeTypeDetective;
using System.Collections.Immutable;
using System.Globalization;



namespace Curso_Basico.Helpers
{
    public class LectorFicheros
    {
        private const string EXCEPCION_FICHERO_NO_EXISTE = "No existe fichero.";

        private bool _ExisteFichero(string rutaArchivo)
        {
            return File.Exists(rutaArchivo);
        }

        private bool _ExisteDirectorio(string rutaArchivo)
        {
            return Directory.Exists(rutaArchivo);
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

             resultado = (existe) ? (fileInfo == null) ? EXCEPCION_FICHERO_SIN_TAMAÑO : $"El tamaño  es {resultado}." : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }

        //Mejor versión, optimizada, no es exponencial
        private string _FormatearTamaño(ulong tamaño, uint decimales = 2)
        {
            //Variables               
            ImmutableArray<string> S_SUFIJOS = ImmutableArray.Create( "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" ); //Esto se podría modificar, debería ser inmutable.
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

            S_SUFIJOS = S_SUFIJOS.Remove("byte");

            //Devuelvo
            return string.Format(CultureInfo.InvariantCulture, "{0:n" + decimales + "} {1}", tamaño_tmp, S_SUFIJOS.ItemRef((int)indice));
        }

        //Solución buena pero que al contener un while en realidad es exponencial
        private string _FormatearTamaño2(ulong tamaño, uint decimales = 2)
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

        private byte[] _ObtenerContenidoEnBytes(string rutaArchivo)
        {
            byte[] buffer = null;

            try
            {
                using (FileStream fs = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read))
                {
                    // Leer el contenido del archivo
                    buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                }
            }
            catch (Exception)
            {

            }
            return buffer;
        }

        private Stream _ObtenerContenidoEnStream(string rutaArchivo)
        {
            Stream stream= null;

            try
            {
                stream = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);
                
            }
            catch (Exception)
            {

            }
            return stream;
        }

        /// <summary>
        /// Lee un archivo para conocer su MIME type y devuelve en consola el MIME.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        public void LeerMIME(string rutaArchivo, int modo = 0)
        {
            const string EXCEPCION_NO_HAY_CONTENIDO = "No se ha podido leer el contenido del fichero.";
            const string EXCEPCION_NO_SE_RECONOCE_EL_TIPO = "MIME desconocido.";

            // 0. Inicializar variables
            string resultado = null;
            ImmutableArray<byte> contenido;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Obtener la información
            if (existe)
            {
                // 3. Comprobar si se puede leer el contenido del archivo
                contenido = _ObtenerContenidoEnBytes(rutaArchivo).ToImmutableArray();

                if (contenido != null)
                {
                    ContentInspector Inspector = ObtenerInspector(modo);

                    var definicion = Inspector.Inspect(contenido);
                    if (definicion.Length > 0)
                    {
                        resultado = definicion.ItemRef(0).Definition.File.MimeType;
                    }
                }

            }

            // 3. Formatear la salida
            string metodo = "Default";
            switch (modo)
            {
                case 1:
                    metodo = "Condensed";
                    break;
                case 2:
                    metodo = "Exhaustive";
                    break;
                default:
                    break;
            }
            resultado = (existe) ? (contenido == null) ? EXCEPCION_NO_HAY_CONTENIDO : (resultado == null) ? EXCEPCION_NO_SE_RECONOCE_EL_TIPO :  $"-- {metodo}: {resultado}" : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }

        /// <summary>
        /// Lee un archivo para conocer su MIME type y devuelve en consola el MIME.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        public void LeerMIME_FileSignatures(string rutaArchivo)
        {
            const string EXCEPCION_NO_HAY_CONTENIDO = "No se ha podido leer el contenido del fichero.";
            const string EXCEPCION_NO_SE_RECONOCE_EL_TIPO = "MIME desconocido.";

            // 0. Inicializar variables
            string resultado = null;
            Stream contenido = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Obtener la información
            if (existe)
            {
                // 3. Comprobar si se puede leer el contenido del archivo
                using ( contenido = _ObtenerContenidoEnStream(rutaArchivo))
                {
                    if (contenido != null)
                    {
                        var inspector = new FileFormatInspector();
                        var formato = inspector.DetermineFileFormat(contenido);
                        if (formato != null)
                        {
                            resultado = formato.MediaType;
                        }
                    }
                }
            }

            // 3. Formatear la salida
            resultado = (existe) ? (contenido == null) ? EXCEPCION_NO_HAY_CONTENIDO : (resultado == null) ? EXCEPCION_NO_SE_RECONOCE_EL_TIPO : $"-- FileSignature: {resultado}" : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }

        /// <summary>
        /// Lee un archivo para conocer su MIME type y devuelve en consola el MIME.
        /// </summary>
        /// <param name="rutaArchivo"></param>
        public void LeerMIME_TwentyDevs(string rutaArchivo)
        {
            const string EXCEPCION_NO_HAY_CONTENIDO = "No se ha podido leer el contenido del fichero.";
            const string EXCEPCION_NO_SE_RECONOCE_EL_TIPO = "MIME desconocido.";

            // 0. Inicializar variables
            string resultado = null;
            Stream contenido = null;

            // 1. Comprobar si el fichero existe
            bool existe = _ExisteFichero(rutaArchivo);

            // 2. Obtener la información
            if (existe)
            {
                // 3. Comprobar si se puede leer el contenido del archivo
                using (contenido = _ObtenerContenidoEnStream(rutaArchivo))
                {
                    if (contenido != null)
                    {
                        MimeTypeInfo mimeInfo = MimeTypeDetection.GetMimeType(contenido);
                        if (mimeInfo != null)
                        {
                            resultado = mimeInfo.MimeType;
                        }
                    }
                }
            }

            // 3. Formatear la salida
            resultado = (existe) ? (contenido == null) ? EXCEPCION_NO_HAY_CONTENIDO : (resultado == null) ? EXCEPCION_NO_SE_RECONOCE_EL_TIPO : $"-- TwentyDevs: {resultado}" : EXCEPCION_FICHERO_NO_EXISTE;

            // 4. Mostrar el resultado
            Console.WriteLine(resultado);
        }

        private static ContentInspector ObtenerInspector(int modo)
        {
            ContentInspector inspector = null;
            switch (modo)
            {
                case 1: //Condensed
                    inspector = new ContentInspectorBuilder()
                    {
                        Definitions = new MimeDetective.Definitions.CondensedBuilder()
                        {
                            UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial
                        }.Build()
                    }.Build();
                    break;
                case 2:
                    inspector = new ContentInspectorBuilder()
                    {
                        Definitions = new MimeDetective.Definitions.ExhaustiveBuilder()
                        {
                            UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial
                        }.Build()
                    }.Build();
                    break;
                default: //Default
                    inspector = new ContentInspectorBuilder()
                    {
                        Definitions = MimeDetective.Definitions.Default.All()
                    }.Build();
                    break;
            }
            return inspector;
        }

        /// <summary>
        /// Busca en una ruta dada y devuelve una lista de rutas de todos los archivos que hay, si el directorio no existe devuelve null, si no encuentra nada devuelve una matriz vacia.
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public string[] LeerDirectorio(string ruta)
        {
            // 0. Inicializar variables
            string[] resultado = null;

            // 1. Comprobar si el directorio existe
            bool existe = _ExisteDirectorio(ruta);

            // 2. Si existe obtener los archivos
            if (existe)
            {
                try
                {
                    resultado = Directory.GetFiles(ruta, "*.*", SearchOption.AllDirectories);
                }
                catch (Exception)
                {
                }
            }
            // 3. Formatear la salida
            return resultado;
        }


    }
}
