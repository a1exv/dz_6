using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication1
{


    class Karta
    {
        public int Value { get; set; }
        public char Mean { get; set; }
        public string Color { get; set; }
        public Karta(int value, char mean, string color)
        {
            Value = value;
            Mean = mean;
            Color = color;
        }
        public override string ToString()
        {
            return String.Format(Mean + Color);
        }
    }
    class Player
    {
        public void Win(object sender, EventArgs e)
        {
            Console.WriteLine(Name + " winning!");
        }
        
        public string Name { get; set; }
        public List<Karta> ruka = new List<Karta>();
        public Player(string name)
        {
            Name = name;
        }
        public static event EventHandler loosing;
        public override string ToString()
        {
            if (this.ruka.Count > 0) return String.Format(Name + "   " + ruka.ToString());
            else return Name;
        }
        public Karta opened = null;
        public void Open(object sender, EventArgs e)
        {
            
                opened = ruka.FindLast(delegate(Karta karta)
                    {
                        return karta != null;
                    });
                ruka.Remove(ruka.FindLast(delegate(Karta karta)
                {
                    return karta != null;
                }));
            }
        
    }
    class Program
    {
        public static event EventHandler loosing;
        public static event EventHandler win;
        public static int openCount = 0;
        public static void Loosing(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString() + "  loser!");
        }

        public static void Shuffle<T>(List<T> array, Random r)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = r.Next(n--);
                T tmp = array[n];
                array[n] = array[k];
                array[k] = tmp;
            }
        }
        public static event EventHandler open;
        static void Main(string[] args)
        {

            List<string> colors = new List<string>() { "\u2665", "\u2666", "\u2663", "\u2660" };
            List<char> means = new List<char>() { '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
            List<int> values = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<Karta> koloda = new List<Karta>();
            for (int i = 0; i < means.Count; i++)
            {
                for (int j = 0; j < colors.Count; j++)
                {
                    koloda.Add(new Karta(values[i], means[i], colors[j]));
                }
            }

            foreach (Karta karta in koloda)
            {
                Console.Write(karta.ToString() + "  ");
            }

            Random r = new Random();
            Shuffle(koloda, r);

            foreach (Karta karta in koloda)
            {
                Console.Write(karta.ToString() + "  ");
            }
            Player pasha = new Player("Pasha");
            Player dima = new Player("Dima");
            while (koloda.Count != 0)
            {
                pasha.ruka.Add(koloda.FindLast(delegate(Karta karta)
                {
                    return karta != null;
                }));
                koloda.Remove(koloda.FindLast(delegate(Karta karta)
                {
                    return karta != null;
                }));
                dima.ruka.Add(koloda.FindLast(delegate(Karta karta)
                {
                    return karta != null;
                }));
                koloda.Remove(koloda.FindLast(delegate(Karta karta)
                {
                    return karta != null;
                }));
            }
                open += pasha.Open;
                open += dima.Open;
                openCount++;
                openCount++;
                loosing += Loosing;
              
                Console.WriteLine("press any buttom to start game");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine(pasha.Name + "\t" + dima.Name);
            
            while (true)
            {
                open(null, EventArgs.Empty);
                Console.WriteLine(pasha.opened.ToString() + "\t" + dima.opened.ToString());
                //Thread.Sleep(200);
                if (pasha.opened.Value > dima.opened.Value)
                {
                    Console.WriteLine("__");
                    pasha.ruka.Insert(0, dima.opened);
                    pasha.ruka.Insert(0, pasha.opened);
                    pasha.opened = null;
                    dima.opened = null;
                }
                else if (dima.opened.Value > pasha.opened.Value)
                {
                    Console.WriteLine("  \t__");
                    dima.ruka.Insert(0, pasha.opened);
                    dima.ruka.Insert(0, dima.opened);
                    pasha.opened = null;
                    dima.opened = null;
                }
                else
                {
                    List<Karta> listPlayer1 = new List<Karta>();
                    List<Karta> listPlayer2 = new List<Karta>();
                    bool IsEqual = true;
                    while (IsEqual == true)
                    {
                      
                        Thread.Sleep(200);
                        if ((pasha.opened.Value > dima.opened.Value)||((dima.ruka.Count==0)&&(pasha.opened.Value==dima.opened.Value)))
                        {
                            Console.WriteLine("__");
                            listPlayer1.Add(pasha.opened);
                            listPlayer2.Add(dima.opened);
                            pasha.ruka.InsertRange(0, listPlayer1);
                            pasha.ruka.InsertRange(0, listPlayer2);
                            pasha.opened = null;
                            dima.opened = null;
                            IsEqual = false;
                        }
                        else if ((dima.opened.Value > pasha.opened.Value)||((pasha.ruka.Count==0)&&(pasha.opened.Value==dima.opened.Value)))
                        {
                            Console.WriteLine(" \t__");
                            listPlayer1.Add(pasha.opened);
                            listPlayer2.Add(dima.opened);
                            dima.ruka.InsertRange(0, listPlayer1);
                            dima.ruka.InsertRange(0, listPlayer2); 
                            pasha.opened = null;
                            dima.opened = null;
                            IsEqual = false;
                        }
                        else
                        {
                            listPlayer1.Add(pasha.opened);
                            listPlayer2.Add(dima.opened);

                            open(null, EventArgs.Empty);
                            Console.WriteLine(pasha.opened.ToString() + "\t" + dima.opened.ToString());
                        }
                    }
                }
                if (pasha.ruka.Count == 0)
                {
                    open -= pasha.Open;
                    openCount--;
                    loosing(pasha, EventArgs.Empty);
                    win += dima.Win;
                }
                if (dima.ruka.Count == 0)
                {
                    open -= dima.Open;
                    loosing(dima, EventArgs.Empty);
                    openCount--;
                    win += pasha.Win;
                }
                if (openCount == 1)
                {
                    win(null, EventArgs.Empty);
                    break;
                }
            }
        }
    }
}

