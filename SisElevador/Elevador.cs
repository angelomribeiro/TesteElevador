using SisElevador.Enums;
using SisElevador.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisElevador
{
    public class ElevadorFactory : IElevador
    {
        #region Propriedades
        private int AndarAtual { get; set; }
        private int QtdMaxPessoas { get; set; }
        private int NumMaxAndares { get; set; }
        private int NumPessoasAtual { get; set; }
        private List<Destino> Rota { get; set; }
        private StatusElevador StatusAtualElevador { get; set; }
        private StatusPorta StatusAtualPorta { get; set; }
        #endregion

        public ElevadorFactory()
        {
            AndarAtual = 0;
            QtdMaxPessoas = 10;
            NumMaxAndares = 10;
            StatusAtualElevador = StatusElevador.Parado;
            StatusAtualPorta = StatusPorta.Aberta;
        }

        public ElevadorFactory(int qtdMaxPessoas, int numMaxAndares)
        {
            if (qtdMaxPessoas <= 0) 
            {
                throw new ArgumentException("Qtd máxima de pessoas inválida.");
            }
            else if (numMaxAndares <= 0)
            {
                throw new ArgumentException("Número máximo de andares inválido.");
            }
            else
            {
                AndarAtual = 0;
                QtdMaxPessoas = qtdMaxPessoas;
                NumMaxAndares = numMaxAndares;
                StatusAtualElevador = StatusElevador.Parado;
                StatusAtualPorta = StatusPorta.Aberta;
            }
        }

        #region Metodos

        /// <summary>
        /// Escolhe o destino
        /// </summary>
        /// <param name="andar"></param>
        /// <returns></returns>
        public void EscolherDestino(int andar)
        {
            // verifica se porta esta aberta
            if (StatusAtualPorta != StatusPorta.Aberta)
                throw new InvalidOperationException("Não é possivel selecionar um andar com a porta fechada");

            // verifica se o andar é valido
            if (andar < 0)
            {
                // informa que o andar é inválido
                throw new ArgumentException("O andar informado deve ser superior ou igual a zero.");
            } 
            else if (andar != AndarAtual) 
            {
                if (Rota == null)
                    Rota = new List<Destino>();

                if (AndarAtual == 0 || andar > AndarAtual) 
                {
                    // adiciona destino na lista de subida
                    Rota.Add(new Destino { Sentido = Sentido.Subir, Andar = andar });
                } else {
                    // adiciona na lista de descida
                    Rota.Add(new Destino { Sentido = Sentido.Descer, Andar = andar });
                }
            }

            Console.WriteLine(string.Format("Novo destino adicionado, {0}º andar.", andar));
            // fecha porta
            FecharPorta();
        }

        /// <summary>
        /// Retorna o status geral do elevador
        /// </summary>
        /// <returns></returns>
        public void ImprimirStatus()
        {
            Console.WriteLine(string.Format("Porta: {0} - Situação: {1}", StatusAtualPorta.ToString(), StatusAtualElevador.ToString()));
        }

        /// <summary>
        /// Embarca pessoas no elevador
        /// </summary>
        /// <param name="numPessoas">Número de pessoas</param>
        /// <returns></returns>
        public void Embarcar(int numPessoas)
        {
            if (numPessoas < 0)
            {
                throw new ArgumentException("Número de passageiro(s) inválido.");
            }
            else if ((numPessoas + NumPessoasAtual) > QtdMaxPessoas)
            {
                throw new InvalidOperationException("Excedeu o número máximo de pessoas permitidas");
            }
            else 
            {
                NumPessoasAtual += numPessoas;
            }

            Console.WriteLine(string.Format("{0} pessoa(s) nova(s) no elevador.", numPessoas));
        }

        /// <summary>
        /// Desembarca pessoas do elevador
        /// </summary>
        /// <param name="numPessoas">Número de pessoas</param>
        /// <returns></returns>
        public void Desembarcar(int numPessoas)
        {
            if (numPessoas < 0)
            {
                throw new ArgumentException("Número de pessoa(s) inválido.");
            }
            else if ((NumPessoasAtual - numPessoas) < 0)
            {
                throw new InvalidOperationException("Número de pessoas desembarcando é inválida");
            }
            else
            {
                NumPessoasAtual -= numPessoas;
            }

            Console.WriteLine(string.Format("{0} pessoa(s) no elevador.", NumPessoasAtual));
        }
        #endregion

        #region Metodos Privados

        /// <summary>
        /// Fechar porta
        /// </summary>
        /// <returns></returns>
        private void FecharPorta()
        {
            if (Rota == null || Rota.Count == 0)
                throw new InvalidOperationException("Porta não pode ser fechada sem uma rota definida.");

            Console.WriteLine("Porta Fechada");

            // mover
            Mover();
        }

        /// <summary>
        /// Abre a porta
        /// </summary>
        /// <returns></returns>
        private void AbirPorta()
        {
            // altera status do elevador para parado
            StatusAtualElevador = StatusElevador.Parado;
            StatusAtualPorta = StatusPorta.Aberta;
            // remove andar da rota
            RemoverRota(AndarAtual);
            Console.WriteLine("Parando elevador e abrindo a porta.");
        }

        /// <summary>
        /// Remove andar da rota
        /// </summary>
        /// <param name="andar">Andar a ser removido</param>
        private void RemoverRota(int andar)
        {
            Rota.Remove(Rota.Find(w => w.Andar == andar));
        }

        /// <summary>
        /// Mover elevador
        /// </summary>
        private void Mover()
        {
            Destino proxDestino = Rota.FirstOrDefault();
            if (AndarAtual < proxDestino.Andar)
                StatusAtualElevador = StatusElevador.Subindo;
            else
                StatusAtualElevador = StatusElevador.Descendo;

            while(AndarAtual != proxDestino.Andar) {

                if (StatusAtualElevador == StatusElevador.Subindo)
                    AndarAtual++;
                else
                    AndarAtual--;

                Console.WriteLine(string.Format("Estamos no {0}º andar", AndarAtual));
            }

            AbirPorta();
        }
        #endregion        
    }
}
