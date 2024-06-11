using System;
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
        private Logics gameLogic;
        private UserInterfaceVisualization visualization;

        static int[] card1OfHumanPlayer = new int[2];
        static int[] card2OfHumanPlayer = new int[2];
        public UserInterface()
        {
            m_BoardHeight = 0;
            m_BoardWidth = 0;
            gameLogic = new Logics();
            visualization = new UserInterfaceVisualization();
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

            player1Name = visualization.GetANameFromUser();
            Player player1 = new Player(player1Name, true, currentTurn);

            againstComputerOrHuman = visualization.ChooseBetweenComputerAndHuman();

            if (againstComputerOrHuman == 2)
            {
                player2Name = visualization.GetANameFromUser();
                player2isHuman = true;
                player2 = new Player(player2Name, player2isHuman, !currentTurn);
            }

            while (true) //continue if not press Q or want to play another game
            {
                if (isNewGame)
                {
                    visualization.GetBoardBoundaries(ref m_BoardHeight,ref m_BoardWidth);

                    if (againstComputerOrHuman == 1)
                    {
                        player2 = new Player(player2isHuman, !currentTurn, (m_BoardHeight * m_BoardWidth) / 2);
                    }

                    board = new Board(m_BoardHeight, m_BoardWidth);
                    board.InitializtingBoard();

                    visualization.PrintBoard(board, arrey, m_BoardHeight, m_BoardWidth);
                    isNewGame = false;
                }

                while (playerIsStillPlaying && !gameLogic.CheckEndGame(board))
                {
                    Player currentPlayer = player1.MyTurn ? player1 : player2;
                    if(!SelectCards(board, arrey, currentPlayer))
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        Console.WriteLine("Quitting the game...");
                        return;
                    }

                    if (gameLogic.CheckIfMatchesCardsAndTurningThem(currentPlayer.Card1, currentPlayer.Card2, board))
                    {
                        currentPlayer.Score++;
                    }
                    else
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        visualization.PrintBoard(board, arrey, m_BoardHeight, m_BoardWidth);
                        playerIsStillPlaying = false;
                    }
                }

                playerIsStillPlaying = true;
                currentTurn = !currentTurn;

                player1.MyTurn = currentTurn;
                player2.MyTurn = !currentTurn;

                if (gameLogic.CheckEndGame(board)) ///////
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

        public bool SelectCards(Board i_Board, char[] i_Arrey, Player i_Player)
        {
            bool continueGame = true;
           
            if (i_Player.IsHuman)
            {
                Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                i_Player.Card1 = visualization.GetACardPlaceFromUser(i_Board);
                if (i_Player.Card1 == null)
                {
                    continueGame = false;
                }
                else
                {
                    card1OfHumanPlayer = i_Player.Card1;
                    Ex02.ConsoleUtils.Screen.Clear();
                    visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);

                    Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                    i_Player.Card2 = visualization.GetACardPlaceFromUser(i_Board);
                    if (i_Player.Card2 == null)
                    {
                        continueGame = false;
                    }
                    else
                    {
                        card2OfHumanPlayer = i_Player.Card2;
                        Ex02.ConsoleUtils.Screen.Clear();
                        visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);
                    }
                }
            }
            else
            {
                i_Player.AddTheCardOfTheHumanToTheRememberArrey(card1OfHumanPlayer, i_Board);
                i_Player.AddTheCardOfTheHumanToTheRememberArrey(card2OfHumanPlayer, i_Board);

                Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                i_Player.Card1 = i_Player.ComputerIsPlaying(i_Board);
                i_Board.OpenCardPlace(i_Player.Card1[0], i_Player.Card1[1]);
                Ex02.ConsoleUtils.Screen.Clear();
                visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);

                Console.WriteLine("{0}, it's your turn!", i_Player.Name);
                i_Player.Card2 = i_Player.ComputerIsPlaying(i_Board);
                i_Board.OpenCardPlace(i_Player.Card2[0], i_Player.Card2[1]);
                Ex02.ConsoleUtils.Screen.Clear();
                visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);

                System.Threading.Thread.Sleep(2000);
            }
            return continueGame;
        }
    }
}