
bool entered = false;
int testArraySize = 0;
while (!entered)
{
    Console.WriteLine("Enter array size:");

    entered = int.TryParse(Console.ReadLine(), out testArraySize);

    if (!entered)
    {
        Console.WriteLine("wrong array size");
    }
}


int[] testArr = GenerateArr(testArraySize);


Alg.ISummator[] methods = new Alg.ISummator[3]{ new Alg.SummatorSimple(), new Alg.SummatorParallel(), new Alg.SummatorPlinq() };

foreach(var method in methods)
{
    long result = 0;
    double msec = MeasureMsec((m,a) => { return m.Sum(a); }, method, testArr, out result);
    Console.WriteLine((method.ToString() ?? "")+" result= "+result+" time= "+msec+" msec");
}



static int[] GenerateArr(int count)
{
    int[] arr = new int[count];

    Random r = new Random();

    for (int i = 0; i < count; i++)
    {
        arr[i] = r.Next(-1000000,1000000);
    }

    return arr;

}

static double MeasureMsec(Func<Alg.ISummator, int[],long> testAction, Alg.ISummator summator, int[] arr, out long result)
{
    DateTime start = DateTime.Now;
    result = testAction(summator, arr);
    DateTime end = DateTime.Now;

    TimeSpan ts = end - start;

    return ts.TotalMilliseconds;
}
