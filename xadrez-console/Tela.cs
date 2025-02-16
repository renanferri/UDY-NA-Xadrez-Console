using tabuleiro;

namespace xadrez_console
{
    class Tela
    {
        public static void ImprimirTabuleiro(Tabuleiro tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                for (int j = 0; j < tab.Colunas; j++)
                {
                    Peca peca = tab.Peca(i, j);
                    if (peca != null)
                    {
                        Console.Write(tab.Peca(i, j));
                    }
                    else
                    {
                        Console.Write("-  ");
                    }

                }
                Console.WriteLine();
            }
        }
    }
}
