# PSWaveDistance


走時表を変換したデータを用いて深さと経過時間からPS波の到達距離を求めます。

変換は検証用プログラム[PSCircleTest](https://github.com/Ichihai1415/PSCircleTest)を使用しました。詳細はこれも参照してください。

変換したデータは`Resources`に0.1秒ごとのJSON形式であります(サイズが大きいです)。PSCircleTestでは各深さに分かれたcsvファイルを作成できます(直接欲しければ言ってください)。

## 使用例

```cs
//データの読み込みのためインスタンスを作成する必要があります
var psd = new PSWaveDistance.PSWaveDistance();

//深さ(double):自動で近い値に調整されます
var depth = 20.2;

//経過秒数(double):小数第二位で四捨五入されます。
var seconds = 13.01;

//結果:(P波の到達距離(double), S波の到達距離(double))のタプルです。
//3つ目の引数(double)は指定しなくても問題ありません。データ範囲外になったとき返す値を指定できます。既定は-1です。
var (pDist, sDist) = psd.GetDistances(depth, seconds, double.NaN);
```