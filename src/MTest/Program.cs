
///FOR MORE DETAILS PLEASE LOOK AT /UnitTests/DataStructuresUnitTests.cs

var struct1 = new QuickPopDataStructure<int>();
Console.WriteLine("QuickPopDataStructure:");

struct1.Push(1);
struct1.Push(2);
struct1.Push(4);
struct1.Push(3);

Console.WriteLine(struct1.Pop());
Console.WriteLine(struct1.Pop());
Console.WriteLine(struct1.Pop());
Console.WriteLine(struct1.Pop());

struct1.Push(2);
struct1.Push(4);

Console.WriteLine(struct1.Pop());
Console.WriteLine(struct1.Pop());

var struct2 = new QuickPushDataStructure<int>();
Console.WriteLine("QuickPushDataStructure:");

struct2.Push(1);
struct2.Push(2);
struct2.Push(4);
struct2.Push(3);

Console.WriteLine(struct2.Pop());
Console.WriteLine(struct2.Pop());
Console.WriteLine(struct2.Pop());
Console.WriteLine(struct2.Pop());

struct2.Push(2);
struct2.Push(4);

Console.WriteLine(struct2.Pop());
Console.WriteLine(struct2.Pop());

Console.ReadKey();