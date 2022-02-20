using System;
using NLog.Web;
using System.IO;

namespace MovieLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "\\nlog.config";

            // create instance of Logger
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();

            string file = "movies.csv";
            string choice;
            do
            {
                Console.WriteLine("");
                Console.WriteLine("1) View all movies in the file.");
                Console.WriteLine("2) Add movies to the file.");
                Console.WriteLine("Enter any other key to exit.");

                choice = Console.ReadLine();

                if (choice == "1")
                {
                    // proccess data from file
                    if (File.Exists(file))
                    {
                        StreamReader sr = new StreamReader(file);
                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] arr = line.Split(',');
                            Console.WriteLine("ID: {0}, Movie: {1}, Genere: {2}",
                             arr[0], arr[1], arr[2]);
                            Console.WriteLine("");
                        }
                        sr.Close();
                    }
                    else
                    {
                        Console.WriteLine("File does not exist");
                    }
                }
                else if (choice == "2")
                {
                    int ID = 1;
                    string name;
                    string genere;
                    // add data to file
                    
                    {
                    StreamReader sr = new StreamReader(file);
                        Console.WriteLine("Add new movie (Y/N)?");
                        string resp = Console.ReadLine().ToUpper();
                        if (resp != "Y") { break; }

                        Console.WriteLine("What is the movie ID?");
                        try {
                            ID = int.Parse(Console.ReadLine());
                        } catch (Exception ex){
                            logger.Error(ex.Message);
                        }
                        string newID = ID.ToString();
                        bool dupID = false;
                        bool dupName = false;
                        
                        Console.WriteLine("What is the name of the movie?");
                        name = Console.ReadLine();

                         while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] arr = line.Split(',');
                            if (arr[1] == name) {
                                dupName = true;
                            }if (arr[0] == newID) {
                                dupID = true;   
                            }
                        }
                        
                        Console.WriteLine("What is the genere of the movie?");
                        genere = Console.ReadLine();
                        sr.Close();

                    StreamWriter sw = new StreamWriter(file, true);
                        if (dupID == false && dupName == false) {
                            sw.WriteLine(" {0},{1},{2}", ID, name, genere, "\n");
                            Console.WriteLine("Successfully added {0}.", name);
                        }else if (dupID == true) {
                            Console.WriteLine("Oops, looks like that ID is already in use, try again.");
                        }else if (dupName == true) {
                            Console.WriteLine("Oops, looks like that movie is already in the list.");
                        }
                        sw.Close();
                    }
                    
                }
            } while (choice == "1" || choice == "2");
        }
    }
}
