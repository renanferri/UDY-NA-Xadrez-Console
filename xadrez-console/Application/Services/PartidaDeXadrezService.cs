using xadrez_console.Domain.Entities;
using xadrez_console.Domain.Entities.PecasXadrez;
using xadrez_console.Enums;
using xadrez_console.Exceptions;

namespace xadrez_console.Application.Services
{
    class PartidaDeXadrezService
    {
        public TabuleiroService Tab { get; private set; }
        public int Turno { get; private set; }
        public CorEnum JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        public bool Xeque { get; private set; }

        public Peca VulneravelEnPassant { get; private set; }

        private HashSet<Peca> _pecas;

        private HashSet<Peca> _capturadas;


        public PartidaDeXadrezService()
        {
            Tab = new TabuleiroService(8, 8);
            Turno = 1;
            JogadorAtual = CorEnum.Branca;
            Terminada = false;
            Xeque = false;
            VulneravelEnPassant = null;
            _pecas = new HashSet<Peca>();
            _capturadas = new HashSet<Peca>();
            ColocarPecas();
        }

        public void ValidarPosicaoDeOrigem(Posicao pos)
        {
            if (Tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }

            if (Tab.Peca(pos).Cor != JogadorAtual)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }

            if (!Tab.Peca(pos).ExisteMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.Peca(origem).MovimentoPossivel(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturda = ExecutaMovimento(origem, destino);            

            if (EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturda);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = Tab.Peca(destino);

            // #jogadaespecial promocao
            if (p is Peao)
            {
                if (p.Cor == CorEnum.Branca && destino.Linha == 0 || p.Cor == CorEnum.Preta && destino.Linha == 7)
                {
                    p = Tab.RetirarPeca(destino);
                    _pecas.Remove(p);
                    Peca dama = new Dama(Tab, p.Cor);
                    Tab.ColocarPeca(dama, destino);
                    _pecas.Add(dama);
                }
            }

            if (EstaEmXeque(Adversario(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if (EstaEmXequeMate(Adversario(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }

            //#jogadaespecial en passant
            VulneravelEnPassant = null;
            if (p is Peao && (destino.Linha == origem.Linha - 2 || destino.Linha == origem.Linha + 2))
            {
                VulneravelEnPassant = p;
            }
        }

        private Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);

            if (pecaCapturada != null)
            {
                _capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(origemT);
                T.IncrementarQteMovimentos();
                Tab.ColocarPeca(T, destinoT);
            }

            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturada == null)
                {
                    Posicao posAdversarioP;

                    if (p.Cor == CorEnum.Branca)
                    {
                        posAdversarioP = new Posicao(destino.Linha + 1, destino.Coluna);
                    }
                    else
                    {
                        posAdversarioP = new Posicao(destino.Linha - 1, destino.Coluna);
                    }

                    pecaCapturada = Tab.RetirarPeca(posAdversarioP);
                    _capturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        private void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturda)
        {
            Peca p = Tab.RetirarPeca(destino);
            p.DecrementarQteMovimentos();

            if (pecaCapturda != null)
            {
                Tab.ColocarPeca(pecaCapturda, destino);
                _capturadas.Remove(pecaCapturda);
            }

            Tab.ColocarPeca(p, origem);


            // #jogadaespecial roque pequeno
            if (p is Rei && destino.Coluna == origem.Coluna + 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna + 3);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna + 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // #jogadaespecial roque grande
            if (p is Rei && destino.Coluna == origem.Coluna - 2)
            {
                Posicao origemT = new Posicao(origem.Linha, origem.Coluna - 4);
                Posicao destinoT = new Posicao(origem.Linha, origem.Coluna - 1);
                Peca T = Tab.RetirarPeca(destinoT);
                T.DecrementarQteMovimentos();
                Tab.ColocarPeca(T, origemT);
            }

            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.Coluna != destino.Coluna && pecaCapturda == VulneravelEnPassant)
                {
                    Peca peao = Tab.RetirarPeca(destino);
                    Posicao posP;
                    if (p.Cor == CorEnum.Branca)
                    {
                        posP = new Posicao(3, destino.Coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.Coluna);
                    }
                    Tab.ColocarPeca(peao, posP);
                }
            }
        }

        public HashSet<Peca> PecasCapturadas(CorEnum cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in _capturadas)
            {
                if (p.Cor == cor)
                {
                    aux.Add(p);
                }
            }

            return aux;
        }

        private bool EstaEmXeque(CorEnum cor)
        {
            Peca R = Rei(cor);
            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor" + cor + " no tabuleiro!");
            }

            foreach (Peca p in PecasEmJogo(Adversario(cor)))
            {
                bool[,] mat = p.MovimentosPossiveis();
                if (mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }

            return false;
        }

        private bool EstaEmXequeMate(CorEnum cor)
        {
            if (!EstaEmXeque(cor))
            {
                return false;
            }

            foreach (Peca p in PecasEmJogo(cor))
            {
                bool[,] mat = p.MovimentosPossiveis();
                for (int i = 0; i < Tab.Linhas; i++)
                {
                    for (int j = 0; j < Tab.Colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = p.Posicao;
                            Posicao destino = new Posicao(i, j);

                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool xeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);

                            if (!xeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private HashSet<Peca> PecasEmJogo(CorEnum cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca p in _pecas)
            {
                if (p.Cor == cor)
                {
                    aux.Add(p);
                }
            }

            aux.ExceptWith(PecasCapturadas(cor));

            return aux;
        }

        private Peca Rei(CorEnum cor)
        {
            foreach (Peca p in PecasEmJogo(cor))
            {
                if (p is Rei)
                {
                    return p;
                }
            }
            return null;
        }

        private CorEnum Adversario(CorEnum cor)
        {
            return cor == CorEnum.Branca ? CorEnum.Preta : CorEnum.Branca;
        }

        private void MudaJogador()
        {
            JogadorAtual = JogadorAtual == CorEnum.Branca ? CorEnum.Preta : CorEnum.Branca;
        }

        private void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrezService(coluna, linha).TransformaPosicaoDeMatriz());
            _pecas.Add(peca);
        }


        private void ColocarPecas()
        {
            ColocarNovaPeca('a', 1, new Torre(Tab, CorEnum.Branca));
            ColocarNovaPeca('b', 1, new Cavalo(Tab, CorEnum.Branca));
            ColocarNovaPeca('c', 1, new Bispo(Tab, CorEnum.Branca));
            ColocarNovaPeca('d', 1, new Dama(Tab, CorEnum.Branca));
            ColocarNovaPeca('e', 1, new Rei(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('f', 1, new Bispo(Tab, CorEnum.Branca));
            ColocarNovaPeca('g', 1, new Cavalo(Tab, CorEnum.Branca));
            ColocarNovaPeca('h', 1, new Torre(Tab, CorEnum.Branca));
            ColocarNovaPeca('a', 2, new Peao(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('b', 2, new Peao(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('c', 2, new Peao(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('d', 2, new Peao(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('e', 2, new Peao(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('f', 2, new Peao(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('g', 2, new Peao(Tab, CorEnum.Branca, this));
            ColocarNovaPeca('h', 2, new Peao(Tab, CorEnum.Branca, this));


            ColocarNovaPeca('a', 8, new Torre(Tab, CorEnum.Preta));
            ColocarNovaPeca('b', 8, new Cavalo(Tab, CorEnum.Preta));
            ColocarNovaPeca('c', 8, new Bispo(Tab, CorEnum.Preta));
            ColocarNovaPeca('d', 8, new Dama(Tab, CorEnum.Preta));
            ColocarNovaPeca('e', 8, new Rei(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('f', 8, new Bispo(Tab, CorEnum.Preta));
            ColocarNovaPeca('g', 8, new Cavalo(Tab, CorEnum.Preta));
            ColocarNovaPeca('h', 8, new Torre(Tab, CorEnum.Preta));
            ColocarNovaPeca('a', 7, new Peao(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('b', 7, new Peao(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('c', 7, new Peao(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('d', 7, new Peao(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('e', 7, new Peao(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('f', 7, new Peao(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('g', 7, new Peao(Tab, CorEnum.Preta, this));
            ColocarNovaPeca('h', 7, new Peao(Tab, CorEnum.Preta, this));
        }
    }
}
