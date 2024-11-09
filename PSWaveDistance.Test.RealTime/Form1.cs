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

            N_latSta.Value = (decimal)33.5;
            N_latEnd.Value = (decimal)37.5;
            N_lonSta.Value = (decimal)135.0;
            N_lonEnd.Value = (decimal)139.0;
        }

        private void B_start_Click(object sender, EventArgs e)
        {
            B_start.Enabled = false;
            B_get.Enabled = false;
            B_stop.Enabled = true;
            Ti_proc.Enabled = true;
        }

        readonly HttpClient hc = new();

        private void B_get_Click(object sender, EventArgs e)
        {
            try
            {
                if (!C_autoGet.Checked)
                {
                    B_start.Enabled = false;
                    B_get.Enabled = false;
                    B_stop.Enabled = true;
                }

                var jsonSt = hc.GetAsync($"http://www.kmoni.bosai.go.jp/webservice/hypo/eew/{DateTime.Now - TimeSpan.FromSeconds(2):yyyyMMddHHmmss}.json").Result.Content.ReadAsStringAsync().Result;
                //var jsonSt = hc.GetAsync("http://www.kmoni.bosai.go.jp/webservice/hypo/eew/20210213231450.json").Result.Content.ReadAsStringAsync().Result;
                var json = JsonNode.Parse(jsonSt);

                var message = json!["result"]!["message"]!.ToString();
                if (message != "")
                {
                    L_message.Text = message;
                    //P_image.BackgroundImage = null;
                    if (!C_autoGet.Checked)
                    {
                        B_start.Enabled = true;
                        B_get.Enabled = true;
                        B_stop.Enabled = false;
                    }
                    return;
                }

                N_lat.Value = decimal.Parse(json!["latitude"]!.ToString());
                N_lon.Value = decimal.Parse(json!["longitude"]!.ToString());
                N_dep.Value = decimal.Parse(json!["depth"]!.ToString().Replace("km", ""));
                T_time.Text = DateTime.ParseExact(json!["origin_time"]!.ToString(), "yyyyMMddHHmmss", CultureInfo.CurrentCulture).ToString("yyyy/MM/dd HH:mm:ss.f");

                L_message.Text = json!["region_name"]!.ToString() + "  M" + json!["magunitude"]!.ToString() + "  #" + json!["report_num"]!.ToString() + "  EventID:" + json!["report_id"]!.ToString();
                Ti_proc.Enabled = true;
            }
            catch (Exception ex)
            {
                L_message.Text = ex.Message;
            }
        }

        private void B_stop_Click(object sender, EventArgs e)
        {
            B_start.Enabled = true;
            B_get.Enabled = true;
            B_stop.Enabled = false;
            Ti_proc.Enabled = false;
        }

        private readonly PSDistances psd = new();

        private void Ti_proc_Tick(object sender, EventArgs e)
        {
            if (C_LockLatLon.Checked)
                mapImg ??= Draw_Map();
            else
                mapImg = Draw_Map();

            using var img = (Bitmap)mapImg.Clone();
            using var g = Graphics.FromImage(img);
            var drawTime = DateTime.Now;
            var originTime = DateTime.Parse(T_time.Text);

            double hypoLat = (double)N_lat.Value, hypoLon = (double)N_lon.Value, depth = (double)N_dep.Value;
            var zoomW = mapSize / (lonEnd - lonSta);
            var zoomH = mapSize / (latEnd - latSta);

            var seconds = (drawTime - originTime).TotalSeconds;
            if (seconds > 480 && C_autoGet.Checked)
            {
                Ti_proc.Enabled = false;
                P_image.BackgroundImage = null;
                return;
            }
            if (seconds > 0)
            {
                var (pLatLon, sLatLon) = psd!.GetLatLonList(depth, seconds, hypoLat, hypoLon, 45);
                if (pLatLon.Count > 2)//��{45�A���s��0��1
                {
                    var pPts = pLatLon.Select(x => new Point((int)((x.Lon - lonSta) * zoomW), (int)((latEnd - x.Lat) * zoomH))).ToList()!;
                    pPts.Add(pPts[0]);
                    g.DrawPolygon(new Pen(Color.FromArgb(64, 64, 255), 2), pPts.ToArray());
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

        readonly JsonNode mapjson;

        double latSta = 20;
        double latEnd = 50;
        double lonSta = 120;
        double lonEnd = 150;
        readonly int mapSize = 1080;

#pragma warning disable CS8602 // null �Q�Ƃ̉\����������̂̋t�Q�Ƃł��B
#pragma warning disable CS8604 // Null �Q�ƈ����̉\��������܂��B
        /// <summary>
        /// �n�}��`�悵�܂��B�ܓx�o�x���w�肷�邱�Ƃ��ł��܂�(�w�肵�Ȃ��ꍇ�ݒ���Q�Ƃ��܂�)�B
        /// </summary>
        /// <remarks>���O�ɕ␳���Ă��������B</remarks>
        /// <param name="latSta">�ܓx�̎n�_</param>
        /// <param name="latEnd">�ܓx�̏I�_</param>
        /// <param name="lonSta">�o�x�̎n�_</param>
        /// <param name="lonEnd">�o�x�̏I�_</param>
        /// <returns>�`�悵���n�}(�E����񕔕��͂��̂܂�)</returns>
        public Bitmap Draw_Map()
        {
            latSta = (double)N_latSta.Value;
            latEnd = (double)N_latEnd.Value;
            lonSta = (double)N_lonSta.Value;
            lonEnd = (double)N_lonEnd.Value;

            using var mapImg = new Bitmap(mapSize, mapSize);
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
            //var mdSize = g.MeasureString("�n�}�f�[�^:�C�ے�", new Font(font, config_map.MapSize / 28, GraphicsUnit.Pixel));
            //g.DrawString("�n�}�f�[�^:�C�ے�", new Font(font, config_map.MapSize / 28, GraphicsUnit.Pixel), new SolidBrush(config_color.Text), config_map.MapSize - mdSize.Width, config_map.MapSize - mdSize.Height);
            return mapImg;
        }

#pragma warning restore CS8602 // null �Q�Ƃ̉\����������̂̋t�Q�Ƃł��B
#pragma warning restore CS8604 // Null �Q�ƈ����̉\��������܂��B

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

        private void C_autoGet_CheckedChanged(object sender, EventArgs e)
        {
            B_start.Enabled = !C_autoGet.Checked;
            B_get.Enabled = !C_autoGet.Checked;
            B_stop.Enabled = !C_autoGet.Checked;

            T_time.Enabled = !C_autoGet.Checked;
            N_lat.Enabled = !C_autoGet.Checked;
            N_lon.Enabled = !C_autoGet.Checked;
            N_dep.Enabled = !C_autoGet.Checked;

            B_get_Click(sender, e);
            Ti_autoGet.Enabled = C_autoGet.Checked;
        }

        private void Ti_autoGet_Tick(object sender, EventArgs e)
        {
            B_get_Click(sender, e);
            GC.Collect();
        }

        private void N_tick_ValueChanged(object sender, EventArgs e)
        {
            Ti_proc.Interval = (int)(N_tick.Value * 1000);
        }
    }
}
