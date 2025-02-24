
using tabuleiro;

namespace xadrez
{
    class Peao : Peca
    {
        public Peao(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        public override string ToString()
        {
            return "P";
        }

        private bool ExisteInimigo(Posicao pos)
        {
            Peca p = Tab.Peca(pos);
            return p != null && p.Cor != Cor;
        }

        private bool Livre(Posicao pos)
        {
            return Tab.Peca(pos) == null;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];

            Posicao pos = new Posicao(0, 0);

            switch (Cor)
            {
                case Cor.Branca:
                    
                    pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
                    if (Tab.PosicaoValida(pos) && Livre(pos))
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    pos.DefinirValores(Posicao.Linha - 2, Posicao.Coluna);
                    if (Tab.PosicaoValida(pos) && Livre(pos) && QteMovimentos == 0)
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
                    if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    break;
                case Cor.Preta:

                    pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
                    if (Tab.PosicaoValida(pos) && Livre(pos))
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    pos.DefinirValores(Posicao.Linha + 2, Posicao.Coluna);
                    if (Tab.PosicaoValida(pos) && Livre(pos) && QteMovimentos == 0)
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
                    if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
                    if (Tab.PosicaoValida(pos) && ExisteInimigo(pos))
                    {
                        mat[pos.Linha, pos.Coluna] = true;
                    }

                    break;
            }

            return mat;
        }
    }
}
