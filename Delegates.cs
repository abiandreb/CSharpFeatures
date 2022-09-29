/*Delegate is an object that knows how to call some method. Type of delegate defines type of method which instances of particular delegate are able to call. 
It defines return type and parameters type of the method
*/

//Basic example
DelegateSqrt d = Square; // assign method to delegate
int result = d(2); // Call delegate
Console.WriteLine(result);

int Square(int x) => x * x; //Original method

delegate int DelegateSqrt(int x); // Declare delegate that takes method which returns int and take int as a parameter

// Write plug-in method for delegates 

int[] values = {1, 2, 3};

Transform(values, Square); //Call service method and pass Square as a delegate parameter
Transform(values, Qube);

void Transform(int[] values, Transformer t){ //service method that takes values and delegate and dynamicaly assign method
  for(int i = 0; i < values.Length; i++){
    values[i] = t(values[i]);
  }
}

int Square(int x) => x * x; //Initial methods
int Qube(int x) => x * x * x;
 
delegate int Transformer(int x); //Delegate
