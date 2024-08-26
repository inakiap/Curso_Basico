using System;
using Curso_Basico.Helpers;


namespace Curso_Basico
{
    class Program
    {
        static void Main(string[] args)
        {

            //var _filePath = "C:\\Test\\nuevo5.txt"; // Cambia esto a la ruta de tu archivo    
            var _filePath = "C:\\Test\\nuevo5.txt"; // Cambia esto a la ruta de tu archivo    
            
            //Voy a leer mi fichero
            var lectorFicheros = new LectorFicheros();
            //lectorFicheros.Leer(_filePath);
            //lectorFicheros.Tamaño(_filePath);

            //TODO
            //Recoger el MIME de un archivo
            lectorFicheros.LeerMIME(_filePath);

            Console.ReadKey();
        }
    }


    
}