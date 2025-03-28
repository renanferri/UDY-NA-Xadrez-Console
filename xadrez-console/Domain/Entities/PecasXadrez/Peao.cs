﻿using xadrez_console.Application.Services;
using xadrez_console.Enums;


namespace xadrez_console.Domain.Entities.PecasXadrez
{
    class Peao : Peca
    {
        private PartidaDeXadrezService _partida;

        public Peao(TabuleiroService tab, CorEnum cor, PartidaDeXadrezService partida) : base(tab, cor)
        {
            _partida = partida;
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
                case CorEnum.Branca:

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

                    // #jogadaespecial en passant
                    if (Posicao.Linha == 3)
                    {
                        Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                        if (Tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) && Tab.Peca(esquerda) == _partida.VulneravelEnPassant)
                        {
                            mat[esquerda.Linha - 1, esquerda.Coluna] = true;
                        }

                        Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                        if (Tab.PosicaoValida(direita) && ExisteInimigo(direita) && Tab.Peca(direita) == _partida.VulneravelEnPassant)
                        {
                            mat[direita.Linha - 1, direita.Coluna] = true;
                        }
                    }

                    break;
                case CorEnum.Preta:

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

                    // #jogadaespecial en passant
                    if (Posicao.Linha == 4)
                    {
                        Posicao esquerda = new Posicao(Posicao.Linha, Posicao.Coluna - 1);
                        if (Tab.PosicaoValida(esquerda) && ExisteInimigo(esquerda) && Tab.Peca(esquerda) == _partida.VulneravelEnPassant)
                        {
                            mat[esquerda.Linha + 1, esquerda.Coluna] = true;
                        }

                        Posicao direita = new Posicao(Posicao.Linha, Posicao.Coluna + 1);
                        if (Tab.PosicaoValida(direita) && ExisteInimigo(direita) && Tab.Peca(direita) == _partida.VulneravelEnPassant)
                        {
                            mat[direita.Linha + 1, direita.Coluna] = true;
                        }
                    }

                    break;
            }

            return mat;
        }
    }
}
