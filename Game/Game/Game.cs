using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game
{
    public class GameNAndC
    {
        private string[,] table;
        private bool X;
        public string winner { get; private set; }

        public GameNAndC()
        {
            this.X = true;
            table = new string[3, 3];
            winner = "";
        }

        public void NewItem(int column, int row)
        {
            if (X)
            {
                table[row, column] = "x";
            }
            else
            {
                table[row, column] = "0";
            }

            if (ColumnWinner())
                return;
            if (RowWinner())
                return;
            if (DiagWinner())
                return;
            
        }

        private bool RowWinner()
        {
            if (table[0, 0] == table[0, 1]  && table[0, 2] == table[0, 1])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            if (table[1, 0] == table[1, 1] && table[1, 2] == table[1, 1])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            if (table[2, 0] == table[2, 1] && table[2, 2] == table[2, 1])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            return false;
        }

        private bool ColumnWinner()
        {
            if (table[0, 0] == table[1, 0] && table[2, 0] == table[1, 0])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            if (table[0, 1] == table[1, 1] && table[2, 1] == table[1, 1])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            if (table[0, 2] == table[1, 2] && table[2, 2] == table[1, 2])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            return false;
        }

        private bool DiagWinner()
        {
            if (table[0, 0] == table[1, 1] && table[2, 2] == table[1, 1])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            if (table[0, 2] == table[1, 1] && table[2, 0] == table[1, 1])
            {
                if (X)
                {
                    this.winner = "x";
                    return true;
                }
                else
                {
                    this.winner = "0";
                    return true;
                }
            }

            return false;
        }

        public void ChangeGamer()
        {
            if (X)
                X = false;
            else
                X = true;
        }
    }
}
