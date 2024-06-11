using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class Logics
    {
        private int m_NumberOfPairsThatExposed;

        public Logics()
        {
            m_NumberOfPairsThatExposed = 0;
        }

        public int PairsThatExposed
        {
            get
            {
                return m_NumberOfPairsThatExposed;
            }
            set
            {
                m_NumberOfPairsThatExposed = value;
            }
        }
        public bool CheckIfMatchesCardsAndTurningThem(int[] i_Card1, int[] i_Card2, Board i_Board)
        {
            int RowsCard1 = i_Card1[0];
            int ColumnCard1 = i_Card1[1];
            int RowsCard2 = i_Card2[0];
            int ColumnCard2 = i_Card2[1];
            bool cardAreEqual = true;

            if (i_Board.GameBoard[RowsCard1, ColumnCard1].Value != i_Board.GameBoard[RowsCard2, ColumnCard2].Value)
            {
                i_Board.GameBoard[RowsCard1, ColumnCard1].IsExposed = false;
                i_Board.GameBoard[RowsCard2, ColumnCard2].IsExposed = false;
                cardAreEqual = false;
                System.Threading.Thread.Sleep(2000);
            }
            else
            {
                m_NumberOfPairsThatExposed++;
            }

            return cardAreEqual;
        }

        public bool CheckEndGame(Board i_Board)
        {
            bool endGame = false;

            if (m_NumberOfPairsThatExposed == (i_Board.Height * i_Board.Width) / 2)
            {
                endGame = true;
            }

            return endGame;
        }
    }
}
