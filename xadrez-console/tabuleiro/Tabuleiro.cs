namespace tabuleiro
{
    class Tabuleiro
    {
        public int Linhas { get; set; }
        public int Colunas { get; set; }
        
        private Peca[,] _pecas;

        public Tabuleiro(int linhas, int colunas)
        {
            Linhas = linhas;
            Colunas = colunas;  
            _pecas = new Peca[linhas, colunas];
        }

        public Peca Peca(int linha, int coluba)
        {
            return _pecas[linha, coluba];
        }

        public Peca Peca(Posicao pos)
        {
            return _pecas[pos.Linha, pos.Coluna];
        }

        public void ColocarPeca(Peca p, Posicao pos)
        {
            if (ExistePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição!");
            }
            _pecas[pos.Linha, pos.Coluna] = p;
            p.Posicao = pos;
        }

        public Peca RetirarPeca(Posicao pos)
        {
            if (Peca(pos) == null)
            {
                return null;
            }
            Peca aux = Peca(pos);
            aux.Posicao = null;
            _pecas[pos.Linha, pos.Coluna] = null;
            return aux;
        }

        private bool ExistePeca(Posicao pos)
        {
            ValidarPosicao(pos);
            return Peca(pos) != null;
        }

        private void ValidarPosicao(Posicao pos)
        {
            if (!PosicaoValida(pos))
            {
                throw new TabuleiroException("Posição Inválida!");
            }
        }

        public bool PosicaoValida(Posicao pos)
        {
            if(pos.Linha < 0 || pos.Linha>=Linhas || pos.Coluna < 0 || pos.Coluna >= Colunas)
            {
                return false;
            }
            return true;
        }


    }
}
