Console.WriteLine("enter you name");
var name =Console.ReadLine();
Console.WriteLine("enter your age");
var age = Convert.ToInt32(Console.ReadLine());
Console.WriteLine("hello {0} you are {1} year old", name, age);
string lastname = "sherstha";
string fullname =$"full name is {name} {lastname}";
Console.WriteLine(fullname);

string[] namelist = { "sherstha", "sherpa", "shrestha" };
foreach (var item in namelist)
{
    Console.WriteLine(item);
}
for  (int i = 0; i < namelist.Length; i++)
{
    Console.WriteLine(namelist[i]);
}
for(int i = 0; i < age; i++)
{
    if(i==14)
    {
        continue;
    }
}



static void add(int a, int b)
{
    Console.WriteLine(a + b);
}

static void multiply(int a, int b)
{
    Console.WriteLine(a * b);
}
public delegate void Operation(int a, int b);
Operation operation=new Operation(add);