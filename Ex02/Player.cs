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

        public Player(string i_Name, bool i_IsHuman)
        {
            m_PlayerNmae = i_Name;
            m_IsHuman = i_IsHuman;
            m_PlayerScore = 0;
        }
        public int[] Card1
        {
            get
            {
                return Card1;
            }
            set
            {
                Card1 = value;
            }
        }

        public int[] Card2
        {
            get
            {
                return Card2;
            }
            set
            {
                Card2 = value;
            }
        }
    }
}
