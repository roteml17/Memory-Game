using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class Player
    {
        private int[] m_Card1 = new int[2];
        private int[] m_Card2 = new int[2];
        private string m_PlayerNmae;
        private int m_PlayerScore;
        private bool m_IsHuman;
        bool m_MyTurn;

        public Player(string i_Name, bool i_IsHuman, bool i_MyTurn)
        {
            m_PlayerNmae = i_Name;
            m_IsHuman = i_IsHuman;
            m_PlayerScore = 0;
            m_MyTurn = i_MyTurn;
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

        public string Name
        {
            get
            {
                return m_PlayerNmae;
            }
        }
    }
}
