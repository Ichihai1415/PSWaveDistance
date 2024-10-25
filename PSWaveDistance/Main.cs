﻿using PSWaveDistance.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PSWaveDistance
{
    /// <summary>
    /// PSWaveDistanceのメインクラスです。
    /// </summary>
    /// <remarks>インスタンスを作成して読み込んでください(オブジェクト参照が必要です)。</remarks>
    public class PSDistances
    {
        /// <summary>
        /// 走時表変換データ
        /// </summary>
        internal PWD_internals.TimePSDists[]? timePSDists;

        /// <summary>
        /// 走時表データの深さのリスト
        /// </summary>
        readonly List<int> depthList;

        /// <summary>
        /// <see cref="PSDistances"/>を初期化します。
        /// </summary>
        /// <exception cref="Exception"></exception>
        public PSDistances()
        {
            timePSDists = JsonSerializer.Deserialize<PWD_internals.TimePSDists[]>(Resources.tjma2001_sec2dist);
            if (timePSDists == null)
                throw new Exception("走時表変換データの読み込みに失敗しました。");
            depthList = timePSDists.Select(x => x.Depth).ToList();
        }

        /// <summary>
        /// 深さ・発生からの秒数からP波・S波の到達距離を求めます。
        /// </summary>
        /// <param name="depth">深さ(走時表にある最も近い値を参照します)</param>
        /// <param name="seconds">発生からの秒数(小数第二位で四捨五入されます)</param>
        /// <param name="errorReplace">範囲外等で取得できなかったときに</param>
        /// <returns><c>PWaveDistance</c> : P波到達距離<br/><c>PWaveDistance</c> : S波到達距離</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public (double PWaveDistance, double SWaveDistance) GetDistances(double depth, double seconds, double errorReplace = -1)
        {
            if (seconds < 0)
                throw new ArgumentException("秒数は0以上である必要があります。", nameof(seconds));
            if (timePSDists == null)
                throw new Exception("走時表変換データが読み込まれていません。");

            depthList.Add(-1000);//近い値を求めるため
            depthList.Add(1000);
            depthList.Sort();//-1000,0,...,700,1000

            var cDepth = -1;//参照する深さの値

            for (int i = 1; i < depthList.Count; i++)
                if (depthList[i] >= depth)//初めて depth以上
                {
                    var depth_nearL_diff = depth - depthList[i - 1];//depth以上で最小のものとの差
                    var depth_nearH_diff = depthList[i] - depth;    //depth以下で最大のものとの差
                    cDepth = depth_nearL_diff > depth_nearH_diff ? depthList[i] : depthList[i - 1];//Hのほうが近ければHを参照
                    break;
                }
            if (cDepth == -1)
                throw new Exception("参照用の深さの算出に失敗しました。");

            seconds = Math.Round(seconds, 1, MidpointRounding.AwayFromZero);

            var depthData = timePSDists.FirstOrDefault(d => d.Depth == cDepth) ?? throw new Exception("深さに対応するデータが見つかりませんでした。");
            var timeData = depthData.TimeData.FirstOrDefault(t => t.Seconds == seconds);
            if (timeData == null)
                return (errorReplace, errorReplace);
            return (timeData.PDist != -1 ? timeData.PDist : errorReplace, timeData.SDist != -1 ? timeData.SDist : errorReplace);
        }

        /// <summary>
        /// 深さ・発生からの秒数からP波・S波の到達した緯度経度のリストを求めます。始点を重複させる必要がある場合(最後にも必要なとき)<c>list.Add(list.First());</c>などで手動で追加してください。リストは距離が0の場合、始点のみ(Length=1)になります。
        /// </summary>
        /// <param name="depth">深さ(走時表にある最も近い値を参照します)</param>
        /// <param name="seconds">発生からの秒数(小数第二位で四捨五入されます)</param>
        /// <param name="firstLat">始点の緯度</param>
        /// <param name="firstLon">始点の経度</param>
        /// <param name="degreeDivide">360度の分割回数(=n角形)</param>
        /// <param name="ellipsoidID"><see cref="AdditionalCalculate.CalAids.EarthEllipsoid"/>で定義されているID</param>
        /// <returns>(緯度, 経度, 方位角)</returns>
        /// <returns>[List[緯度, 経度], List[緯度, 経度]]</returns>
        /// <remarks>詳細は内部コードを参照してください。</remarks>
        public (List<(double Lat, double Lon)> PWaveLatLonList, List<(double Lat, double Lon)> SWaveLatLonList) GetLatLonList(double depth, double seconds, double firstLat, double firstLon, int degreeDivide, int ellipsoidID = 1)
        {
            var (pWaveDistance, sWaveDistance) = GetDistances(depth, seconds, 0);
            return (AdditionalCalculate.GetLatLonList(firstLat, firstLon, degreeDivide, pWaveDistance, ellipsoidID), AdditionalCalculate.GetLatLonList(firstLat, firstLon, degreeDivide, sWaveDistance, ellipsoidID));
        }
    }
}
