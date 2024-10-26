using BenchmarkDotNet.Attributes;

public class Program
{
    static readonly Random r = new();
    static readonly PSWaveDistance.PSDistances psd = new();

    private static void Main(string[] args)
    {

        Console.WriteLine("time,pWave,sWave");
        for (int i = 0; i < 1000; i++)
        {
            var (PWaveDistance, SWaveDistance) = psd.GetDistances(10, i * 0.1, 0);
            Console.WriteLine($"{i * 0.1},{PWaveDistance},{SWaveDistance}");
        }


        /*
        for (int i = 0; i < 1; i++)
        {
            var d = r.Next(300);
            var s = r.NextDouble() * 100;
            var (PWaveDistance, SWaveDistance) = psd.GetDistances(d, s, double.NaN);
            Console.WriteLine($"d={d} s={s}  ->  p:{PWaveDistance} s:{SWaveDistance}");
        }
        Console.WriteLine();*/

        /*
        var (pWaveLatLonList, sWaveLatLonList) = psd.GetLatLonList(10, 50, 35.2, 136.2, 360);
        foreach(var (lat, lon) in pWaveLatLonList)
        {
            Console.WriteLine(lat + "," + lon);
        }*/

        /*
        var b = new Benchmark();
        for (int i = 0; i < 30; i++)
        {
            b.GetLatLonList360();
        }
        return;*/

        //var summary = BenchmarkRunner.Run<Benchmark>();
    }

    /// <summary>
    /// ベンチマーク関連クラス
    /// </summary>
    [ShortRunJob]
    public class Benchmark
    {
        [Benchmark]
        public (double, double, double, double) GetRandom()
        {
            var d = r.NextDouble() * 600;
            var s = r.NextDouble() * 300;
            var la = 20 + r.NextDouble() * 50;
            var lo = 120 + r.NextDouble() * 50;
            return (d, s, la, lo);
        }

        [Benchmark]
        public string GetDist()
        {
            var (d, s, _, _) = GetRandom();
            var ps = psd.GetDistances(d, s, double.NaN);
            return ps.ToString();
        }

        [Benchmark]
        public string GetLatLonList360()
        {
            var (d, s, la, lo) = GetRandom();
            var a = psd.GetLatLonList(d, s, la, lo, 360);
            return a.ToString();
        }
    }
}
