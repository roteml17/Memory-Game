using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class Player
    {
        internal class CardKeeper
        {
            private int m_NumberOfCardsThatExposed;
            private List<int[]> m_TheCardsThatExposed;

            public CardKeeper()
            {
                m_NumberOfCardsThatExposed = 0;
                m_TheCardsThatExposed = new List<int[]>();
            }

            public int CardsExposed
            {
                get
                {
                    return m_NumberOfCardsThatExposed;
                }
                set
                {
                    m_NumberOfCardsThatExposed = value;
                }
            }

            public List<int[]> TheCardsThatExposed
            {
                get
                {
                    return m_TheCardsThatExposed;
                }
            }
        }

        private int[] m_Card1 = new int[2];
        private int[] m_Card2 = new int[2];
        private string m_PlayerNmae;
        private int m_PlayerScore;
        private bool m_IsHuman;
        bool m_MyTurn;
        int m_SizeCardKeepers;
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

            m_SizeCardKeepers = i_BoardSize;
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

        public int[] ComputerIsPlaying(Board i_Board)
        {
            Random randomComputerChoose = new Random();
            int[] cardFounded = new int[2];
            bool cardFoundedIsTrue = false;

            for (int i = 0; i < m_SizeCardKeepers; i++)
            {
                if (m_CardKeepers[i].CardsExposed == (int)eGameConfig.CardArreySize)
                {
                    if (!i_Board.GameBoard[m_CardKeepers[i].TheCardsThatExposed[0][0], 
                        m_CardKeepers[i].TheCardsThatExposed[0][1]].IsExposed)
                    {
                        cardFounded = m_CardKeepers[i].TheCardsThatExposed[0];
                        cardFoundedIsTrue = true;
                        break;
                    }
                    else if (!i_Board.GameBoard[m_CardKeepers[i].TheCardsThatExposed[1][0],
                        m_CardKeepers[i].TheCardsThatExposed[1][1]].IsExposed)
                    {
                        cardFounded = m_CardKeepers[i].TheCardsThatExposed[1];
                        cardFoundedIsTrue = true;
                        break;
                    }
                }
            }

            if (!cardFoundedIsTrue)
            {
                cardFounded = ChooseARandomCardForComputer(randomComputerChoose, i_Board);
            }

            return cardFounded;
        }

        public int[] ChooseARandomCardForComputer(Random i_RandomComputerChoose, Board i_Board)
        {
            int height = i_RandomComputerChoose.Next(i_Board.Height);
            int width = i_RandomComputerChoose.Next(i_Board.Width);
            int[] randomCardChoseByComputer = { height, width };
            int value = i_Board.GameBoard[height, width].Value;
            bool theCardIsAlreadyIn, theCardIsAlreadyExposed, weFoundANewCard = false;

            while (!weFoundANewCard)
            {
                theCardIsAlreadyIn = CheckIfTheCardIsAlreadyInTheArrey(value, randomCardChoseByComputer);
                theCardIsAlreadyExposed = CheckIfTheCardIsAlreadyExposed(height, width, i_Board);

                if (theCardIsAlreadyExposed || theCardIsAlreadyIn)
                {
                    height = i_RandomComputerChoose.Next(i_Board.Height);
                    width = i_RandomComputerChoose.Next(i_Board.Width);
                    randomCardChoseByComputer[0] = height;
                    randomCardChoseByComputer[1] = width;
                    value = i_Board.GameBoard[height, width].Value;
                    theCardIsAlreadyExposed = theCardIsAlreadyIn = false;
                }
                else
                {
                    AddACardToTheArrey(value, randomCardChoseByComputer);
                    weFoundANewCard = true;
                }
            }

            return randomCardChoseByComputer;
        }

        public bool CheckIfTheCardIsAlreadyInTheArrey(int i_Value, int[] i_RandomCardChoseByComputer)
        {
            bool theCardIsAlreadyIn = false;

            for (int j = 0; j < m_CardKeepers[i_Value].TheCardsThatExposed.Count; j++)
            {
                if (i_RandomCardChoseByComputer[0] == m_CardKeepers[i_Value].TheCardsThatExposed[j][0] &&
                            i_RandomCardChoseByComputer[1] == m_CardKeepers[i_Value].TheCardsThatExposed[j][1])
                {
                    theCardIsAlreadyIn = true;
                    break;
                }
            }

            return theCardIsAlreadyIn;
        }

        public bool CheckIfTheCardIsAlreadyExposed(int i_Height, int i_Width, Board i_Board)
        {
            bool theCardIsAlreadyExposed = false;

            if (i_Board.GameBoard[i_Height, i_Width].IsExposed)
            {
                theCardIsAlreadyExposed = true;
            }

            return theCardIsAlreadyExposed;
        }

        public void AddACardToTheArrey(int i_Value, int[] i_RandomCardChoseByComputer)
        {
            m_CardKeepers[i_Value].TheCardsThatExposed.Add(i_RandomCardChoseByComputer);
            m_CardKeepers[i_Value].CardsExposed++;
        }

        public void AddTheCardOfTheHumanToTheRememberArrey(int[] i_Card, Board i_Board)
        {
            int value = i_Board.GameBoard[i_Card[0], i_Card[1]].Value;
            bool theCardIsAlreadyExposed, theCardIsAlreadyIn;

            theCardIsAlreadyIn = CheckIfTheCardIsAlreadyInTheArrey(value, i_Card);
            theCardIsAlreadyExposed = CheckIfTheCardIsAlreadyExposed(i_Card[0], i_Card[1], i_Board);

            if (!theCardIsAlreadyExposed && !theCardIsAlreadyIn)
            {
                AddACardToTheArrey(value, i_Card);
            }

        }
    }
}