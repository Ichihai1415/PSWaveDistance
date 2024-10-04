using System.Collections.Generic;

namespace PSWaveDistance
{
    /// <summary>
    /// 内部用クラス
    /// </summary>
    internal class PWD_internals
    {
        /// <summary>
        /// 深さ・発生からの秒数とP波・S波の到達距離の対応データのクラス
        /// </summary>
        public class TimePSDists
        {
            /// <summary>
            /// 深さ(tjma2021走時表のあるもの)
            /// </summary>
            public int Depth { get; set; }

            /// <summary>
            /// 秒数・P波・S波の到達距離のリスト
            /// </summary>
            public List<TimeData_> TimeData { get; set; } = new List<TimeData_>();


            /// <summary>
            /// 秒数、P波・S波の到達距離のクラス
            /// </summary>
            public class TimeData_
            {
                /// <summary>
                /// 秒数
                /// </summary>
                public double Seconds { get; set; }

                /// <summary>
                /// P波の到達距離(km)
                /// </summary>
                public double PDist { get; set; }

                /// <summary>
                /// S波の到達距離(km)
                /// </summary>
                public double SDist { get; set; }
            }


        }
    }

}
