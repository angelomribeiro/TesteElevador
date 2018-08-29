using SisElevador.Enums;

namespace SisElevador.Interfaces
{
    public interface IElevador
    {
        void EscolherDestino(int andar);
        void ImprimirStatus();
        void Embarcar(int numPassageiros);
        void Desembarcar(int numPassageiros);
    }
}
