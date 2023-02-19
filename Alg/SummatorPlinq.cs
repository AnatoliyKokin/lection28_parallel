namespace Alg;

public class SummatorPlinq : ISummator
{
    public long Sum(int[] arr)
    {
        return arr.Select(i => (long)i).AsParallel().Sum();
    }
}