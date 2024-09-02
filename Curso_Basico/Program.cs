using System;
using Curso_Basico.Helpers;


namespace Curso_Basico
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Entrada.ObtenerVersion());

            //Entrada mientrada = new Entrada("Aitana");

            //Console.WriteLine($"El precio es: {mientrada.CualEsElPrecio()}");
            //Console.WriteLine($"El descuento es de: {mientrada.CualEsElDescuento()}");
            //Console.WriteLine($"El descuento extra es de: {mientrada.CualEsElDescuentoExtra()}");
            //Console.WriteLine($"El precio con descuento es: {mientrada.CualEsElPrecioFinal()}");


            //if (mientrada.EstaVendida())
            //{
            //    Console.WriteLine("La entrada Sí está vendida.");
            //}
            //else
            //{
            //    Console.WriteLine("La entrada No está vendida.");
            //}

            ////////////////

            string rutaTest = "C:\\Test\\";


            //Voy a leer mi fichero
            var lectorFicheros = new LectorFicheros();

            string[] archivos = lectorFicheros.LeerDirectorio(rutaTest);
            if (archivos != null )
            {
                Console.WriteLine($"Directorio: {rutaTest}");

                for (int i = 0; i < archivos.Length; i++)
                {
                    Console.WriteLine($"{i+1} {archivos[i]}");
                    //lectorFicheros.Leer(_filePath);
                    //lectorFicheros.Tamaño(archivos[i]);
                    lectorFicheros.LeerMIME(archivos[i]);
                    lectorFicheros.LeerMIME(archivos[i],1);
                    lectorFicheros.LeerMIME(archivos[i],2);
                    lectorFicheros.LeerMIME_FileSignatures(archivos[i]);
                    lectorFicheros.LeerMIME_TwentyDevs(archivos[i]);
                }
            }
            

            Console.ReadKey();
        }
    }


    
}