using xadrez_console.Domain.Entities;

namespace xadrez_console.Application.Services
{
    class PosicaoXadrezService
    {
        public char Coluna {  get; set; }
        public int Linha { get; set; }

        public PosicaoXadrezService(char coluna, int linha)
        {
            Coluna = coluna;
            Linha = linha;
        }

        public Posicao TransformaPosicaoDeMatriz()
        {
            return new Posicao(8 - Linha, Coluna - 'a');
        }

        public override string ToString()
        {
            return Coluna + Linha.ToString();
        }
    }
}
