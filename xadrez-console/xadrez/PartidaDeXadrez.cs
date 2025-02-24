using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        public bool Xeque { get; private set; }

        private HashSet<Peca> _pecas;

        private HashSet<Peca> _capturadas;


        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Xeque = false;
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
            if (!Tab.Peca(origem).PodeMoverPara(destino))
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

            Xeque = false;
            if (EstaEmXeque(Adversario(JogadorAtual)))
            {
                Xeque = true;
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
        }

        public HashSet<Peca> PecasCapturadas(Cor cor)
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

        private bool EstaEmXeque(Cor cor)
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

        private bool EstaEmXequeMate(Cor cor)
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

        private HashSet<Peca> PecasEmJogo(Cor cor)
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

        private Peca Rei(Cor cor)
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

        private Cor Adversario(Cor cor)
        {
            return (cor == Cor.Branca) ? Cor.Preto : Cor.Branca;
        }

        private void MudaJogador()
        {
            JogadorAtual = (JogadorAtual == Cor.Branca) ? Cor.Preto : Cor.Branca;
        }

        private void ColocarNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocarPeca(peca, new PosicaoXadrez(coluna, linha).TransformaPosicaoDeMatriz());
            _pecas.Add(peca);
        }


        private void ColocarPecas()
        {
            ColocarNovaPeca('c', 1, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('c', 5, new Torre(Tab, Cor.Branca));
            ColocarNovaPeca('a', 1, new Rei(Tab, Cor.Branca));
            ColocarNovaPeca('d', 1, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('d', 5, new Torre(Tab, Cor.Preto));
            ColocarNovaPeca('a', 7, new Rei(Tab, Cor.Preto));



        }
    }
}
