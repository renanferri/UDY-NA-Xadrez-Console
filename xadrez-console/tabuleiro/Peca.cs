namespace tabuleiro
{
    abstract class Peca
    {
        public Tabuleiro Tab { get; protected set; }
        public Cor Cor { get; protected set; }
        public Posicao Posicao { get; set; }
        public int QteMovimentos { get; protected set; }
        

        public Peca(Tabuleiro tab, Cor cor)
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

        public abstract bool[,] MovimentosPossiveis();
    }
}
