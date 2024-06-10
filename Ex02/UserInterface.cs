﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
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
                Console.Write("Height is out of boundaries, please enter again: ");
                m_BoardHeight = int.Parse(Console.ReadLine());
            }

            Console.Write("Please enter the board's width: ");
            m_BoardWidth = int.Parse(Console.ReadLine());

            while (!ValidationCheckForBoardWidth() || !Board.CheckIfMultiplicationIsEven(m_BoardHeight, m_BoardWidth))
            {
                Console.Write("Width is out of boundaries, please enter again: ");
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
            bool playerIsStillPlaying = true, currentTurn = true, player2isHuman = false;
            string player2Name, player1Name;
            int againstComputerOrHuman = 0;
            Player player2 = null;
            Board board = new Board();
            bool isNewGame = true;

            player1Name = GetANameFromUser();
            Player player1 = new Player(player1Name, true, currentTurn);

            againstComputerOrHuman = ChooseBetweenComputerAndHuman();

            if (againstComputerOrHuman == 2)
            {
                player2Name = GetANameFromUser();
                player2isHuman = true;
                player2 = new Player(player2Name, player2isHuman, !currentTurn);
            }

            while (true) //continue if not press Q or want to play another game
            {
                if (isNewGame)
                {
                    GetBoardBoundaries();

                    if (againstComputerOrHuman == 1)
                    {
                        player2 = new Player(player2isHuman, !currentTurn, (m_BoardHeight * m_BoardWidth) / 2);
                    }

                    board = new Board(m_BoardHeight, m_BoardWidth);
                    board.InitializtingBoard();

                    PrintBoard(board, arrey);
                    isNewGame = false;
                }

                while (playerIsStillPlaying && !board.CheckEndGame())
                {
                    Player currentPlayer = player1.MyTurn ? player1 : player2;
                    if(!SelectCards(board, arrey, currentPlayer))
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        Console.WriteLine("Quitting the game...");
                        return;
                    }

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

                    Console.Write("Do you want to play another game? Y/N: ");
                    char response = char.Parse(Console.ReadLine().ToUpper());
                    if (response == 'N')
                    {
                        break;
                    }
                    else
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        isNewGame = true; 
                    }
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

            if (cardPlace == "Q")
            {
                returnedCardPlace = null;
            }
            else
            {
                int column = cardPlace[0] - 'A';
                int row = cardPlace[1] - '0' - 1;

                isValidLocation = i_Board.IsValidCardPlace(row, column);
                if (isValidLocation)
                {
                    isExposed = i_Board.IsAlreadyExposed(row, column);
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

                    isValidLocation = i_Board.IsValidCardPlace(row, column);
                    if (isValidLocation)
                    {
                        isExposed = i_Board.IsAlreadyExposed(row, column);
                    }
                }
                returnedCardPlace[0] = row;
                returnedCardPlace[1] = column;
            }
            return returnedCardPlace;
        }

        public bool SelectCards(Board i_Board, char[] i_Arrey, Player i_Player)
        {
            bool continueGame = true;

            if (i_Player.IsHuman)
            {
                Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                i_Player.Card1 = GetACardPlaceFromUser(i_Board);
                if (i_Player.Card1 == null)
                {
                    continueGame = false;
                }
                else
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    PrintBoard(i_Board, i_Arrey);

                    Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                    i_Player.Card2 = GetACardPlaceFromUser(i_Board);
                    if (i_Player.Card2 == null)
                    {
                        continueGame = false;
                    }
                    else
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        PrintBoard(i_Board, i_Arrey);
                    }
                }
            }
            else
            {
                Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                i_Player.Card1 = i_Player.ComputerIsPlaying(i_Board);
                i_Board.OpenCardPlace(i_Player.Card1[0], i_Player.Card1[1]);
                Ex02.ConsoleUtils.Screen.Clear();
                PrintBoard(i_Board, i_Arrey);

                Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                i_Player.Card2 = i_Player.ComputerIsPlaying(i_Board);
                i_Board.OpenCardPlace(i_Player.Card2[0], i_Player.Card2[1]);
                Ex02.ConsoleUtils.Screen.Clear();
                PrintBoard(i_Board, i_Arrey);

                System.Threading.Thread.Sleep(2000);
            }
            return continueGame;
        }

        public int ChooseBetweenComputerAndHuman()
        {
            int theUserChose;

            Console.WriteLine("Would you like to play against another player or against the computer?");
            Console.WriteLine("(1) Computer");
            Console.WriteLine("(2) Another player");

            theUserChose = int.Parse(Console.ReadLine());

            while (!ValidationCheckForComputerVsHuman(theUserChose))
            {
                Console.Write("Unvalid choose! Please choose between 1 or 2: ");
                theUserChose = int.Parse(Console.ReadLine());
            }

            return theUserChose;
        }

        public bool ValidationCheckForComputerVsHuman(int i_TheUserChose)
        {
            bool validChoose = true;

            if (i_TheUserChose != 1 && i_TheUserChose != 2)
            {
                validChoose = false;
            }

            return validChoose;
        }

        public void GetBoardBoundariesAndInitializtingBoard()
        {

        }
    }
}