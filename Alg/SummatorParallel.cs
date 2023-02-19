namespace Alg;

public class SummatorParallel : ISummator
{
    private int mThreadCount = 0;

    public SummatorParallel()
    {

    }

    public SummatorParallel(int threadCount)
    {
        mThreadCount = threadCount;
    }
    public long Sum(int[] arr)
    {
        //если не задано количество потоков
        if (mThreadCount == 0)
        {
            mThreadCount = Environment.ProcessorCount;
        }

        // оптимизация количества потоков
        if (arr.Length < mThreadCount * 2)
        {
            mThreadCount = arr.Length / 2;
        }

        if (mThreadCount < 2)
        {
            return arr.Sum();
        }

        List<Thread> threads = new List<Thread>();
        List<long> sums = new List<long>();
        int offset = 0;
        int itemsPerThread = arr.Length / mThreadCount;
        for (int i = 0; i < mThreadCount - 1; i++)
        {
            sums.Add(0);
            var summator = new SummatorImpl(arr, offset, itemsPerThread, sums, sums.Count - 1);
            Thread t = new Thread(new ThreadStart(summator.Sum));
            threads.Add(t);
            offset += itemsPerThread;
        }

        {
            sums.Add(0);
            var summator = new SummatorImpl(arr, offset, arr.Length - offset, sums, sums.Count - 1);
            Thread t = new Thread(new ThreadStart(summator.Sum));
            threads.Add(t);
        }


        foreach(var th in threads)
        {
            th.Start();
        }

        foreach(var th in threads)
        {
            th.Join();
        }

        return sums.Sum();

    }

    private class SummatorImpl
    {
        private int mOffset = 0;
        private int mCount = 0;
        private int[] mArr;
        private readonly IList<long> mOutList;
        private int mOutIndex = 0;
        public SummatorImpl(int[] arr, int offset, int count, IList<long> outputList, int outIndex)
        {
            mOffset = offset;
            mCount = count;
            mArr = arr;
            mOutList = outputList;
            mOutIndex = outIndex;
        }

        public void Sum()
        {
            long sum = 0;
            for (int i = mOffset; i < mOffset + mCount; i++)
            {
                sum += mArr[i];
            }
            mOutList[mOutIndex] = sum;
        }
    }

}