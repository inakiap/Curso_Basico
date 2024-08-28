﻿using System;
using System.Collections.Generic;
using Curso_Basico.Helpers;


namespace Curso_Basico
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Entrada.ObtenerVersion());

            Entrada mientrada = new Entrada("Aitana");

            Console.WriteLine($"El precio es: {mientrada.CualEsElPrecio()}");
            Console.WriteLine($"El descuento es de: {mientrada.CualEsElDescuento()}");
            Console.WriteLine($"El descuento extra es de: {mientrada.CualEsElDescuentoExtra()}");
            Console.WriteLine($"El precio con descuento es: {mientrada.CualEsElPrecioFinal()}");
            

            if (mientrada.EstaVendida())
            {
                Console.WriteLine("La entrada Sí está vendida.");
            }
            else
            {
                Console.WriteLine("La entrada No está vendida.");
            }
            
            //List<string> archivos = new List<string>();
            //archivos.Add("C:\\Test\\no_existe.zip");
            //archivos.Add("C:\\Test\\Wf_iAhorro.zip");
            //archivos.Add("C:\\Test\\imagen.png");
            //archivos.Add("C:\\Test\\PresenceLogo.jpg");
            //archivos.Add("C:\\Test\\nuevo5.txt");
            //archivos.Add("C:\\Test\\Wf_iAhorro.zip");
            //archivos.Add("C:\\Test\\SRI_Deployment_Generic.xlsx"); // Error, dice que es un ZIP   
            

            ////Voy a leer mi fichero
            //var lectorFicheros = new LectorFicheros();
            ////lectorFicheros.Leer(_filePath);
            ////lectorFicheros.Tamaño(_filePath);

            //var a = lectorFicheros._FormatearTamaño(43432532,10);
            //var b = lectorFicheros._FormatearTamaño(ulong.MaxValue,10);
            //Console.WriteLine(a);
            //Console.WriteLine(b);
            ////TODO
            ////Recoger el MIME de un archivo
            ////foreach (var item in archivos)
            ////{
            ////    Console.WriteLine(item);
            ////    //lectorFicheros.Leer(_filePath);
            ////    lectorFicheros.Tamaño(item);
            ////    //lectorFicheros.LeerMIME(item);
            ////}

            Console.ReadKey();
        }
    }


    
}