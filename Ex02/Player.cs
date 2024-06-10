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
                set
                {
                    numberOfCardsThatExposed = value;
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
            for (int i = 0; i < i_BoardSize; i++)
            {
                m_CardKeepers.Add(new CardKeeper());
            }
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

        public int[] ComputerIsPlaying(Board i_board)
        {
            Random random = new Random();
            int[] cardFounded = new int[2];
            bool cardFoundedIsTrue = false;

            for (int i = 0; i < m_sizeCardKeepers; i++)
            {
                if (m_CardKeepers[i].CardsExposed == 2)
                {
                    // CHANGED: Switched indices to match UserInterface
                    if (!i_board.GameBoard[m_CardKeepers[i].TheCardsThatExposed[0][0], m_CardKeepers[i].TheCardsThatExposed[0][1]].IsExposed)
                    {
                        cardFounded = m_CardKeepers[i].TheCardsThatExposed[0];
                        cardFoundedIsTrue = true;
                        break;
                    }
                    else if (!i_board.GameBoard[m_CardKeepers[i].TheCardsThatExposed[1][0], m_CardKeepers[i].TheCardsThatExposed[1][1]].IsExposed)
                    {
                        cardFounded = m_CardKeepers[i].TheCardsThatExposed[1];
                        cardFoundedIsTrue = true;
                        break;
                    }
                }
            }
            if (!cardFoundedIsTrue)
            {
                cardFounded = ChooseARandomCardForComputer(random, i_board);
            }
            return cardFounded;
        }

        public int[] ChooseARandomCardForComputer(Random random, Board i_board)
        {
            int height = random.Next(i_board.Height);
            int width = random.Next(i_board.Width);
            int[] randomCardChoseByComputer = { height, width }; // CHANGED: Switched order to match UserInterface
            int value = i_board.GameBoard[height, width].Value;
            bool theCardIsAlreadyIn = false, theCardIsAlreadyExposed = false, weFoundANewCard = false;

            while (!weFoundANewCard)
            {
                for (int j = 0; j < m_CardKeepers[value].TheCardsThatExposed.Count; j++)
                {
                    // CHANGED: Switched order to match UserInterface
                    if (randomCardChoseByComputer[0] == m_CardKeepers[value].TheCardsThatExposed[j][0] &&
                                randomCardChoseByComputer[1] == m_CardKeepers[value].TheCardsThatExposed[j][1])
                    {
                        theCardIsAlreadyIn = true;
                        break;
                    }
                }
                if (i_board.GameBoard[height, width].IsExposed)
                {
                    theCardIsAlreadyExposed = true;
                }

                if (theCardIsAlreadyExposed || theCardIsAlreadyIn)
                {
                    height = random.Next(i_board.Height);
                    width = random.Next(i_board.Width);
                    randomCardChoseByComputer[0] = height; // CHANGED: Switched order to match UserInterface
                    randomCardChoseByComputer[1] = width; // CHANGED: Switched order to match UserInterface
                    value = i_board.GameBoard[height, width].Value;

                    theCardIsAlreadyExposed = theCardIsAlreadyIn = false;
                }
                else
                {
                    m_CardKeepers[value].TheCardsThatExposed.Add(randomCardChoseByComputer); // CHANGED: Added clone to ensure original is not modified
                    m_CardKeepers[value].CardsExposed++;
                    weFoundANewCard = true;
                }
            }
            return randomCardChoseByComputer;
        }
    }
}