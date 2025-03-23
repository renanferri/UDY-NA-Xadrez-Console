using xadrez_console.Application.Services;
using xadrez_console.Domain.Entities;
using xadrez_console.Enums;

namespace xadrez_console.Presentation.ConsoleUI
{
    class Tela
    {
        public static void ImprimirPartida(PartidaDeXadrezService partida)
        {
            ImprimirTabuleiro(partida.Tab);
            Console.WriteLine();
            ImprimirPecasCapturadas(partida);
            Console.WriteLine();
            Console.WriteLine("Turno:" + partida.Turno);

            if (!partida.Terminada)
            {
                Console.WriteLine("Aguardando jogada:" + partida.JogadorAtual);
            
                if (partida.Xeque)
                {
                    Console.WriteLine("XEQUE!");
                }
            }
            else
            {
                Console.WriteLine("XEQUEMATE!");
                Console.WriteLine("Vencedor:" + partida.JogadorAtual);
            }


        }

        public static void ImprimirTabuleiro(TabuleiroService tab)
        {
            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    ImprimirPeca(tab.Peca(i, j));
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
        }        

        public static void ImprimirTabuleiro(TabuleiroService tab, bool[,] posicoesPossiveis)
        {
            ConsoleColor fundoOriginal = Console.BackgroundColor;
            ConsoleColor fundoAlterado = ConsoleColor.DarkGray;

            for (int i = 0; i < tab.Linhas; i++)
            {
                Console.Write(8 - i + " ");
                for (int j = 0; j < tab.Colunas; j++)
                {
                    Console.BackgroundColor = fundoOriginal;
                    if (posicoesPossiveis[i, j])
                    {
                        Console.BackgroundColor = fundoAlterado;
                    }
                    
                    ImprimirPeca(tab.Peca(i, j));

                    Console.BackgroundColor = fundoOriginal;
                }
                Console.WriteLine();
            }
            Console.WriteLine("  a b c d e f g h");
            Console.BackgroundColor = fundoOriginal;
        }


        public static PosicaoXadrezService LerPosicaoXadrez()
        {
            string s = Console.ReadLine();

            char coluna = s[0];
            int linha = int.Parse(s[1].ToString());
            return new PosicaoXadrezService(coluna, linha);
        }

        private static void ImprimirPeca(Peca peca)
        {
            if (peca == null)
            {
                Console.Write("- ");
            }
            else
            {
                if (peca.Cor == CorEnum.Branca)
                {
                    Console.Write(peca);
                }
                else
                {
                    ConsoleColor aux = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(peca);
                    Console.ForegroundColor = aux;
                }
                Console.Write(" ");
            }
        }

        private static void ImprimirPecasCapturadas(PartidaDeXadrezService partida)
        {
            Console.WriteLine("Peças capturadas");
            
            Console.Write("Brancas: ");
            ImprimirConjunto(partida.PecasCapturadas(CorEnum.Branca));
            Console.WriteLine();
            
            Console.Write("Pretas: ");
            ConsoleColor aux = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            ImprimirConjunto(partida.PecasCapturadas(CorEnum.Preta));
            Console.ForegroundColor = aux;
            
            Console.WriteLine();
        }

        private static void ImprimirConjunto(HashSet<Peca> conjunto)
        {
            Console.Write("[");
            foreach (Peca p in conjunto)
            {
                Console.Write(p + " ");
            }
            Console.Write("]");
        }
    }
}
