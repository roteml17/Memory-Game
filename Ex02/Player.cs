using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class Player
    {
        internal class CardKeeper
        {
            private int numberOfCardsThatExposed;
            private List<int[]> theCardsThatExposed;

            public CardKeeper()
            {
                numberOfCardsThatExposed = 0;
                theCardsThatExposed = new List<int[]>();
            }

            public int CardsExposed
            {
                get
                {
                    return numberOfCardsThatExposed;
                }
            }

            public List<int[]> TheCardsThatExposed
            {
                get
                {
                    return theCardsThatExposed;
                }
            }
        }

        private int[] m_Card1 = new int[2];
        private int[] m_Card2 = new int[2];
        private string m_PlayerNmae;
        private int m_PlayerScore;
        private bool m_IsHuman;
        bool m_MyTurn;
        int m_sizeCardKeepers;
        List<CardKeeper> m_CardKeepers;

        public Player(string i_Name, bool i_IsHuman, bool i_MyTurn)
        {
            m_PlayerNmae = i_Name;
            m_IsHuman = i_IsHuman;
            m_PlayerScore = 0;
            m_MyTurn = i_MyTurn;
        }

        public Player(bool i_IsHuman, bool i_MyTurn, int i_BoardSize)
        {
            m_PlayerNmae = "Computer";
            m_IsHuman = i_IsHuman;
            m_PlayerScore = 0;
            m_MyTurn = i_MyTurn;

            m_sizeCardKeepers = i_BoardSize;
            m_CardKeepers = new List<CardKeeper>(i_BoardSize);
        }

        public int[] Card1
        {
            get
            {
                return m_Card1;
            }
            set
            {
                m_Card1 = value;
            }
        }

        public int[] Card2
        {
            get
            {
                return m_Card2;
            }
            set
            {
                m_Card2 = value;
            }
        }

        public bool MyTurn
        {
            get
            {
                return m_MyTurn;
            }
            set
            {
                m_MyTurn = value;
            }
        }

        public int Score
        {
            get
            {
                return m_PlayerScore;
            }
            set
            {
                m_PlayerScore = value;
            }
        }

        public bool IsHuman
        {
            get
            {
                return m_IsHuman;
            }
        }

        public string Name
        {
            get
            {
                return m_PlayerNmae;
            }
        }

        public void ComputerIsPlaying(Board i_board)
        {
            Random random = new Random();

            for (int i = 0; i < m_sizeCardKeepers; i++)
            {
                if (m_CardKeepers[i].CardsExposed == 2)
                {
                    m_Card1 = m_CardKeepers[i].TheCardsThatExposed[0];
                    m_Card2 = m_CardKeepers[i].TheCardsThatExposed[1];
                    break;
                }
            }
            m_Card1 = ChooseARandomCardForComputer(random, i_board);
        }

        public int[] ChooseARandomCardForComputer(Random random, Board i_board)
        {
            int height =  random.Next(i_board.Height);
            int width = random.Next(i_board.Width);
            int[] placeInBoard = { height, width };

            for (int i = 0; i < m_sizeCardKeepers; i++)
            {
                for (int j = 0; j < m_CardKeepers[i].TheCardsThatExposed.Count; j++)
                {
                    if (placeInBoard[0] == m_CardKeepers[i].TheCardsThatExposed[j][0] &&
                            placeInBoard[1] == m_CardKeepers[i].TheCardsThatExposed[j][1])
                    {
                        
                    }
                }
            }
            return placeInBoard;
        }
    }
}

