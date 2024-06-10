using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex02
{
    internal struct Board
    {
        internal struct cell
        {
            private int m_Value;
            private bool m_IsExposed;

            public int Value
            {
                get
                {
                    return m_Value;
                }
                set
                {
                    m_Value = value;
                }
            }
            public bool IsExposed
            {
                get
                {
                    return m_IsExposed;
                }
                set
                {
                    m_IsExposed = value;
                }
            }
        }

        private cell[,] m_Board;
        private int m_BoardHeight;
        private int m_BoardWidth;
        private int m_NumberOfPairsThatExposed;

        public cell[,] GameBoard
        {
            get
            {
                return m_Board;
            }
        }

        public Board(int i_Height, int i_Width)
        {
            m_BoardHeight = i_Height;
            m_BoardWidth = i_Width;
            m_Board = new cell[i_Height, i_Width];
            m_NumberOfPairsThatExposed = 0;
        }

        public int Height
        {
            get
            {
                return m_BoardHeight;
            }
        }

        public int Width
        {
            get
            {
                return m_BoardWidth;
            }
        }

        public static bool CheckIfMultiplicationIsEven(int i_BoardHeight, int i_BoardWidth)
        {
            bool evenMultiplication = true;

            if ((i_BoardHeight * i_BoardWidth) % 2 != 0)
            {
                evenMultiplication = false;
            }

            return evenMultiplication;
        }

        public void InitializtingBoard()
        {
            Random random = new Random();
            int countOfNumbersOfValues = (m_BoardHeight * m_BoardWidth) / 2;
            int[] valuesOccurrences = new int[countOfNumbersOfValues];

            for (int i = 0; i < m_BoardHeight; i++)
            {
                for (int j = 0; j < m_BoardWidth; j++)
                {
                    m_Board[i, j].IsExposed = false;
                    int value = ChooseTheValueToPutIn(valuesOccurrences, random);
                    m_Board[i, j].Value = value;
                }
            }
        }

        public int ChooseTheValueToPutIn(int[] i_ValuesOccurrences, Random random)
        {
            int value = random.Next(i_ValuesOccurrences.Length);

            while (i_ValuesOccurrences[value] == 2)
            {
                value = random.Next(i_ValuesOccurrences.Length);
            }

            i_ValuesOccurrences[value]++;

            return value;
        }

        public bool IsValidCardPlace(int i_Row, int i_Column)
        {
            bool returnedVal = true;

            if (i_Column >= m_BoardWidth || i_Column < 0)
            {
                returnedVal = false;
            }
            else if (i_Row >= m_BoardHeight || i_Row < 0)
            {
                returnedVal = false;
            }
            return returnedVal;
        }

        public bool IsAlreadyExposed(int i_Row, int i_Column)
        {
            bool isExposedAlready = false;

            if (m_Board[i_Row, i_Column].IsExposed)
            {
                isExposedAlready = true;
            }
            else
            {
                OpenCardPlace(i_Row, i_Column);
            }

            return isExposedAlready;
        }

        public void OpenCardPlace(int i_Row, int i_Column)
        {
            m_Board[i_Row, i_Column].IsExposed = true;
        }

        public bool CheckIfMatchesCardsAndTurningThem(int[] i_Card1, int[] i_Card2)
        {
            int RowsCard1 = i_Card1[0];
            int ColumnCard1 = i_Card1[1];
            int RowsCard2 = i_Card2[0];
            int ColumnCard2 = i_Card2[1];
            bool cardAreEqual = true;

            if (m_Board[RowsCard1, ColumnCard1].Value != m_Board[RowsCard2, ColumnCard2].Value)
            {
                m_Board[RowsCard1, ColumnCard1].IsExposed = false;
                m_Board[RowsCard2, ColumnCard2].IsExposed = false;
                cardAreEqual = false;
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                m_NumberOfPairsThatExposed++;
            }

            return cardAreEqual;
        }

        public bool CheckEndGame()
        {
            bool endGame = false;

            if (m_NumberOfPairsThatExposed == (m_BoardHeight * m_BoardWidth) / 2)
            {
                endGame = true;
            }

            return endGame;
        }
    }
}