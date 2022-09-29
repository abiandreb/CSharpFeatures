/*Delegate is an object that knows how to call some method. Type of delegate defines type of method which instances of particular delegate are able to call. 
It defines return type and parameters type of the method
*/

//Basic example
DelegateSqrt d = Square; // assign method to delegate
int result = d(2); // Call delegate
Console.WriteLine(result);

int Square(int x) => x * x; //Original method

delegate int DelegateSqrt(int x); // Declare delegate that takes method which returns int and take int as a parameter
