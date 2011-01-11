using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Diagnostics;

namespace LSRI.AuditoryBubbleBlast
{
    /// <summary>
    /// 
    /// </summary>
    public class Bullet
    {
        /// <summary>
        /// 
        /// </summary>
        public enum  Direction
        {
            None = 0,
            Up = 1, 
            Right = 2, 
            Down = 3, 
            Left = 4
        };

        public int _val;
        public Direction _dir = Direction.None;


        public Bullet(Direction d)
        {
            this._dir = d;
            _val = 0;
        }

        public override string ToString()
        {
            return "" + _dir;
        }

    }

    public class Cell
    {
        public int _posX;
        public int _posY;
        public int _val;
        public List<Bullet> _bullets = null;
        public List<Bullet> _newbullets = null;

        public Cell() : this(0,0)
        {
        }

        public Cell(int i,int j)
        {
            _bullets = new List<Bullet>();
            _newbullets = new List<Bullet>();
            _val = 0;
        }
    }

    public class Grid
    {
        public const int _dimX = 15;
        public const int _dimY = 15;

        public Cell[,] _grid = null;
        public int _nbBullets;

        public Grid()
        {
             this._grid = new Cell[_dimX, _dimY];
             for (int i = 0; i < _dimX; i++)
             {
                 for (int j = 0; j < _dimY; j++)
                 {
                     _grid[i, j] = new Cell(i,j);
                 }
             }
        }

        public void dedbug()
        {
            Debug.WriteLine("--------------------");
            for (int i = 0; i < _dimX; i++)
            {

                String str = "";
                for (int j = 0; j < _dimY; j++)
                {
                    Cell c = _grid[i, j]; 
                    //str += "" + c._val + String.Join(",",c._bullets);
                    int nb = (c._val + c._bullets.Count);
                    str += "" + ((nb==0)? "~" : ""+nb);

                }
                Debug.WriteLine("{0}", str);
            }
            Debug.WriteLine("--------------------");

        }

        public void fire()
        {
            if (_nbBullets == 0) return;

            for (int i = 0; i < _dimX; i++)
            {
                for (int j = 0; j < _dimY; j++)
                {
                    Cell c = _grid[i, j];
                    if (c._bullets.Count == 0) continue;

                    if (c._val == 0)
                    {
                        // move bullets
                    }
                    else
                    {
                        // explode, clear bullets, new bullets
                        c._val -= Math.Max(c._bullets.Count,0);
                        c._bullets.Clear();
                        if (c._val == 0)
                        {
                            c._bullets.Add(new Bullet(Bullet.Direction.Left));
                            c._bullets.Add(new Bullet(Bullet.Direction.Up));
                            c._bullets.Add(new Bullet(Bullet.Direction.Right));
                            c._bullets.Add(new Bullet(Bullet.Direction.Down));
                        }
                    }


                    for (int k = 0; k < c._bullets.Count; k++)
                    {
                            
                            Bullet _bul = c._bullets[k];
                            switch (_bul._dir)
                            {
                                case Bullet.Direction.Up:
                                    if (i != 0)
                                    {
                                        Cell cu = _grid[i - 1, j];
                                        cu._newbullets.Add(_bul);
                                    }
                                    break;
                                case Bullet.Direction.Right:
                                    if (j < _dimY - 1)
                                    {
                                        Cell cu = _grid[i, j + 1];
                                        cu._newbullets.Add(_bul);
                                    }
                                    break;
                                case Bullet.Direction.Down:
                                    if (i < _dimX - 1)
                                    {
                                        Cell cu = _grid[i + 1, j];
                                        cu._newbullets.Add(_bul);
                                    }
                                    break;
                                case Bullet.Direction.Left:
                                    if (j != 0)
                                    {
                                        Cell cu = _grid[i, j-1];
                                        cu._newbullets.Add(_bul);
                                    }
                                    break;
                            }
 
                    }
                    c._bullets.Clear();

                }
            }

            // update grid
            _nbBullets = 0;
            for (int i = 0; i < _dimX; i++)
            {
                for (int j = 0; j < _dimY; j++)
                {
                    Cell c = _grid[i, j];
                    c._bullets.AddRange(c._newbullets);
                    c._newbullets.Clear();
                    _nbBullets += c._bullets.Count;
                }
            }


        }
    }

}
