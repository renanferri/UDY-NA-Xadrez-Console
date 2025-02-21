using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        private int Turno;
        private Cor JogadorAtual;
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            ColocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetirarPeca(origem);
            p.IncrementarQteMovimentos();
            Peca pecaCapturada = Tab.RetirarPeca(destino);
            Tab.ColocarPeca(p, destino);
        }

        private void ColocarPecas()
        {
            Tab.ColocarPeca(new Torre(Tab, Cor.Branca), new PosicaoXadrez('c', 1).TransformaPosicaoDeMatriz());

            Tab.ColocarPeca(new Torre(Tab, Cor.Branca), new PosicaoXadrez('c', 5).TransformaPosicaoDeMatriz());

            Tab.ColocarPeca(new Torre(Tab, Cor.Branca), new PosicaoXadrez('d', 1).TransformaPosicaoDeMatriz());

            Tab.ColocarPeca(new Torre(Tab, Cor.Preto), new PosicaoXadrez('d', 5).TransformaPosicaoDeMatriz());
        }
    }
}
