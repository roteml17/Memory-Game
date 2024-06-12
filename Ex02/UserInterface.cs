using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class UserInterface
    {
        private int m_BoardHeight;
        private int m_BoardWidth;
        private Logics gameLogic;
        private UserInterfaceVisualization visualization;
        static int[] s_Card1OfHumanPlayer = new int[(int)eGameConfig.CardArreySize];
        static int[] s_Card2OfHumanPlayer = new int[(int)eGameConfig.CardArreySize];
        private const char k_ChooseQuitTheGame = 'N';

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
            bool playerIsStillPlaying = true, currentTurn = true, player2isHuman = false, isNewGame = true;
            string player2Name, player1Name;
            int againstComputerOrHuman = 0;
            Player player2 = null;
            Board board = new Board();

            player1Name = visualization.GetANameFromUser();
            Player player1 = new Player(player1Name, true, currentTurn);
            againstComputerOrHuman = visualization.ChooseBetweenComputerAndHuman();
            if (againstComputerOrHuman == (int)eGameConfig.AnotherPlayerChoice)
            {
                player2Name = visualization.GetANameFromUser();
                player2isHuman = true;
                player2 = new Player(player2Name, player2isHuman, !currentTurn);
            }

            while (true)
            {
                if (isNewGame)
                {
                    currentTurn = true;
                    player1.MyTurn = currentTurn;
                    initializeNewGame(out board, ref player2, player2isHuman, currentTurn,
                                       arrey, againstComputerOrHuman);
                    isNewGame = false;
                }

                while (playerIsStillPlaying && !gameLogic.CheckEndGame(board))
                {
                    Player currentPlayer = player1.MyTurn ? player1 : player2;
                    if (!selectCards(board, arrey, currentPlayer))
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        Console.WriteLine("Quitting the game...");

                        return;
                    }

                    turn(currentPlayer, board, arrey, ref playerIsStillPlaying);
                }

                playerIsStillPlaying = true;
                currentTurn = !currentTurn;
                player1.MyTurn = currentTurn;
                player2.MyTurn = !currentTurn;
                if (gameLogic.CheckEndGame(board))
                {
                    char response = visualization.EndOfGameMessage(player1, player2);

                    if (response == k_ChooseQuitTheGame)
                    {
                        break;
                    }
                    else
                    {
                        Ex02.ConsoleUtils.Screen.Clear();
                        gameLogic.PairsThatExposed = 0;
                        isNewGame = true; 
                    }
                }
            }
        }

        private bool selectCards(Board i_Board, char[] i_Arrey, Player i_Player)
        {
            bool continueGame = true;

            if (i_Player.IsHuman)
            {
                Console.WriteLine(string.Format("{0}, it's your turn!", i_Player.Name));
                i_Player.Card1 = visualization.GetACardPlaceFromUser(i_Board);
                if (i_Player.Card1 == null)
                {
                    continueGame = false;
                }
                else
                {
                    s_Card1OfHumanPlayer = i_Player.Card1;
                    Ex02.ConsoleUtils.Screen.Clear();
                    visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);
                    Console.WriteLine(string.Format("{0}, it's your turn!", i_Player.Name));
                    i_Player.Card2 = visualization.GetACardPlaceFromUser(i_Board);
                    if (i_Player.Card2 == null)
                    {
                        continueGame = false;
                    }
                    else
                    {
                        s_Card2OfHumanPlayer = i_Player.Card2;
                        Ex02.ConsoleUtils.Screen.Clear();
                        visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);
                    }
                }
            }
            else
            {
                i_Player.AddTheCardOfTheHumanToTheRememberArrey(s_Card1OfHumanPlayer, i_Board);
                i_Player.AddTheCardOfTheHumanToTheRememberArrey(s_Card2OfHumanPlayer, i_Board);

                Console.WriteLine(string.Format("{0}, it's your turn!", i_Player.Name));
                i_Player.Card1 = i_Player.ComputerIsPlaying(i_Board);
                i_Board.OpenCardPlace(i_Player.Card1[0], i_Player.Card1[1]);
                Ex02.ConsoleUtils.Screen.Clear();
                visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);

                Console.WriteLine(string.Format("{0}, it's your turn!", i_Player.Name));
                i_Player.Card2 = i_Player.ComputerIsPlaying(i_Board);
                i_Board.OpenCardPlace(i_Player.Card2[0], i_Player.Card2[1]);
                Ex02.ConsoleUtils.Screen.Clear();
                visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);
            }

            return continueGame;
        }

        private void initializeNewGame(out Board o_Board, ref Player io_Player2, bool i_Player2isHuman, 
                                       bool i_CurrentTurn, char[] i_Arrey, int i_AgainstComputerOrHuman)
        {
            visualization.GetBoardBoundaries(ref m_BoardHeight, ref m_BoardWidth);
            if (i_AgainstComputerOrHuman == (int)eGameConfig.ComputerChoice)
            {
                io_Player2 = new Player(i_Player2isHuman, !i_CurrentTurn, (m_BoardHeight * m_BoardWidth) / 2);
            }

            o_Board = new Board(m_BoardHeight, m_BoardWidth);
            o_Board.InitializtingBoard();
            visualization.PrintBoard(o_Board, i_Arrey, m_BoardHeight, m_BoardWidth);
        }

        private void turn(Player i_CurrentPlayer, Board i_Board, 
                            char[] i_Arrey, ref bool io_PlayerIsStillPlaying)
        {
            if (gameLogic.CheckIfMatchesCardsAndTurningThem(i_CurrentPlayer.Card1, i_CurrentPlayer.Card2, i_Board))
            {
                i_CurrentPlayer.Score++;
            }
            else
            {
                Ex02.ConsoleUtils.Screen.Clear();
                visualization.PrintBoard(i_Board, i_Arrey, m_BoardHeight, m_BoardWidth);
                io_PlayerIsStillPlaying = false;
            }
        }   
    }
}