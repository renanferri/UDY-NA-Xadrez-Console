namespace tabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        
        private Peca[,] Pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;  
            Pecas = new Peca[linhas, colunas];
        }

        public Peca Peca(int linha, int coluba)
        {
            return Pecas[linha, coluba];
        }

        public void ColocarPeca(Peca p, Posicao pos)
        {
            Pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }
    }
}
