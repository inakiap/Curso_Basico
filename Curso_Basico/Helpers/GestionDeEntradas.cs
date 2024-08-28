using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso_Basico.Helpers
{
    class GestionDeEntradas
    {

    }

    public class Entrada
    {
        private int ID;
        private const string Version = "Version Origenial";
        private string Codigo;
        private string Concierto;
        private decimal Precio;
        private bool Vendida = false;
        private int Descuento;
        private int DescuentoExtra;
        
        private Entrada()
        {
            
        }

        //Crear entradas por el promotor
        public Entrada(decimal precio, string concierto, int descuento = 0)
        {
            ID = 1;
            Codigo = "A111";
            Concierto = concierto;
            Precio = precio;
            Vendida = false;
            Descuento = descuento;
        }

        public Entrada(string concierto) : this( 100, concierto)
        {
            //El concierto de vetusta lleva un 10% de descuento
            if (Concierto.Equals("Vetusta"))
            {
                Descuento = 10;
                DescuentoExtra = 5;
            }

            //El concierto de Aitana lleva un 5% de descuento extra
            if (Concierto.Equals("Aitana"))
            {
                Descuento = 0;
                DescuentoExtra = 5;
            }
        }

        public bool EstaVendida()
        {
            return Vendida;
        }

        public int CualEsElDescuento()
        {
            return Descuento;
        }

        public int CualEsElDescuentoExtra()
        {
            return DescuentoExtra;
        }

        public decimal CualEsElPrecio()
        {
            return Precio;
        }

        public decimal CualEsElPrecioFinal()
        {
            return CalcularDescuento(this.CualEsElDescuentoExtra());
        }

        private decimal CalcularDescuento(int extra = 0)
        {
            // Init variables
            decimal precio = this.CualEsElPrecio();
            int descuento = this.CualEsElDescuento() + extra; //Los descuentos no se suelen aplicar así.

            if (descuento > 0)
            {
                precio -= ((precio * descuento) / 100); //Se puede simplificar porque tiene 2 operaciones!!!!!!
            }
            return precio;
        }

        public static string ObtenerVersion()
        {
            return Version;
        }

    }
}
