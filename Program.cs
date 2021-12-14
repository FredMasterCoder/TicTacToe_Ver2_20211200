using System;
using System.Threading;
//using System.Text.RegularExpressions; // may need to be installed, could have scurity issues

//  C# .NET Core 3.1

namespace TicTacToe_20211200
{
    class TicTacToeGame
    {
        //-------------------------------------------------------
        //Author Fred Menke
        //Date: Dec/13/2021
        //----------------------------------------------------

        static char[] gamearr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };   // array - zero index holder  -- game positions

        static int player = 1; //By default player 1 is first 
        static int usercheck = 1;

        static string userinput; // raw data input by player
        static int validateChoice = -2; // Input data validation  -2 assume wrong till confirmed right
        static int choice = 0; //Validate input data  

        static int flag = 0;  // game status

        static int errstatus = -2;  // to track errors

        static void Main(string[] args)
        {

            // loop til game over
            do
            {
                IntroHeader();

                player = PlayerCheck(usercheck, player);   // santa

                Console.WriteLine("   Player {0} Chance", player);
                Console.WriteLine("    Make a Selection: \n");

                GameBoard();     // Draw the GameBoard after every move

                // read input from console ---------------------------------------------

                //Get user input  ---------------------------------------------
                validateChoice = -2; // assume wrong each time untill confirmed right

                userinput = Console.ReadLine();
                string msg = "You entered: " + userinput;
                Console.WriteLine(msg);

                validateChoice = ValidateUserEntry(userinput);  ///check
                Thread.Sleep(1000);

                // Input valid, mark on board
                if (validateChoice == -2)
                {
                    // bad data, no update
                    choice = 0;
                    usercheck = -2; // keep user the same
                }
                else
                {
                    // good data update 
                    choice = validateChoice;
                    usercheck = player;
                    errstatus = MarkPlayerSelection(choice);
                }

                flag = CheckForWinner2();

            } while (flag != 1 && flag != -1);


            // This loop will run until there is a winner, or a draw (all cells marked with an x or o) 

            Console.Clear();
            GameBoard();  // draw GameBoard again after player makes a choice

            CheckGameOver(flag);

        } // static main

        private static void IntroHeader()
        {
            // when loop starts the screen will be cleared 
            Console.Clear();
            Console.WriteLine("****   WELCOME TO TIC TIX TOE   ****");
            Console.WriteLine("           TWO PLAYERS");
            Console.WriteLine(" PLEASE PICK A NUMBER ON THE BAORD");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("      Player 1 WILL BE X");
            Console.WriteLine("      Player 2 WILL BE O");
            Console.WriteLine("----------------------------------");
            Console.WriteLine("\n");
        }
        private static int PlayerCheck(int usercheck, int player)
        {
            // Do not change the player if the data entered is not valid
            // Do not update the game board position array if the data entered is not valid
            // Prompt the same player for another value

            if (usercheck != -2)  // current player had a problem - no change
            {
                // either 1 or 2 update player status

                if (player % 2 == 0)//checking whos turn it is  
                {
                    player = 2;
                }
                else
                {
                    player = 1;
                }
            }
            return player;
        }

        private static void GameBoard() // Draw Game Board - called each time a player moves
        {
            // load the array into the game board

            Console.WriteLine("Game Board Updated");
            Console.WriteLine("   ");

            string gridvert = "       |     |      ";
            string gridhorz = "  _____|_____|_____ ";

            Console.WriteLine(gridvert);
            Console.WriteLine("    {0}  |  {1}  |  {2}", gamearr[1], gamearr[2], gamearr[3]);
            Console.WriteLine(gridhorz);
            Console.WriteLine(gridvert);
            Console.WriteLine("    {0}  |  {1}  |  {2}", gamearr[4], gamearr[5], gamearr[6]);
            Console.WriteLine(gridhorz);
            Console.WriteLine(gridvert);
            Console.WriteLine("    {0}  |  {1}  |  {2}", gamearr[7], gamearr[8], gamearr[9]);
            Console.WriteLine(gridvert);
        }

        private static int ValidateUserEntry(string user_entered)
        {
            // Note Regex may have been simpler but that may have had install issue with the dll for this demo

            string errmsg = "You did not select a number from 1 to 9";
            int tttmove = -2;   // set to - 2 for errors
            try
            {

                bool b1;
                b1 = String.IsNullOrEmpty(userinput);  // test for empty

                if (b1 == true)
                {
                    // test any value
                    tttmove = -2;
                    Console.WriteLine(errmsg);
                }
                else
                {
                    // test if integer
                    int ix = 0;
                    bool result = int.TryParse(userinput, out ix); //i now = 108  
                    if (result == false)
                    {
                        tttmove = -2;
                        Console.WriteLine(errmsg);
                    }
                    else
                    {
                        // test if it an integer 1 to 9
                        int irange = 0;
                        irange = int.Parse(userinput);
                        if (1 <= irange && irange <= 9)     ///(resultrange == false)
                        {
                            //  ok a valid integer  !!! Wahooo!
                            tttmove = int.Parse(userinput);
                        }
                        else
                        {
                            tttmove = -2;
                            Console.WriteLine(errmsg);
                        }
                    }
                }
            }
            catch
            {
                //  Inform user and return -2  let them try again
                //  as this is a tic tax toe game no cause to stop the game
                tttmove = -2;
                Console.WriteLine("You entered something wierd, please stick to the game rules");
                Console.WriteLine(errmsg);
                Thread.Sleep(3000);
            }

            return tttmove;
        }

        private static void CheckGameOver(int flag)  // CheckForWinner
        {

            try
            {
                if (flag == 1) // = 1 then we have a winner
                {
                    Console.WriteLine("Player {0} has won the game!", (player % 2) + 1);
                }
                else if (flag == 0)
                {
                    Console.WriteLine("Player {0} Please pic a value between 1 and 9", (player % 2) + 1);
                }
                else  // if flag value is -1 the match is a draw and no winner  
                      // (all cells marked with an x or o) 
                {
                    Console.WriteLine("The match is a Draw");
                }
            }
            catch
            {
                Console.WriteLine("Sorry the game has experienced an error");
            }


            Thread.Sleep(3000);

        }

        private static int CheckForWinner2()     // return value 1 if there is a winner
        {
            // Check 5 conditions
            // Draw, Horizon, Verticle, Diagonal, still playing

            int returnstatus = -2;  //  default to error if this fails

            if (CheckDraw(gamearr) == -1)  //Draw
            {
                returnstatus = -1;
            }
            // check for 3 in a row

            if (returnstatus != -1) // not a draw
            {
                if (CheckHorizontal(gamearr) == 1)  // Horizontal
                {
                    returnstatus = 1;
                }
                else if (CheckVerticle(gamearr) == 1) //Verticle
                {
                    returnstatus = 1;
                }
                else if (CheckDiagonal(gamearr) == 1) // Diagonal
                {
                    returnstatus = 1;
                }
                else
                {
                    returnstatus = 0; // still playing
                }
            }

            return returnstatus;
        }
        private static int CheckDraw(char[] gamearr)
        {
            int returnstatus = -2;// initiate with fail code

            if (gamearr[1] != '1' && gamearr[2] != '2' && gamearr[3] != '3' && gamearr[4] != '4' && gamearr[5] != '5' && gamearr[6] != '6' && gamearr[7] != '7' && gamearr[8] != '8' && gamearr[9] != '9')
            {
                returnstatus = -1;
            }
            return returnstatus;
        }
        private static int CheckHorizontal(char[] gamearr)
        {
            int returnstatus = -2; // initiate with fail code

            //Row   1
            if (gamearr[1] == gamearr[2] && gamearr[2] == gamearr[3])
            {
                returnstatus = 1;
            }
            //Row  2 
            else if (gamearr[4] == gamearr[5] && gamearr[5] == gamearr[6])
            {
                returnstatus = 1;
            }
            //Row   3
            else if (gamearr[6] == gamearr[7] && gamearr[7] == gamearr[8])
            {
                returnstatus = 1;
            }
            return returnstatus;
        }
        private static int CheckVerticle(char[] gamearr)
        {
            int retstatus = -2;// initiate with fail code

            //Column 1      
            if (gamearr[1] == gamearr[4] && gamearr[4] == gamearr[7])
            {
                retstatus = 1;
            }
            //Column  2
            else if (gamearr[2] == gamearr[5] && gamearr[5] == gamearr[8])
            {
                retstatus = 1;
            }
            //Column 3 
            else if (gamearr[3] == gamearr[6] && gamearr[6] == gamearr[9])
            {
                retstatus = 1;
            }
            return retstatus;
        }
        private static int CheckDiagonal(char[] gamearr)
        {
            int returnstatus = -2; // initiate with fail code
            if (gamearr[1] == gamearr[5] && gamearr[5] == gamearr[9])
            {
                returnstatus = 1;
            }
            else if (gamearr[3] == gamearr[5] && gamearr[5] == gamearr[7])
            {
                returnstatus = 1;
            }
            return returnstatus;
        }
        private static int MarkPlayerSelection(int choice)
        {
            int retstatus = -2;

            // checking that position the user wants is taken or not --- marked (with X or O)
            // update the array with an X or an O

            if (validateChoice != -2)
            {
                //  good data
                if (gamearr[choice] != 'X' && gamearr[choice] != 'O')
                {
                    //if selected is player 2 then mark O - else mark X  

                    if (player % 2 == 0)
                    {
                        gamearr[choice] = 'O';
                        player++;
                    }
                    else
                    {
                        gamearr[choice] = 'X';
                        player++;
                    }
                }
                else //Position already marked - show message - load GameBoard again  
                {
                    Console.WriteLine("Sorry the row {0} is already marked with {1}", choice, gamearr[choice]);
                    Console.WriteLine("\n");
                    Console.WriteLine("Please wait game board is refreshing.....");
                    Thread.Sleep(3000);
                }
                retstatus = 0;
            }
            return retstatus;
        }

    }  // program
}  // namespace
