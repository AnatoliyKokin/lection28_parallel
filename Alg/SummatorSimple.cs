namespace Alg;

public class SummatorSimple : ISummator
{
    public long Sum(int[] arr)
    {
        long sum = 0;
        foreach (int x in arr)
        {
            sum += x;
        }
        return sum;
    }
}