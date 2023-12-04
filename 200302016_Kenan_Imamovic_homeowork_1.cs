
// make sure to install Newtonsoft.Json
// command:
// dotnet add package Newtonsoft.Json

﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

class Program
{
    private static Dictionary<string, object> player = new Dictionary<string, object>()
    {
        {"name", ""},
        {"currentQuestionIndex", 0},
        {"correctAnswers", 0},
        {"incorrectAnswers", 0}
    };

    private static List<Dictionary<string, object>> questions;

    static void Main(string[] args)
    {
        questions = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(File.ReadAllText("Quiz_Questions.json"))["questions"];
        player["name"] = AskForName();

        if (player["name"].ToString().ToLower() == "exit")
        {
            return;
        }
        else
        {
            Console.WriteLine($"\nWelcome to my quiz, {player["name"]}!\n");
            Game();
        }
    }

    static string AskForName()
    {
        Console.Write("What is your name? (Type \"exit\" to exit) ");
        return Console.ReadLine();
    }

    static void Game()
    {
        if ((int)player["currentQuestionIndex"] < questions.Count)
        {
            AskQuestion((int)player["currentQuestionIndex"]);
        }
        else
        {
            GenerateReport();
        }
    }

static void AskQuestion(int index)
{
    var questionData = questions[index];
    var question = questionData["question"].ToString();
    var choices = ((Newtonsoft.Json.Linq.JArray)questionData["choices"]).ToObject<List<string>>();

    Console.WriteLine($"\n{question}\n");

    for (int i = 0; i < choices.Count; i++)
    {
        Console.WriteLine($"{i + 1}) {choices[i]}");
    }

    Console.Write($"\nEnter your choice (1-{choices.Count}): ");
    var userAnswer = Console.ReadLine();

    if (!ValidateAnswer(userAnswer, choices.Count))
    {
        Console.WriteLine($"Invalid input. Please enter a number between 1 and {choices.Count}.");
        AskQuestion(index);
    }
    else
    {
        ValidateQuestion(questionData, userAnswer);
    }
}

static bool ValidateAnswer(string answer, int numberOfChoices)
{
    int parsedAnswer;
    return int.TryParse(answer, out parsedAnswer) && parsedAnswer >= 1 && parsedAnswer <= numberOfChoices;
}


    static void ValidateQuestion(Dictionary<string, object> question, string userAnswer)
    {
        var correctAnswer = question["correctAnswer"].ToString();

        if (correctAnswer == userAnswer)
        {
            player["correctAnswers"] = (int)player["correctAnswers"] + 1;
            Console.WriteLine("Correct!");
        }
        else
        {
            player["incorrectAnswers"] = (int)player["incorrectAnswers"] + 1;
            Console.WriteLine("Incorrect!");
        }

        player["currentQuestionIndex"] = (int)player["currentQuestionIndex"] + 1;
        Game();
    }

    static void GenerateReport()
    {
        Console.WriteLine("\nQuiz Summary:");
        Console.WriteLine($"Name: {player["name"]}");
        Console.WriteLine($"Total Questions: {questions.Count}");
        Console.WriteLine($"Correct Answers: {player["correctAnswers"]}");
        Console.WriteLine($"Incorrect Answers: {player["incorrectAnswers"]}");
    }
}
