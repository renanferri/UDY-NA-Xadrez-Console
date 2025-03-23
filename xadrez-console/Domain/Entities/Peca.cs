using xadrez_console.Application.Services;
using xadrez_console.Enums;

namespace xadrez_console.Domain.Entities
{
    abstract class Peca
    {
        public TabuleiroService Tab { get; protected set; }
        public CorEnum Cor { get; protected set; }
        public Posicao Posicao { get; set; }
        public int QteMovimentos { get; protected set; }
        

        public Peca(TabuleiroService tab, CorEnum cor)
        {
            Tab = tab;
            Cor = cor;
            Posicao = null;
            QteMovimentos = 0;
        }

        public void IncrementarQteMovimentos()
        {
            QteMovimentos++;
        }

        public void DecrementarQteMovimentos()
        {
            QteMovimentos--;
        }

        public bool ExisteMovimentosPossiveis()
        {
            bool[,] mat = MovimentosPossiveis();

            for (int i = 0; i < Tab.Linhas; i++)
            {
                for (int j = 0; j < Tab.Colunas; j++)
                {
                    if (mat[i, j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool MovimentoPossivel(Posicao pos)
        {
            return MovimentosPossiveis()[pos.Linha, pos.Coluna];
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
