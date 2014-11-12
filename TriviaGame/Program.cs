﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriviaGame
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Trivia q = new Trivia("Lyrics: making love to you was never second best*melt with you");
            Console.WriteLine(q.Category);
            Console.WriteLine(q.Question);
            Console.WriteLine(q.Answer);
            Console.ReadKey(); */
            //The logic for your trivia game happens here
            List<Trivia> AllQuestions = GetTriviaList();
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
        //TODO: Fill out the Trivia Object

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
            if (garbledString.Contains(":"))
            {
                this.Category = garbledString.Split(':').First().ToString();
            }
            this.Question = garbledString.Split(':').Last().Split('*').First().Trim();
            this.Answer = garbledString.Split('*').Last();
        }
    }
}
