using Unni.Infrastructure;

namespace Unni.Samples.ConsoleApp
{
    public static class Program
    {
        public static void Main()
        {
            //Testing validation errors
            ExecuteResut(0);

            //Testing inner exception
            ExecuteResut(-1);

            //Testing successful method execution
            ExecuteResut(5);
        }

        public static void ExecuteResut(int count)
        {
            Console.WriteLine($"Executing method with count {count}");
            var result = FillArray(count);
            if (result.Success)
            {
                for (int i = 0; i < result.Data.Count; i++)
                {
                    Console.WriteLine(result.Data[i]);
                }
            }
            else
            {
                ShowErrorResult(result);
            }
        }

        public static UnniResult<List<int>> FillArray(int count)
        {
            try
            {
                if (count == 0)
                {
                    return new UnniResult<List<int>>("Count can not be 0");
                }
                List<int> result = new List<int>(count);
                for (int i = 1; i <= count; i++)
                {
                    result.Add(i);
                }
                return new UnniResult<List<int>>(result);
            }
            catch(Exception ex)
            {
                return new UnniResult<List<int>>(ex);
            }
        }

        public static void ShowErrorResult(UnniResult result)
        {
            if (result.InnerException != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Oops, something went wrong!"); //Tell user about unexpected error
                Console.WriteLine($"Exception occured: {result.InnerException.Message}"); //Log exception to proceed it in the future
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error); //Tell user about validation error
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
