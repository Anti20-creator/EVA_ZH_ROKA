using System;
using System.Collections.Generic;
using System.Text;

namespace EVA_ZH.Model
{
    public class GameTable
    {
        private int _mapSize;
        private int[,] _map;

        public int getMapSize() { return _mapSize; }
        public int getMapElem(int i, int j)
        {
            if (i < _mapSize && j < _mapSize)
            {
                return _map[i, j];
            }
            else
            {
                throw new Exception();
            }
        }

        //setterek
        public void setMapElem(int i, int j, int val)
        {
            if (i < _mapSize && j < _mapSize)
            {
                _map[i, j] = val;
            }
            else
            {
                throw new Exception();
            }
        }

        public GameTable()
        {
            _map = new int[0, 0];
        }
        public GameTable(int x)
        {
            _mapSize = x;
            _map = new int[x, x];
            for(int i = 0; i < x; ++i)
            {
                for(int j = 0; j < x; ++j)
                {
                    _map[i, j] = 0;
                }
            }
        }
    }
}
