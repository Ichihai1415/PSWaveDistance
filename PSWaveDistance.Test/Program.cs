using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class Program
{
    static readonly Random r = new();
    static readonly PSWaveDistance.PSWaveDistance psd = new();

    private static void Main(string[] args)
    {

        for (int i = 0; i < 30; i++)
        {
            var d = r.Next(300);
            var s = r.NextDouble() * 100;
            var (PWaveDistance, SWaveDistance) = psd.GetDistances(d, s, double.NaN);
            Console.WriteLine($"d={d} s={s}  ->  p:{PWaveDistance} s:{SWaveDistance}");
        }
        //var summary = BenchmarkRunner.Run<Benchmark>();
    }

    [ShortRunJob]
    public class Benchmark
    {
        [Benchmark]
        public void GetDist()
        {
            var d = r.Next(300);
            var s = r.NextDouble() * 100;
            var ps = psd.GetDistances(d, s, double.NaN);
        }
    }
}
