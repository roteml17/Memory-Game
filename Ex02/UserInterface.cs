﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
            string player2Name = GetANameFromUser();
            bool playerIsStillPlaying = true, currentTurn = true;

            Player player1 = new Player(player1Name, true, currentTurn);
            Player player2 = new Player(player2Name, true, !currentTurn);
            GetBoardBoundaries(); //את הבדיקה של הזוגי להעביר ללוגיקה

            Board board = new Board(m_BoardHeight, m_BoardWidth);
            board.InitializtingBoard();

            PrintBoard(board, arrey);

            while (true) //continue if not press Q or want to play another game
            {
                while (playerIsStillPlaying && !board.CheckEndGame())
                {
                    Player currentPlayer = player1.MyTurn ? player1 : player2;
                    SelectCards(board, arrey, currentPlayer);

                    if (board.CheckIfMatchesCardsAndTurningThem(currentPlayer.Card1, currentPlayer.Card2))
                    {
                        currentPlayer.Score++;
                    }
                    else
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        PrintBoard(board, arrey);
                        playerIsStillPlaying = false;
                    }
                }

                playerIsStillPlaying = true;
                currentTurn = !currentTurn;

                player1.MyTurn = currentTurn;
                player2.MyTurn = !currentTurn;

                if (board.CheckEndGame()) ///////
                {
                    Console.WriteLine("{0}: {1}", player1.Name, player1.Score);
                    Console.WriteLine("{0}: {1}", player2.Name, player2.Score);
                    break;
                }
            }
        }

        public int[] GetACardPlaceFromUser(Board i_Board)
        {
            string cardPlace;
            int[] returnedCardPlace = new int[2];
            bool isValidLocation, isExposed = false;

            Console.Write("Please enter a card place: ");
            cardPlace = Console.ReadLine();

            int column = cardPlace[0] - 'A';
            int row = cardPlace[1] - '0' - 1;

            isValidLocation = i_Board.IsValidCardPlace(column, row);
            if (isValidLocation)
            {
                isExposed = i_Board.IsAlreadyExposed(column, row);
            }

            while (!isValidLocation || isExposed)
            {
                if (!isValidLocation)
                {
                    Console.Write("Invalid place, Please enter again: ");
                }
                else if (isExposed)
                {
                    Console.Write("The card you chose is already exposed, Please enter again: ");
                }

                cardPlace = Console.ReadLine();
                column = cardPlace[0] - 'A';
                row = cardPlace[1] - '0' - 1;

                isValidLocation = i_Board.IsValidCardPlace(column, row);
                if (isValidLocation)
                {
                    isExposed = i_Board.IsAlreadyExposed(column, row);
                }
            }
            returnedCardPlace[0] = column;
            returnedCardPlace[1] = row;

            return returnedCardPlace;
        }

        public void SelectCards(Board i_Board, char[] i_Arrey, Player i_Player)
        {
            Console.WriteLine("{0}, it's your turn!", i_Player.Name);
            i_Player.Card1 = GetACardPlaceFromUser(i_Board);
            Ex02.ConsoleUtils.Screen.Clear();
            PrintBoard(i_Board, i_Arrey);

            Console.WriteLine("{0}, it's your turn!", i_Player.Name);
            i_Player.Card2 = GetACardPlaceFromUser(i_Board);
            Ex02.ConsoleUtils.Screen.Clear();
            PrintBoard(i_Board, i_Arrey);
        }
    }
}
