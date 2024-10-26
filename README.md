# PSWaveDistance
![Nuget](https://img.shields.io/nuget/v/PSWaveDistance)
![Nuget](https://img.shields.io/nuget/dt/PSWaveDistance)
![GitHub last commit](https://img.shields.io/github/last-commit/Ichihai1415/PSWaveDistance)
![GitHub Release Date](https://img.shields.io/github/release-date/Ichihai1415/PSWaveDistance)
![GitHub issues](https://img.shields.io/github/issues/Ichihai1415/PSWaveDistance)

走時表を変換したデータを用いて深さと経過時間からPS波の到達距離を求めます。

変換は検証用プログラム[PSCircleTest](https://github.com/Ichihai1415/PSCircleTest)を使用しました。詳細はこれも参照してください。

変換したデータは`Resources`に0.1秒ごとのJSON形式であります(サイズが大きいです)。PSCircleTestでは各深さに分かれたcsvファイルを作成できます(直接欲しければDM等で伝えてください)。

## 使用例

```cs
using PSWaveDistance;

//到達距離を求めるのみ

//データの読み込みのためインスタンスを作成する必要があります
var psd = new PSDistances();

//深さ(double):自動で近い値に調整されます
var depth = 20.2;

//経過秒数(double):小数第二位で四捨五入されます。
var seconds = 13.01;

//返り値:(P波の到達距離(double), S波の到達距離(double))のタプルです(小数第3桁まで)。
//3つ目の引数(double)は指定しなくても問題ありません。データ範囲外になったとき返す値を指定できます。既定は-1です。
var (pDist, sDist) = psd.GetDistances(depth, seconds, double.NaN);


//到達緯度と経度のリスト(n角形で近似して描画するなど用)

//始点の緯度
var firstLat = 35.2;

//始点の経度
var firstLon = 136.4;

//n角形で近似
var degreeDivide = 180;

//返り値:(P波のList<(到達緯度(double), 到達経度(double))>, S波のList<(到達緯度(double), 到達経度(double))>)のタプルのリストのタプルです。
var (pLatLon, sLatLon) = psd.GetLatLonList(depth, seconds, firstLat, firstLon, degreeDivide)
```

## Benchmark結果

BenchmarkDotNetでの検証結果(GetLatLonList360のほうが短いからおかしい感はある)
```cs
/*2024/10/05
BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4169/23H2/2023Update/SunValley3)
13th Gen Intel Core i5-13500, 1 CPU, 20 logical and 14 physical cores
.NET SDK 9.0.100-rc.1.24452.12
  [Host]   : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1
WarmupCount=3

| Method           | Mean           | Error            | StdDev        |
|----------------- |---------------:|-----------------:|--------------:|
| GetRandom        |       7.343 ns |        32.709 ns |      1.793 ns |
| GetDist          | 568,991.689 ns | 1,607,179.758 ns | 88,094.946 ns |
| GetLatLonList360 | 368,013.045 ns |   604,944.177 ns | 33,159.032 ns |

// * Legends *
  Mean   : Arithmetic mean of all measurements
  Error  : Half of 99.9% confidence interval
  StdDev : Standard deviation of all measurements
  1 ns   : 1 Nanosecond (0.000000001 sec) 
*/

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
```