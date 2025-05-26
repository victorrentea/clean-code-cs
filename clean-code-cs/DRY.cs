using System;
namespace clean_code_cs
{

    public class SomeController
    {
        private SomeService someService = new SomeService();

        // [HttpGet("blue/{storeId}")]
        public void BlueEndpoint(int storeId, Task task)
        {
            someService.BlueMethod(storeId, task);
        }

        // [HttpGet("red/{storeId}")]
        public void RedEndpoint(int storeId, Task task)
        {
            someService.RedMethod(storeId, task);
        }
    }

    public class SomeService
    {
        public void BlueMethod(int id, Task task)
        {
            BooleanParameters.BigUglyMethod(id, task);
        }

        public void GreenMethod(int id, Task task)
        {
            BooleanParameters.BigUglyMethod(id, task);
        }

        public void YellowMethod(int id, Task task)
        {
            BooleanParameters.BigUglyMethod(id, task);
        }

        public void RedMethod(int id, Task task)
        {
            BooleanParameters.BigUglyMethod(id, task);
        }
    }

    public class MyService
    {
        public void UseCase323(int id, Task task)
        {
            // TODO: The shared called method must execute logic specific for my use-case #323
            BooleanParameters.BigUglyMethod(id, task, true);
        }
    }

    
    public static class BooleanParameters
    {
        // ⚠️ has multiple callers
        public static void BigUglyMethod(int storeId, Task task, bool cr323=false) // keep few bools
        {
            Console.WriteLine("Donkey Logic 1 " + task + " and " + storeId);
            Console.WriteLine(task);
            Console.WriteLine("Donkey Logic 3 " + task);

            if (cr323)
                Console.WriteLine("Logic just for CR#323 : " + task);

            Console.WriteLine("Sheep Logic 1 " + storeId);
            Console.WriteLine("Sheep Logic 2 ");
            Console.WriteLine("Sheep Logic 3 ");
        }
    }

    // Dummy Task class placeholder
    public class Task
    {
        public override string ToString()
        {
            return "TaskInstance";
        }
    }
}


