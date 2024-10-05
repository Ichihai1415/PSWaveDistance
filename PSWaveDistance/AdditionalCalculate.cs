using System;
using System.Collections.Generic;

namespace PSWaveDistance
{
    /// <summary>
    /// 補助計算クラス
    /// </summary>
    /// <remarks>こちらは<c>public static</c>なので自由に使用できます。</remarks>
    public static class AdditionalCalculate
    {
        /// <summary>
        /// 計算補助データ/メソッドのクラス
        /// </summary>
        public class CalAids
        {
            /// <summary>
            /// 楕円体モデル
            /// </summary>
            public enum EarthEllipsoid
            {
                /// <summary>
                /// GRS80
                /// </summary>
                GRS80 = 1,
                /// <summary>
                /// 
                /// </summary>
                WGS84 = 2
            }

            /// <summary>
            /// 度数法から弧度法に変換します。
            /// </summary>
            /// <param name="degrees">角度(度)</param>
            /// <returns>弧度法で表された<paramref name="degrees"/></returns>
            public static double ToRadians(double degrees)
            {
                return degrees * Math.PI / 180d;
            }

            /// <summary>
            /// 弧度法から度数法に変換します。
            /// </summary>
            /// <param name="radians">角度(ラジアン)</param>
            /// <returns>度数法で表された<paramref name="radians"/></returns>
            public static double ToDegrees(double radians)
            {
                return radians * 180d / Math.PI;
            }
        }

        /// <summary>
        /// 楕円体別の長軸半径と扁平率
        /// </summary>
        /// <remarks>[ID, (長軸半径, 扁平率)]</remarks>
        public static readonly Dictionary<int, (double, double)> EarthEllipsoidModels = new Dictionary<int, (double, double)>
        {
            { (int)CalAids.EarthEllipsoid.GRS80, (6378137, 1 / 298.257222101) },
            { (int)CalAids.EarthEllipsoid.WGS84, (6378137, 1 / 298.257223563) }
        };

        /// <summary>
        /// 反復計算の上限回数
        /// </summary>
        public const int ITERATION_LIMIT = 1000;

        /// <summary>
        /// 収束値
        /// </summary>
        public const double CONVERGENCE_MAX= 1e-12;

        /// <summary>
        /// Vincenty法(順解法)を用いて緯度経度を求めます。
        /// </summary>
        /// <param name="firstLat">始点の緯度</param>
        /// <param name="firstLon">始点の経度</param>
        /// <param name="azimuth">方位角</param>
        /// <param name="distance">距離</param>
        /// <param name="ellipsoidID"><see cref="CalAids.EarthEllipsoid"/>で定義されているID</param>
        /// <returns>(緯度, 経度, 方位角)</returns>
        /// <remarks><see href="https://qiita.com/r-fuji/items/5eefb451cf7113f1e51b"/>を参考</remarks>
        public static (double Lat, double Lon, double Azimuth)? GetLatLonFromDistance(double firstLat, double firstLon, double azimuth, double distance, int ellipsoidID = 1)
        {
            var (a, f) = EarthEllipsoidModels[ellipsoidID];
            var b = (1d - f) * a;

            var phi1 = CalAids.ToRadians(firstLat);
            var lambda1 = CalAids.ToRadians(firstLon);
            var alpha1 = CalAids.ToRadians(azimuth);
            var s = distance;

            var sinAlpha1 = Math.Sin(alpha1);
            var cosAlpha1 = Math.Cos(alpha1);
            var U1 = Math.Atan((1d - f) * Math.Tan(phi1));
            var sinU1 = Math.Sin(U1);
            var cosU1 = Math.Cos(U1);
            var tanU1 = Math.Tan(U1);
            var sigma1 = Math.Atan2(tanU1, cosAlpha1);

            var sinAlpha = cosU1 * sinAlpha1;
            var cos2Alpha = 1d - sinAlpha * sinAlpha;
            var u2 = cos2Alpha * (a * a - b * b) / (b * b);
            var A = 1d + u2 / 16384d * (4096d + u2 * (-768d + u2 * (320d - 175d * u2)));
            var B = u2 / 1024d * (256d + u2 * (-128d + u2 * (74d - 47d * u2)));
            var sigma = s / (b * A);

            var cos2SigmaM = double.NaN;
            var sinSigma = double.NaN;
            var cosSigma = double.NaN;
            for (int i = 0; i < ITERATION_LIMIT; i++)
            {
                cos2SigmaM = Math.Cos(2d * sigma1 + sigma);
                sinSigma = Math.Sin(sigma);
                cosSigma = Math.Cos(sigma);
                var deltaSigma = B * sinSigma * (cos2SigmaM + B / 4d * (cosSigma * (-1d + 2d * cos2SigmaM * cos2SigmaM) - B / 6d * cos2SigmaM * (-3d + 4d * sinSigma * sinSigma) * (-3d + 4d * cos2SigmaM * cos2SigmaM)));
                var sigmaDash = sigma;
                sigma = s / (b * A) + deltaSigma;

                if (Math.Abs(sigma - sigmaDash) <= CONVERGENCE_MAX)
                    break;
            }

            var x = sinU1 * sinSigma - cosU1 * cosSigma * cosAlpha1;
            var phi2 = Math.Atan2(sinU1 * cosSigma + cosU1 * sinSigma * cosAlpha1, (1d - f) * Math.Sqrt(sinAlpha * sinAlpha + x * x));
            var lambda = Math.Atan2(sinSigma * sinAlpha1, cosU1 * cosSigma - sinU1 * sinSigma * cosAlpha1);
            var C = f / 16d * cos2Alpha * (4d + f * (4d - 3d * cos2Alpha));
            var L = lambda - (1d - C) * f * sinAlpha * (sigma + C * sinSigma * (cos2SigmaM + C * cosSigma * (-1d + 2d * cos2SigmaM * cos2SigmaM)));
            var lambda2 = L + lambda1;
            var alpha2 = Math.Atan2(sinAlpha, -x) + Math.PI;

            return (CalAids.ToDegrees(phi2), CalAids.ToDegrees(lambda2), CalAids.ToDegrees(alpha2));
        }

        /// <summary>
        /// 始点を中心とする等距離の<paramref name="degreeDivide"/>角形(等距離の円に内接)の緯度経度のリストを求めます。始点を重複させる必要がある場合(最後にも必要なとき)<c>list.Add(list.First());</c>などで手動で追加してください。距離が0の場合は始点のみ(Length=1)のリストを返します。
        /// </summary>
        /// <param name="firstLat">始点の緯度</param>
        /// <param name="firstLon">始点の経度</param>
        /// <param name="distance">距離</param>
        /// <param name="degreeDivide">360度の分割回数(=n角形)</param>
        /// <param name="ellipsoidID"><see cref="CalAids.EarthEllipsoid"/>で定義されているID</param>
        /// <returns>(緯度, 経度, 方位角)</returns>
        /// <remarks>詳細は<see cref="GetLatLonFromDistance"/></remarks>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static List<(double Lat, double Lon)> GetLatLonList(double firstLat, double firstLon, int degreeDivide, double distance, int ellipsoidID = 1)
        {
            var latLonList = new List<(double Lat, double Lon)>();
            if (distance < 0)
                throw new ArgumentException("距離は0以上である必要があります。", nameof(distance));
            else if (distance == 0)
            {
                latLonList.Add((firstLat, firstLon));
                return latLonList;
            }

            var dDivide = 360d / degreeDivide;
            for (int i = 0; i < degreeDivide; i++)
            {
                var degree = dDivide * i;
                var (lat, lon, _) = GetLatLonFromDistance(firstLat, firstLon, degree, distance, ellipsoidID) ?? throw new Exception("計算に失敗しています。");
                latLonList.Add((lat, lon));
            }
            return latLonList;
        }
    }
}
