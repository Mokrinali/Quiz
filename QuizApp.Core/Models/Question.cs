﻿namespace QuizApp.Core.Models
{
    public class Question
    {
        public string Text { get; set; }
        public List<Answer> Answers { get; set; } = new();
    }
}
