using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class UserInterfaceVisualization
    {
        private const int k_MinimumBoardBoundaries = 4;
        private const int k_MaximumBoardBoundaries = 6;
        private const string k_ExitGame = "Q";

        public string GetANameFromUser()
        {
            string playerName;

            Console.Write("Please enter your Name: ");
            playerName = Console.ReadLine();

            return playerName;
        }

        public void GetBoardBoundaries(ref int i_BoardHeight,ref int i_BoardWidth)
        {
            Console.Write("Please enter the board's height: ");
            i_BoardHeight = int.Parse(Console.ReadLine());

            while (!ValidationCheckForBoardHeight(i_BoardHeight, i_BoardWidth))
            {
                Console.Write("Height is out of boundaries, please enter again: ");
                i_BoardHeight = int.Parse(Console.ReadLine());
            }

            Console.Write("Please enter the board's width: ");
            i_BoardWidth = int.Parse(Console.ReadLine());

            while (!ValidationCheckForBoardWidth(i_BoardHeight, i_BoardWidth) || 
                !Board.CheckIfMultiplicationIsEven(i_BoardHeight, i_BoardWidth))
            {
                Console.Write("Width is out of boundaries, please enter again: ");
                i_BoardWidth = int.Parse(Console.ReadLine());
            }
        }

        public bool ValidationCheckForBoardHeight(int i_BoardHeight, int i_BoardWidth)
        {
            bool validHeight = true;

            if (i_BoardHeight < k_MinimumBoardBoundaries ||
                i_BoardHeight > k_MaximumBoardBoundaries)
            {
                validHeight = false;
            }

            return validHeight;
        }

        public bool ValidationCheckForBoardWidth(int i_BoardHeight, int i_BoardWidth)
        {
            bool validWidth = true;

            if (i_BoardWidth < k_MinimumBoardBoundaries ||
                i_BoardWidth > k_MaximumBoardBoundaries)
            {
                validWidth = false;
            }

            return validWidth;
        }

        public void PrintBoard(Board i_Board, char[] i_Arrey, int i_BoardHeight, int i_BoardWidth)
        {
            string separator = "  " + new string('=', i_BoardWidth * 6 + 1);

            for (int i = 0; i < i_BoardWidth; i++)
            {
                Console.Write("     {0}", (char)('A' + i));
            }

            Console.WriteLine();
            Console.WriteLine(separator);
            for (int i = 0; i < i_BoardHeight; i++)
            {
                Console.Write("{0} |", i + 1);
                for (int j = 0; j < i_BoardWidth; j++)
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

        public int[] GetACardPlaceFromUser(Board i_Board)
        {
            string cardPlace;
            int[] returnedCardPlace = new int[(int)eGameConfig.CardArreySize];
            bool isValidLocation = true, isExposed = false;
            int row, column;

            Console.Write("Please enter a card place: ");
            cardPlace = Console.ReadLine();
            CheckIfTheUserPutOnlyRowOrOnlyColumn(ref cardPlace);

            if (cardPlace == k_ExitGame)
            {
                returnedCardPlace = null;
            }
            else
            {
                ConvertToCoordinateInTheBoard(out column, out row, cardPlace);
                CheckValitationOfTheChosenCard(ref isValidLocation, ref isExposed, 
                                                 i_Board, row, column);
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
                    CheckIfTheUserPutOnlyRowOrOnlyColumn(ref cardPlace);
                    ConvertToCoordinateInTheBoard(out column, out row, cardPlace);
                    CheckValitationOfTheChosenCard(ref isValidLocation, ref isExposed, 
                                                     i_Board, row, column);
                }

                returnedCardPlace[0] = row;
                returnedCardPlace[1] = column;
            }

            return returnedCardPlace;
        }

        public void ConvertToCoordinateInTheBoard(out int o_Column,out int o_Row, string cardPlace)
        {
            o_Column = cardPlace[0] - 'A';
            o_Row = cardPlace[1] - '0' - 1;
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

        public void CheckIfTheUserPutOnlyRowOrOnlyColumn(ref string io_CardPlace)
        {
            while (io_CardPlace.Length != (int)eGameConfig.CardArreySize && io_CardPlace != k_ExitGame) 
            {
                Console.Write("Invalid selection of row and column, please enter again: ");
                io_CardPlace = Console.ReadLine();
            }
        }

        public void CheckValitationOfTheChosenCard(ref bool io_IsValidLocation, 
                                                    ref bool io_IsExposed, Board i_Board, int row, int column)
        {
            io_IsValidLocation = i_Board.IsValidCardPlace(row, column);
            if (io_IsValidLocation)
            {
                io_IsExposed = i_Board.IsAlreadyExposed(row, column);
            }
        }

        public bool ValidationCheckForComputerVsHuman(int i_TheUserChose)
        {
            bool validChoose = true;

            if (i_TheUserChose != (int)eGameConfig.ComputerChoice && i_TheUserChose != (int)eGameConfig.AnotherPlayerChoice)
            {
                validChoose = false;
            }

            return validChoose;
        }

        public char EndOfGameMessage(Player i_Player1, Player i_Player2)
        {
            Console.WriteLine("{0}: {1}", i_Player1.Name, i_Player1.Score);
            Console.WriteLine("{0}: {1}", i_Player2.Name, i_Player2.Score);
            Console.Write("Do you want to play another game? Y/N: ");
            char response = char.Parse(Console.ReadLine().ToUpper());

            return response;
        }

    }
}
