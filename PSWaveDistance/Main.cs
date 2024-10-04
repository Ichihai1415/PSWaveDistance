using PSWaveDistance.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace PSWaveDistance
{
    /// <summary>
    /// インスタンスを作成して読み込んでください(オブジェクト参照が必要です)。
    /// </summary>
    public class PSWaveDistance
    {
        internal PWD_internals.TimePSDists[]? timePSDists;

        List<int> depthList;

        public PSWaveDistance()
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
        /// <returns><code>PWaveDistance</code>:P波到達距離 <code>PWaveDistance</code>:S波到達距離</returns>
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
    }
}
