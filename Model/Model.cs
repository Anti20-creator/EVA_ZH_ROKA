using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace EVA_ZH.Model
{
    struct Pos
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Cheese
    {
        public Pos pos;
        public bool isBad;
        public Cheese(int size)
        {
            isBad = false;
            Random rnd = new Random();
            int x = rnd.Next(0, size);
            int y = rnd.Next(0, size);
            pos = new Pos(x, y);
        }
        public void makeBad()
        {
            isBad = true;
        }
    }
    class Player
    {
        public int health { get; private set; }
        public Pos pos { get; set; }
        public void damage(int x)
        {
            health -= x;
        }
        public bool dead()
        {
            return health <= 0;
        }
        public Player(int size)
        {
            pos = new Pos(0, 0);
            health = size * 3;
        }
        public void move(int plusx, int plusy, int size)
        {
            if(this.pos.X + plusx < 0 || this.pos.X + plusx > size ||
                this.pos.Y + plusy < 0 || this.pos.Y + plusy > size)
            {
                return;
            }
            else
            {
                this.pos = new Pos(this.pos.X + plusx, this.pos.Y + plusy);
            }
        }

    }
    public class GameModel
    {
        public int Size { get { return map.getMapSize(); } }
        private GameTable map;
        Player player;
        List<Cheese> cheeses;
        int gametime;
        int points;
        bool canMove;

        public EventHandler<int> generateTable;
        public EventHandler<int> refreshTable;
        public EventHandler<int> gameOver;
        public GameModel()
        {
            map = new GameTable();
            cheeses = new List<Cheese>();
        }
        public int getMapElem(int x, int y)
        {
            return map.getMapElem(x, y);
        }

        public void newGame(int size)
        {
            canMove = true;
            points = 0;
            gametime = 0;
            map = new GameTable(size);
            player = new Player(size);
            cheeses.Clear();
            cheeses.Add(new Cheese(size));

            OnGenerateTable(size);
            genMap();
        }

        private void genMap()
        {
            int size = Size;
            map = new GameTable(size);

            var badcheeses = cheeses.FindAll(e => e.isBad);
            var goodone = cheeses.Find(e => !e.isBad);
            if(goodone != null)
            {
                map.setMapElem(goodone.pos.X, goodone.pos.Y, 2);
            }
            foreach(Cheese c in badcheeses)
            {
                map.setMapElem(c.pos.X, c.pos.Y, 3);
            }

            map.setMapElem(player.pos.X, player.pos.Y, 1);

            OnRefreshTable(Size);
        }

        private void OnGenerateTable(int size)
        {
            if(generateTable != null)
            {
                generateTable(this, size);
            }
        }
        private void OnRefreshTable(int size)
        {
            if (refreshTable != null)
            {
                refreshTable(this, size);
            }
        }
        private void OnGameOver(int points)
        {
            if (gameOver != null)
            {
                gameOver(this, points);
            }
        }

        public void movePlayer(int x, int y) 
        {
            if (!canMove) return;
            player.move(x, y, Size);
            var cheese = cheeses.Find(e => e.pos.X == player.pos.X && e.pos.Y == player.pos.Y);
            if(cheese != null)
            {
                if (cheese.isBad)
                {
                    player.damage(3);
                }
                else
                {
                    points++;
                    player.damage(-2);
                }
                cheeses.RemoveAll(e => e.pos.X == player.pos.X && e.pos.Y == player.pos.Y);
            }
            canMove = false;
        }

        public void advanceTime()
        {
            canMove = true;
            gametime++;
            player.damage(1);

            genMap();


            if(gametime % 2 == 0 && gametime > 0)
            {
                cheeses.ForEach(e => e.makeBad());

                bool goodnew = false;
                while (!goodnew)
                {
                    Cheese c = new Cheese(Size);
                    var same = cheeses.Find(e => e.pos.X == c.pos.X && e.pos.Y == c.pos.Y);
                    if (same == null)
                    {
                        goodnew = true;
                        cheeses.Add(c);
                    }
                }
            }
            if (player.dead() || cheeses.Count == Size * Size - 1)
            {
                OnGameOver(points);
                return;
            }
        }

        public int getHealth()
        {
            return player.dead() ? 0 : player.health;
        }
    }
}
