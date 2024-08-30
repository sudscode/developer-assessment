using System;
using System.Collections.Generic;
using TodoList.Domain;

namespace TodoList.Api.UnitTests
{
    public class TestHelper

    {
        public static Guid TaskIdOne = Guid.Parse("6b0f7a39-0fd6-42c7-87b0-1e07bfaeb356");
        public static Guid TaskIdTwo = Guid.Parse("b3ffab1c-5d1e-4426-991f-ffabf2448011");

        public static List<TodoItem> SeedData()
        {
            return new List<TodoItem>
               {
                    new TodoItem
                    {
                        Id = TaskIdOne,
                        Description = "Task 1",
                        IsCompleted = false
                    },
                    new TodoItem
                    {
                        Id = TaskIdTwo,
                        Description = "Task 2",
                        IsCompleted = false
                    }

             };
        }
    }
}
