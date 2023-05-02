using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CS_ttt.Program.ConcretePlayer;

namespace CS_ttt
{
    public class Program
    {
        public interface ICommand
        {
            void Execute();
            void Undo();
        }
        public class CommandManager
        {
            public static CommandManager commandManager;

            public static CommandManager getCommandeManager()
            {
                if ((commandManager == null))
                {
                    commandManager = new CommandManager();
                }

                return commandManager;
            }

            private Stack<ICommand> history = new Stack<ICommand>();

             Stack<ICommand> redo = new Stack<ICommand>();

            public virtual void clearRedo()
            {
                this.redo.Clear();
            }

            public void Redo()
            {
                if (!this.redo.Contains(null))
                {
                    ICommand command = this.redo.Pop();

                    command.Execute();

                    this.history.Push(command);
                }
            }

            public void undo()
            {
                if (!this.history.Contains(null))
                {
                    ICommand command = this.history.Pop();

                    command.Undo();
                     
                    this.redo.Push(command);
                }
            }
            public void execute(ICommand command)
            {
                this.history.Push(command);
                 
                this.redo.Clear();

                command.Execute();
            }
        }

        public class ConcretePlayer : Player
        {
            private string name;

            private string symbol;

            private bool role;

            public ConcretePlayer(string name, string symbol, bool role)
            {
                this.name = name;

                this.symbol = symbol;

                this.role = role;
            }

            //[Override()]
            public void play(int i, int j)
            {
                //  TODO Auto-generated method stub
                ICommand command = new PlayRole(i, j, this.symbol);

                CommandManager.getCommandeManager().execute(command);
            }

            public string getName()
            {
                return this.name;
            }

            public void setName(string name)
            {
                this.name = name;
            }

            public bool isRole()
            {
                return this.role;
            }

            public void setRole(bool role)
            {
                this.role = role;
            }

            public bool getRole()
            {
                return this.role;
            }

            public void setSymbole(string symbole)
            {
                this.symbol = symbole;
            }

            public string getSymbol()
            {
                return this.symbol;
            }

            public class Game
            {
                public static Game game;
                public static Game getGame()
                {
                    if ((game == null))
                    {
                        game = new Game();
                    }

                    return game;
                }

                // Tick tack toe matrix
                private string[,] matrix = {{"-","-","-"}
                                            ,{"-","-","-"},
                                            {"-","-","-"}};

        
                // current player role
                private bool role = false;

                public void updateMatrix(string s, int i, int j)
                {
                    this.matrix[i,j] = s;
                }

                public bool checkWin(string symbole)
                {
                    if (((this.matrix[0,0].Equals(symbole)
                                && (this.matrix[0,1].Equals(symbole) && this.matrix[0,2].Equals(symbole)))
                                || ((this.matrix[0,0].Equals(symbole)
                                && (this.matrix[1,1].Equals(symbole) && this.matrix[2,2].Equals(symbole)))
                                || ((this.matrix[0,1].Equals(symbole)
                                && (this.matrix[0,2].Equals(symbole) && this.matrix[0,3].Equals(symbole)))
                                || ((this.matrix[1,0].Equals(symbole)
                                && (this.matrix[1,1].Equals(symbole) && this.matrix[1,2].Equals(symbole)))
                                || ((this.matrix[2,0].Equals(symbole)
                                && (this.matrix[2,1].Equals(symbole) && this.matrix[2,2].Equals(symbole)))
                                || ((this.matrix[0,1].Equals(symbole)
                                && (this.matrix[1,1].Equals(symbole) && this.matrix[2,1].Equals(symbole)))
                                || ((this.matrix[0,2].Equals(symbole)
                                && (this.matrix[1,2].Equals(symbole) && this.matrix[2,2].Equals(symbole)))
                                || (this.matrix[0,2].Equals(symbole)
                                && (this.matrix[1,1].Equals(symbole) && this.matrix[2,0].Equals(symbole)))))))))))
                    {
                        return true;
                    }

                    return false;
                }

                // role getter
                public bool getRole()
                {
                    return this.role;
                }

                public void setRole(bool role)
                {
                    this.role = role;
                }

                public void printMatrix()
                {
                    for (int i = 0; (i < 3); i++)
                    {
                        Console.Write("||");
                        for (int j = 0; (j < 3); j++)
                        {
                            Console.Write((this.matrix[i,j] + " || "));
                        }

                        Console.Write("\n");
                    }
                }

                public bool isEmpty(int i, int j)
                {
                    return (this.matrix[i,j] == "-");
                }
            }

            public class PlayRole : ICommand
            {

                private int i;

                private int j;

                private string symbol;

                public PlayRole(int i, int j, string symbol)
                {
                    this.i = i;
                    this.j = j;
                    this.symbol = symbol;
                }

                //[Override()]
                public void Execute()
                {
                    //  TODO Auto-generated method stub
                    Game.getGame().updateMatrix(this.symbol, this.i, this.j);

                    Game.getGame().setRole(!Game.getGame().getRole());
                }

                //[Override()]
                public void Undo()
                {
                    //  TODO Auto-generated method stub
                    Game.getGame().updateMatrix("-", this.i, this.j);

                    Game.getGame().setRole(!Game.getGame().getRole());
                }
            }

            public interface Player
            {
                void play(int i, int j);
            }


                public static void Main(string[] args)
                {
                    //  TODO Auto-generated method stub 
                    ConcretePlayer player1 = new ConcretePlayer("Player1", "x", true);
                    ConcretePlayer player2 = new ConcretePlayer("Player2", "o", false);
                    ConcretePlayer currentPlayer;

                    //BufferedReader reader = new BufferedReader(new InputStreamReader(System.in));

                    while (true)
                    {
                        // print the game matrix
                        Game.getGame().printMatrix();

                        if ((Game.getGame().getRole() == player1.getRole()))
                        {
                            Console.Write(("it\'s " + (player1.getName() + " role !")));

                            currentPlayer = player1;
                        }
                        else
                        {
                            Console.Write(("it\'s " + (player2.getName() + " role !")));

                            currentPlayer = player2;
                        }

                        Console.Write(("(undo/redo/" + (currentPlayer.getSymbol() + ")\n")));

                        try
                        {
                            string console = Console.ReadLine();

                            if (currentPlayer.getSymbol().Equals(console))
                            {
                                try
                                {
                                    Console.Write("please enter line\n");

                                    string line = Console.ReadLine();

                                    int i = Int32.Parse(line);

                                    Console.Write("please enter column\n");

                                    string column = Console.ReadLine();

                                    int j = Int32.Parse(column);
                                    if (Game.getGame().isEmpty(i, j))
                                    {
                                        currentPlayer.play(i, j);

                                        if (Game.getGame().checkWin(currentPlayer.getSymbol()))
                                        {
                                            Console.WriteLine((currentPlayer.getName() + " won the game !"));
                                            break;
                                        }
                                    }

                                    Console.WriteLine("please enter a valid line and column");
                                }
                                catch (Exception e)
                                {
                                //  TODO: handle exception
                                Console.WriteLine("please enter a valid line and column");
                                }
                            }
                            else if ("redo".Equals(console))
                            {
                                CommandManager.getCommandeManager().Redo();
                            }
                            else if ("undo".Equals(console))
                            {
                                CommandManager.getCommandeManager().undo();
                            }
                            else
                            {
                                Console.WriteLine("please enter a valid command !");
                            }

                        }
                        catch (IOException e)
                        {
                            //  TODO Auto-generated catch block
                            Console.WriteLine(e.StackTrace);
                        }
                    }
                }
            
        }
        
    }
    
}

