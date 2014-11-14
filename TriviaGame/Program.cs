using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaGame
{
    class Program
    {
        // Lists that will be used at the end to tabulate results page
        public static List<Trivia> CorrectList = new List<Trivia>();
        public static List<Trivia> WrongList = new List<Trivia>();

        // Needed for other pieces of logic in the game
        public static List<Trivia> AllQuestions;

        static void Main(string[] args)
        {
            // Begin the game
            InitGame();
            
        }

        public static void InitGame()
        {
            //The logic for your trivia game happens here
            AllQuestions = GetTriviaList();

            // reset global variables
            CorrectList.Clear();
            WrongList.Clear();

            // Start the game and ask the user if they want all questions or categories
            PrintIntroScreen();
        }
        /// <summary>
        /// Function to run the main engine of asking questions until the total # have been asked
        /// </summary>
        /// <param name="category">Category, "0" will mean all</param>
        public static void QuestionEngine(string category)
        {
            List<Trivia> possibleQ;
            Random rng = new Random();
            Console.Clear();
            if (category == "0")
            {
                // All questions
                possibleQ = AllQuestions;
            } else {
                // Get all questions that haven't been used yet for a category
                possibleQ = AllQuestions.Where(x => x.Category.ToLower().StartsWith(category.ToLower())).ToList();
            }
            Console.WriteLine("How many questions do you want to answer? (max {0} questions)", possibleQ.Count);
            // set to max
            int userInputNum = possibleQ.Count;
            int totalQuestions = possibleQ.Count;
            string userInput = Console.ReadLine();
            // Make sure a number is coming in
            try { userInputNum = int.Parse(userInput); }
            catch {}
            if (userInputNum <= possibleQ.Count)
            {
                // User input was less then the total amount of questions, so change the max questions
                totalQuestions = userInputNum;
            }
            
            while (totalQuestions >0)
            {
                List<Trivia> currentQ;
                // Make sure to get the remaining questions that haven't been used yet
                if (category == "0")
                {
                    // All questions, unused
                    currentQ = AllQuestions;
                }
                else
                {
                    // Questions from a certain category, unused
                    currentQ = AllQuestions.Where(x => x.Category.ToLower().StartsWith(category.ToLower())).ToList();
                }

                Trivia currentTempQuestion = currentQ.ElementAt(rng.Next(currentQ.Count));
                AskQuestion(currentTempQuestion, totalQuestions);
                AllQuestions.Remove(currentTempQuestion);
                totalQuestions--;
            }

            // Once they have finished print the result screen
            PrintResults();

        }
        /// <summary>
        /// A function to ask a question using a Trivia Object
        /// </summary>
        /// <param name="currentQuestion">Trivia obj to ask</param>
        private static void AskQuestion(Trivia currentQuestion,int questionsToGo)
        {
            Console.Clear();
            Console.WriteLine("There are {0} questions to go", questionsToGo);
            if(currentQuestion.Category!=null || currentQuestion.Category!=string.Empty) { Console.WriteLine("Category: {0}", currentQuestion.Category);}
            Console.WriteLine("Question: {0}", currentQuestion.Question);
            Console.WriteLine("Type your answer below:");
            string answer = Console.ReadLine();
            if (CleanAnswer(answer) == CleanAnswer(currentQuestion.Answer)) {
                // Correct answer
                CorrectList.Add(currentQuestion);
                Console.WriteLine("Correct!");
                System.Threading.Thread.Sleep(500);
            }
            else
            {
                // Wrong answer
                WrongList.Add(currentQuestion);
                Console.WriteLine("Wrong!");
                System.Threading.Thread.Sleep(500);
            }

        }
        /// <summary>
        /// Function to clean up a string for better comparision
        /// </summary>
        /// <param name="userInput">string to strip down</param>
        /// <returns>cleaned up string</returns>
        private static string CleanAnswer(string userInput)
        {
            StringBuilder tempString = new StringBuilder();
            foreach (char letter in userInput)
            {
                if (char.IsLetter(letter)) { tempString.Append(letter); }
            }
            return tempString.ToString().ToLower();
        }

        private static void PrintIntroScreen()
        {
            Console.Clear();
            Console.WriteLine("Do you want to play with all of the questions or just a certain category?");
            Console.Write("Type 1 for all or 2 for categories: ");
            string tempCategoryChoice = Console.ReadLine();
            if (tempCategoryChoice == "1")
            {
                // Start game with all questions
                QuestionEngine("0");
            }
            else
            {
                // Ask about which category
                PrintCategoryChoiceScreen();
            }

        }
        private static void PrintCategoryChoiceScreen()
        {
            Console.Clear();
            Console.WriteLine("Pick a category from the choices below:");
            foreach (string categoryText in AllQuestions.Select(x => x.Category).Distinct())
            {
                Console.WriteLine(categoryText);
            }
            Console.WriteLine("Type in the beginning of a Category to select that Category:");
            QuestionEngine(Console.ReadLine());

        }

        private static void PrintResults()
        {
            Console.Clear();
            Console.WriteLine("Thanks for playing, here's how you did:");
            int numCorrect = CorrectList.Count;
            Console.WriteLine("Number Correct: {0}", numCorrect);
            int numWrong = WrongList.Count;
            Console.WriteLine("Number Wrong: {0}", numWrong);
            int numTotal = numWrong + numCorrect;
            double numPer = (double)numCorrect / (double)numTotal;
            double numPercentage = numPer*100.0;
            Console.WriteLine("Percentage Correct: {0}%", Math.Round(numPercentage,2));

            Console.WriteLine("\nDo you want to play again? (Y for yes, N for no)");
            if("y"==Console.ReadLine().ToString().ToLower()) { InitGame(); }
        }

        //This functions gets the full list of trivia questions from the Trivia.txt document
        static List<Trivia> GetTriviaList()
        {
            //Get Contents from the file.  Remove the special char "\r".  Split on each line.  Convert to a list.
            List<string> contents = File.ReadAllText("trivia.txt").Replace("\r", "").Split('\n').ToList();

            //Each item in list "contents" is now one line of the Trivia.txt document.
            
            //make a new list to return all trivia questions
            List<Trivia> returnList = new List<Trivia>();
            // TODO: go through each line in contents of the trivia file and make a trivia object.
            //       add it to our return list.
            // Example: Trivia newTrivia = new Trivia("what is my name?*question");
            //Return the full list of trivia questions
            foreach (string text in contents)
            {
                returnList.Add(new Trivia(text));
            }
            return returnList;
        }
    }

    class Trivia
    {
        //The Trivia Object will have 2 properties
        // at a minimum, Question and Answer
        public string Category { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        //The Constructor for the Trivia object will
        // accept only 1 string parameter.  You will
        // split the question from the answer in your
        // constructor and assign them to the Question
        // and Answer properties

        public Trivia(string garbledString)
        {
            this.Category = string.Empty;
            if (garbledString.Split('*').First().Contains(":"))
            {
                this.Category = garbledString.Split(':').First().ToString();
            }
            this.Question = garbledString.Split(':').Last().Split('*').First().Trim();
            this.Answer = garbledString.Split('*').Last();
        }
    }
}
