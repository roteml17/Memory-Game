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

        public bool IsValidCardPlace(string i_CardPlace)
        {
            bool returnedVal = true;

            if (i_CardPlace[0] - 'A' > m_BoardWidth || i_CardPlace[0] - 'A' < 0)
            {
                returnedVal = false;
            }
            else if (i_CardPlace[1] - '0' > m_BoardHeight || i_CardPlace[1] - '0' < 0)
            {
                returnedVal = false;
            }
            else
            {
                OpenCardPlace(i_CardPlace);
            }
            return returnedVal;
        }

        //not string
        public void OpenCardPlace(string i_CardPlace)
        {
            int i = i_CardPlace[0] - 'A';
            int j = i_CardPlace[1] - '0' - 1;
            m_Board[j,i].IsExposed = true;
        }

        public void CheckIfMatchesCardsAndTurningThem(string i_CardPlace1, string i_CardPlace2)
        {
            int ColumnCard1 = i_CardPlace1[0] - 'A';
            int RowsCard1 = i_CardPlace1[1] - '0' - 1;
            int ColumnCard2 = i_CardPlace2[0] - 'A';
            int RowsCard2 = i_CardPlace2[1] - '0' - 1;

            if (m_Board[RowsCard1, ColumnCard1].Value != m_Board[RowsCard2, ColumnCard2].Value)
            {
                m_Board[RowsCard1, ColumnCard1].IsExposed = false;
                m_Board[RowsCard2, ColumnCard2].IsExposed = false;
            }
        }
    }
}
