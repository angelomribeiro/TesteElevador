using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisElevador
{
    class Program
    {
        static void Main(string[] args)
        {
            ElevadorFactory elevador = new ElevadorFactory();
            // embarca 2 pessoas
            elevador.Embarcar(2);
            elevador.EscolherDestino(5);
            elevador.Desembarcar(1);
            elevador.EscolherDestino(2);
            elevador.Desembarcar(1);
            elevador.Embarcar(3);
            elevador.EscolherDestino(10);
            elevador.Desembarcar(2);
            Console.ReadKey();
        }
    }
}
