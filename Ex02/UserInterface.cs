using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class UserInterface
    {
        private readonly string mr_UserName;
        private int m_BoardHeight;
        private int m_BoardWidth;

        private const int k_MinimumBoardBoundaries = 4;
        private const int k_MaximumBoardBoundaries = 6;
        public UserInterface()
        {
            m_BoardHeight = 0;
            m_BoardWidth = 0;
        }

        public string GetANameFromUser()
        {
            string playerName;
            Console.Write("Please enter your Name: ");
            playerName = Console.ReadLine();

            return playerName;
        }
        public void GetBoardBoundaries()
        {
            Console.Write("Please enter the board's height: ");
            m_BoardHeight = int.Parse(Console.ReadLine());

            while (!ValidationCheckForBoardHeight())
            {
                Console.Write("Unvalid Height, please enter again: ");
                m_BoardHeight = int.Parse(Console.ReadLine());
            }

            Console.Write("Please enter the board's width: ");
            m_BoardWidth = int.Parse(Console.ReadLine());

            while (!ValidationCheckForBoardWidth() || !CheckIfMultiplicationIsEven())
            {
                Console.Write("Unvalid Width, please enter again: ");
                m_BoardWidth = int.Parse(Console.ReadLine());
            }
        }

        public bool ValidationCheckForBoardHeight()
        {
            bool validHeight = true;

            if (m_BoardHeight < k_MinimumBoardBoundaries || 
                m_BoardHeight > k_MaximumBoardBoundaries)
            {
                validHeight = false;
            }

            return validHeight;
        }

        public bool ValidationCheckForBoardWidth()
        {
            bool validWidth = true;

            if (m_BoardWidth < k_MinimumBoardBoundaries ||
                m_BoardWidth > k_MaximumBoardBoundaries)
            {
                validWidth = false;
            }

            return validWidth;
        }

        //move to logic
        public bool CheckIfMultiplicationIsEven()
        {
            bool evenMultiplication = true;

            if ((m_BoardHeight * m_BoardWidth) % 2 != 0)
            {
                evenMultiplication = false;
            }

            return evenMultiplication;
        }

        public void PrintBoard(Board i_Board, char[] i_Arrey)
        {
            // Print the column headers
            for (int i = 0; i < m_BoardWidth; i++)
            {
                Console.Write("     {0}", (char)('A' + i));
            }
            Console.WriteLine();

            // Generate the separator string
            string separator = "  " + new string('=', m_BoardWidth * 6 + 1);
            Console.WriteLine(separator);

            // Print each row
            for (int i = 0; i < m_BoardHeight; i++)
            {
                Console.Write("{0} |", i + 1);
                for (int j = 0; j < m_BoardWidth; j++)
                {
                    if (i_Board.GameBoard[i, j].IsExposed)
                    {
                        Console.Write("  {0}  |", i_Arrey[i_Board.GameBoard[i, j].Value]);
                    }
                    else
                    {
                        Console.Write("     |");
                    }
                }
                Console.WriteLine();
                Console.WriteLine(separator);
            }
        }


        public void RunGame()
        {
            char[] arrey = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' };


            string player1Name = GetANameFromUser();
            //string player2Name = GetANameFromUser();

            Player player1 = new Player(player1Name, true);
            //Player player2 = new Player(player2Name, true);
            GetBoardBoundaries();

            Board board = new Board(m_BoardHeight, m_BoardWidth);
            board.InitializtingBoard();

            PrintBoard(board, arrey);

            while(true)
            {
                GetACardPlaceFromUser(board);
                Ex02.ConsoleUtils.Screen.Clear();
                PrintBoard(board, arrey);
                GetACardPlaceFromUser(board);
                Ex02.ConsoleUtils.Screen.Clear();
                PrintBoard(board, arrey);
                //board.CheckIfMatchesCardsAndTurningThem();
            }
        }

        public void GetACardPlaceFromUser(Board i_Board)
        {
            string cardPlace;

            Console.Write("Please enter a card place: ");
            cardPlace = Console.ReadLine();

            int column = cardPlace[0] - 'A';
            int row = cardPlace[1] - '0';

            while (!i_Board.IsValidCardPlace(column, row))
            {
                Console.Write("Invalid place, Please enter again: ");
                cardPlace = Console.ReadLine();
            }
        }
    }
}
