/*Delegate is an object that knows how to call some method. Type of delegate defines type of method which instances of particular delegate are able to call. 
It defines return type and parameters type of the method
*/

/***************************************************************************/

//Basic example
DelegateSqrt d = Square; // assign method to delegate
int result = d(2); // Call delegate
Console.WriteLine(result);

int Square(int x) => x * x; //Original method

delegate int DelegateSqrt(int x); // Declare delegate that takes method which returns int and take int as a parameter

/***************************************************************************/

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

/***************************************************************************/

// Target methods of delegate instance, static target methods
Transformer t = Test.Square;
Console.WriteLine (t(10));      // 100

// Target methods of delegate instance, class member target methods

Test test = new Test();
Transformer t = test.Qube;
Console.WriteLine (t(10));

class Test
{
	public static int Square(int x) => x * x;
  public int Qube(int x ) => x * x * x;
}

delegate int Transformer (int x);

// When a delegate object is assigned to an instance method, the delegate object must maintain
// a reference not only to the method, but also to the instance to which the method belongs:

MyReporter r = new MyReporter();
r.Prefix = "%Complete: ";
ProgressReporter p = r.ReportProgress;
p(99);                                 // 99
Console.WriteLine (p.Target == r);     // True
Console.WriteLine (p.Method);          // Void InstanceProgress(Int32)
r.Prefix = "";
p(99);                                 // 99

public delegate void ProgressReporter (int percentComplete);

class MyReporter
{
	public string Prefix = "";
	public void ReportProgress (int percentComplete) => Console.WriteLine (Prefix + percentComplete);
}

/***************************************************************************/

//Multicast Delegates

// All delegate instances have multicast capability:

SomeDelegate d = SomeMethod1;
d += SomeMethod2;

d();
" -- SomeMethod1 and SomeMethod2 both fired\r\n".Dump();

d -= SomeMethod1;
d();
" -- Only SomeMethod2 fired".Dump();

void SomeMethod1 () => "SomeMethod1".Dump();
void SomeMethod2 () => "SomeMethod2".Dump();

delegate void SomeDelegate();

/***************************************************************************/

//Generic Types Delegates 

int[] values = {1, 2, 3,};

Transformer(values, Square);
Transformer(values, Qube);

int Square(int x ) => x * x;
int Qube(int x) => x * x * x;

void Transformer<T>(T[] values, Transform<T> t){
	for(int i = 0; i < values.Length; i++){
		values[i] = t(values[i]);
	}
}

delegate T Transform<T> (T arg);

/***************************************************************************/

//Func Action Delegates

int[] values = {1, 2, 3};

Utils.Transformer(values, Square);
Utils.Transformer(values, Qube);

int Square(int x ) => x * x;
void Qube(int x) => Console.WriteLine(x * x * x);

class Utils 
{
	public static void Transformer<T>(T[] args, Func<T, T> t){
		for(int i = 0; i < args.Length; i++){
			args[i] = t(args[i]);
		}
	}
	
	public static void Transformer<T>(T[] args, Action<T> t){ //Action does not return value
		for(int i = 0; i < args.Length; i++){
			t(args[i]);
		}
	}
}

/***************************************************************************/

//Delegates vs Interfaces

// A problem that can be solved with a delegate can also be solved with an interface:

int[] values = { 1, 2, 3 };
Util.TransformAll (values, new Squarer());
values.Dump();

public interface ITransformer
{
	int Transform (int x);
}

public class Util
{
	public static void TransformAll (int[] values, ITransformer t)
	{
		for (int i = 0; i < values.Length; i++)
			values[i] = t.Transform (values[i]);
	}
}

class Squarer : ITransformer
{
	public int Transform (int x) => x * x;
}

// With interfaces, we’re forced into writing a separate type per transform
// since Test can only implement ITransformer once:

int[] values = { 1, 2, 3 };
Util.TransformAll (values, new Cuber());
values.Dump();

public interface ITransformer
{
	int Transform (int x);
}

public class Util
{
	public static void TransformAll (int[] values, ITransformer t)
	{
		for (int i = 0; i < values.Length; i++)
			values[i] = t.Transform (values[i]);
	}
}

class Squarer : ITransformer
{
	public int Transform (int x) => x * x;
}

class Cuber : ITransformer
{
	public int Transform (int x) => x * x * x;
}

/***************************************************************************/

//Delegates types incompatability

// Delegate types are all incompatible with each other, even if their signatures are the same:

D1 d1 = Method1;
D2 d2 = d1;            // Compile-time error
D2 d2 = new D2 (d1);	 // Legal

static void Method1() { }

delegate void D1();
delegate void D2();


/***************************************************************************/

//Delegates equality

// Delegate instances are considered equal if they have the same method targets:

D d1 = Method1;
D d2 = Method1;
Console.WriteLine (d1 == d2);         // True

static void Method1() { }

delegate void D();

/***************************************************************************/

//Delegates parameter compatibility (Contravariance)

// A delegate can have more specific parameter types than its method target. This is called contravariance:

delegate void StringAction (string s);

static void Main()
{
	StringAction sa = new StringAction (ActOnObject);
	sa ("hello");
}

static void ActOnObject (object o) => Console.WriteLine (o);   // hello

/***************************************************************************/

//Delegates return type compatibility (Covariance)

// A delegate can have more specific parameter types than its method target. This is called contravariance:

ObjectRetriever o = new ObjectRetriever (RetriveString);
object result = o();
Console.WriteLine (result);      // hello

string RetriveString() => "hello";

delegate object ObjectRetriever();

/***************************************************************************/

//Delegates type parameter variance

/* From C# 4.0, type parameters on generic delegates can be marked as covariant (out) or contravariant (in).

For instance, the System.Func delegate in the Framework is defined as follows:

	public delegate TResult Func<out TResult>();

This makes the following legal:  */

Func<string> x = () => "Hello, world";
Func<object> y = x;

/* The System.Action delegate is defined as follows:

	void Action<in T> (T arg);

This makes the following legal:  */

Action<object> x2 = o => Console.WriteLine (o);
Action<string> y2 = x2;
