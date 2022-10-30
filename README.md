# Unni
Open-source libraries to create multifunctional and safe structure of your project
# Unni.Infrastructure
Represents UnniResult class which allows you to easy transfer validation errors and inner exceptions between methods.
## Installation
Install NuGet package: [Unni.Infrastructure](https://www.nuget.org/packages/unni.infrastructure/)

Or with the Package Manager using the command line below:
```
dotnet add package unni.infrastructure
```
## Usage
### Imports
```c#
using Unni.Infrastructure;
```
### UnniResult
Divided on 2 classes - `UnniResult` and `UnniResult<T>`

`UnniResult` contains these properties:
| Property | Description |
| --- | --- |
| Success | Indicates if executed method was successful |
| Errors | Contains list of validation errors |
| InnerException | Contains thrown exception from method |

`UnniResult<T>` extends `UnniResult` and adds this property:
| Property | Description |
| --- | --- |
| Data<T> | Final result of executed method with type `<T>` |

## Examples
Let's create the simple console app with example of usage
1. Create a new Console project
2. Add `Unni.Infrastructure` library using NuGet
3. Add the namespace to the 'Program.cs' file
```c#
using Unni.Infrastructure;
```
4. Add the method which represents a service for getting list of numbers
```c#
public static UnniResult<List<int>> FillArray(int count)
{
    try
    {
        if (count == 0)
        {
            return new UnniResult<List<int>>("! Count can not be 0");
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
```
5. Add the method for displaying errors
```c#
public static void ShowErrorResult(UnniResult result)
{
    if (result.InnerException != null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"- Oops, something went wrong!"); //Tell user about unexpected error
        Console.WriteLine($"- Exception occured: {result.InnerException.Message}"); //Log exception to proceed it in the future
        Console.ForegroundColor = ConsoleColor.White;
    }
    Console.ForegroundColor = ConsoleColor.Yellow;
    foreach (var error in result.Errors)
    {
        Console.WriteLine(error); //Tell user about validation errors
    }
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine();
}
```
6. Add the method for executing `FillArray` method
```c#
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
```
7. Change the `Main` method to run some tests
```c#
public static void Main()
{
    //Testing validation errors
    ExecuteResut(0);

    //Testing inner exception
    ExecuteResut(-1);

    //Testing successful method execution
    ExecuteResut(5);
}
```
8. Run the project and you will get this output
```diff
Executing method with count 0
! Count can not be 0

Executing method with count -1
- Oops, something went wrong!
- Exception occured: Non-negative number required. (Parameter 'capacity')

Executing method with count 5
1
2
3
4
5
```
