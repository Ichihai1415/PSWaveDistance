using PSWaveDistance.Test.RealTime.Properties;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Text.Json.Nodes;

namespace PSWaveDistance.Test.RealTime
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            mapjson = JsonNode.Parse(Resources.AreaForecastLocalE_GIS_20240520_1)!;
            T_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.f");
        }

        private void B_start_Click(object sender, EventArgs e)
        {
            B_start.Enabled = false;
            B_get.Enabled = false;
            B_stop.Enabled = true;
            Ti_proc.Enabled = true;
        }

        HttpClient hc = new HttpClient();

        private void B_get_Click(object sender, EventArgs e)
        {
            B_start.Enabled = false;
            B_get.Enabled = false;
            B_stop.Enabled = true;
            var jsonSt = hc.GetAsync($"http://www.kmoni.bosai.go.jp/webservice/hypo/eew/{DateTime.Now - TimeSpan.FromSeconds(2):yyyyMMddHHmmss}.json").Result.Content.ReadAsStringAsync().Result;
            var json = JsonNode.Parse(jsonSt);

            var message = json["result"]["message"].ToString();
            if (message != "")
            {
                MessageBox.Show("取得失敗 : " + message, "PSWaveDistance.Test.RealTime", MessageBoxButtons.OK, MessageBoxIcon.Information);
                B_start.Enabled = true;
                B_get.Enabled = true;
                B_stop.Enabled = false;
                return;
            }

            N_lat.Value = decimal.Parse(json["latitude"].ToString());
            N_lon.Value = decimal.Parse(json["longitude"].ToString());
            N_dep.Value = decimal.Parse(json["depth"].ToString().Replace("km", ""));
            T_time.Text = DateTime.ParseExact(json["origin_time"].ToString(), "yyyyMMddHHmmss", CultureInfo.CurrentCulture).ToString("yyyy/MM/dd HH:mm:ss.f");

            Ti_proc.Enabled = true;
        }

        private void B_stop_Click(object sender, EventArgs e)
        {
            B_start.Enabled = true;
            B_get.Enabled = true;
            B_stop.Enabled = false;
            Ti_proc.Enabled = false;
        }

        PSDistances psd = new();

        private void Ti_proc_Tick(object sender, EventArgs e)
        {
            if (C_LockLatLon.Checked)
                mapImg ??= Draw_Map();
            else
                mapImg = Draw_Map();

            var img = (Bitmap)mapImg.Clone();
            using var g = Graphics.FromImage(img);
            var drawTime = DateTime.Now;
            var originTime = DateTime.Parse(T_time.Text);

            double hypoLat = (double)N_lat.Value, hypoLon = (double)N_lon.Value, depth = (double)N_dep.Value;
            var zoomW = mapSize / (lonEnd - lonSta);
            var zoomH = mapSize / (latEnd - latSta);

            var seconds = (drawTime - originTime).TotalSeconds;
            if (seconds > 0)
            {
                var (pLatLon, sLatLon) = psd!.GetLatLonList(depth, seconds, hypoLat, hypoLon, 360);
                if (pLatLon.Count > 2)//基本360、失敗時0か1
                {
                    var pPts = pLatLon.Select(x => new Point((int)((x.Lon - lonSta) * zoomW), (int)((latEnd - x.Lat) * zoomH))).ToList()!;
                    pPts.Add(pPts[0]);
                    g.DrawPolygon(new Pen(Color.Blue, 2), pPts.ToArray());
                    //if (seconds == 20)
                    //throw new Exception();
                }
                if (sLatLon.Count > 2)
                {
                    var sPts = sLatLon.Select(x => new Point((int)((x.Lon - lonSta) * zoomW), (int)((latEnd - x.Lat) * zoomH))).ToList()!;
                    sPts.Add(sPts[0]);
                    g.DrawPolygon(new Pen(Color.Red, 2), sPts.ToArray());
                    g.FillPolygon(new SolidBrush(Color.FromArgb(64, 255, 0, 0)), sPts.ToArray());
                }
                var hypoLength = 20;
                var hypoPt = new Point((int)((hypoLon - lonSta) * zoomW), (int)((latEnd - hypoLat) * zoomH));
                g.DrawLine(new Pen(Color.Red, 6), hypoPt.X - hypoLength, hypoPt.Y - hypoLength, hypoPt.X + hypoLength, hypoPt.Y + hypoLength);
                g.DrawLine(new Pen(Color.Red, 6), hypoPt.X + hypoLength, hypoPt.Y - hypoLength, hypoPt.X - hypoLength, hypoPt.Y + hypoLength);
            }

            P_image.BackgroundImage = img;
        }

        Bitmap? mapImg = null;

        JsonNode mapjson;

        double latSta = 20;
        double latEnd = 50;
        double lonSta = 120;
        double lonEnd = 150;
        int mapSize = 1080;

#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
        /// <summary>
        /// 地図を描画します。緯度経度を指定することができます(指定しない場合設定を参照します)。
        /// </summary>
        /// <remarks>事前に補正してください。</remarks>
        /// <param name="latSta">緯度の始点</param>
        /// <param name="latEnd">緯度の終点</param>
        /// <param name="lonSta">経度の始点</param>
        /// <param name="lonEnd">経度の終点</param>
        /// <returns>描画した地図(右側情報部分はそのまま)</returns>
        public Bitmap Draw_Map()
        {
            latSta = (double)N_latSta.Value;
            latEnd = (double)N_latEnd.Value;
            lonSta = (double)N_lonSta.Value;
            lonEnd = (double)N_lonEnd.Value;

            var mapImg = new Bitmap(mapSize, mapSize);
            var zoomW = mapSize / (lonEnd - lonSta);
            var zoomH = mapSize / (latEnd - latSta);
            using var g = Graphics.FromImage(mapImg);
            g.Clear(Color.Black);

            using var gPath = new GraphicsPath();
            gPath.StartFigure();
            foreach (var mapjson_feature in mapjson["features"].AsArray())
            {
                if (mapjson_feature["geometry"] == null)
                    continue;
                if ((string?)mapjson_feature["geometry"]["type"] == "Polygon")
                {
                    var points = mapjson_feature["geometry"]["coordinates"][0].AsArray().Select(mapjson_coordinate => new Point((int)(((double)mapjson_coordinate[0] - lonSta) * zoomW), (int)((latEnd - (double)mapjson_coordinate[1]) * zoomH))).ToArray();
                    if (points.Length > 2)
                        gPath.AddPolygon(points);
                }
                else
                {
                    foreach (var mapjson_coordinates in mapjson_feature["geometry"]["coordinates"].AsArray())
                    {
                        var points = mapjson_coordinates[0].AsArray().Select(mapjson_coordinate => new Point((int)(((double)mapjson_coordinate[0] - lonSta) * zoomW), (int)((latEnd - (double)mapjson_coordinate[1]) * zoomH))).ToArray();
                        if (points.Length > 2)
                            gPath.AddPolygon(points);
                    }
                }
            }
            g.DrawPath(new Pen(Color.White, mapSize / 1080f * 2), gPath);
            //var mdSize = g.MeasureString("地図データ:気象庁", new Font(font, config_map.MapSize / 28, GraphicsUnit.Pixel));
            //g.DrawString("地図データ:気象庁", new Font(font, config_map.MapSize / 28, GraphicsUnit.Pixel), new SolidBrush(config_color.Text), config_map.MapSize - mdSize.Width, config_map.MapSize - mdSize.Height);
            return mapImg;
        }

        private void B_zoomUp_Click(object sender, EventArgs e)
        {
            var d = (decimal)0.5;
            N_latSta.Value += d;
            N_latEnd.Value -= d;
            N_lonSta.Value += d;
            N_lonEnd.Value -= d;
        }

        private void B_zoomDown_Click(object sender, EventArgs e)
        {
            var d = (decimal)0.5;
            N_latSta.Value -= d;
            N_latEnd.Value += d;
            N_lonSta.Value -= d;
            N_lonEnd.Value += d;
        }

        private void B_moveUp_Click(object sender, EventArgs e)
        {
            var d = (decimal)0.5;
            N_latSta.Value += d;
            N_latEnd.Value += d;
        }

        private void B_moveDown_Click(object sender, EventArgs e)
        {
            var d = (decimal)0.5;
            N_latSta.Value -= d;
            N_latEnd.Value -= d;
        }

        private void B_moveLeft_Click(object sender, EventArgs e)
        {
            var d = (decimal)0.5;
            N_lonSta.Value -= d;
            N_lonEnd.Value -= d;
        }

        private void B_moveRight_Click(object sender, EventArgs e)
        {
            var d = (decimal)0.5;
            N_lonSta.Value += d;
            N_lonEnd.Value += d;
        }
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
#pragma warning restore CS8604 // Null 参照引数の可能性があります。

    }
}
