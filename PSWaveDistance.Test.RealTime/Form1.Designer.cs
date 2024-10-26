namespace PSWaveDistance.Test.RealTime
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            T_time = new TextBox();
            N_lat = new NumericUpDown();
            N_lon = new NumericUpDown();
            N_dep = new NumericUpDown();
            B_start = new Button();
            B_get = new Button();
            B_stop = new Button();
            Ti_proc = new System.Windows.Forms.Timer(components);
            N_latSta = new NumericUpDown();
            N_latEnd = new NumericUpDown();
            N_lonSta = new NumericUpDown();
            N_lonEnd = new NumericUpDown();
            C_LockLatLon = new CheckBox();
            P_image = new PictureBox();
            B_zoomUp = new Button();
            B_zoomDown = new Button();
            B_moveUp = new Button();
            B_moveDown = new Button();
            B_moveLeft = new Button();
            B_moveRight = new Button();
            C_autoGet = new CheckBox();
            Ti_autoGet = new System.Windows.Forms.Timer(components);
            L_message = new Label();
            label1 = new Label();
            N_tick = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)N_lat).BeginInit();
            ((System.ComponentModel.ISupportInitialize)N_lon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)N_dep).BeginInit();
            ((System.ComponentModel.ISupportInitialize)N_latSta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)N_latEnd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)N_lonSta).BeginInit();
            ((System.ComponentModel.ISupportInitialize)N_lonEnd).BeginInit();
            ((System.ComponentModel.ISupportInitialize)P_image).BeginInit();
            ((System.ComponentModel.ISupportInitialize)N_tick).BeginInit();
            SuspendLayout();
            // 
            // T_time
            // 
            T_time.Location = new Point(12, 12);
            T_time.Name = "T_time";
            T_time.Size = new Size(121, 23);
            T_time.TabIndex = 0;
            T_time.Text = "2024/01/01 00:00:00.0";
            // 
            // N_lat
            // 
            N_lat.DecimalPlaces = 1;
            N_lat.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_lat.Location = new Point(139, 13);
            N_lat.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            N_lat.Minimum = new decimal(new int[] { 180, 0, 0, int.MinValue });
            N_lat.Name = "N_lat";
            N_lat.Size = new Size(53, 23);
            N_lat.TabIndex = 1;
            N_lat.Value = new decimal(new int[] { 35, 0, 0, 0 });
            // 
            // N_lon
            // 
            N_lon.DecimalPlaces = 1;
            N_lon.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_lon.Location = new Point(198, 12);
            N_lon.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            N_lon.Minimum = new decimal(new int[] { 180, 0, 0, int.MinValue });
            N_lon.Name = "N_lon";
            N_lon.Size = new Size(53, 23);
            N_lon.TabIndex = 2;
            N_lon.Value = new decimal(new int[] { 135, 0, 0, 0 });
            // 
            // N_dep
            // 
            N_dep.DecimalPlaces = 1;
            N_dep.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_dep.Location = new Point(257, 12);
            N_dep.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
            N_dep.Name = "N_dep";
            N_dep.Size = new Size(53, 23);
            N_dep.TabIndex = 3;
            N_dep.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // B_start
            // 
            B_start.BackColor = SystemColors.Control;
            B_start.ForeColor = Color.Black;
            B_start.Location = new Point(326, 13);
            B_start.Name = "B_start";
            B_start.Size = new Size(50, 23);
            B_start.TabIndex = 4;
            B_start.Text = "START";
            B_start.UseVisualStyleBackColor = false;
            B_start.Click += B_start_Click;
            // 
            // B_get
            // 
            B_get.BackColor = SystemColors.Control;
            B_get.ForeColor = Color.Black;
            B_get.Location = new Point(382, 13);
            B_get.Name = "B_get";
            B_get.Size = new Size(50, 23);
            B_get.TabIndex = 5;
            B_get.Text = "GET";
            B_get.UseVisualStyleBackColor = false;
            B_get.Click += B_get_Click;
            // 
            // B_stop
            // 
            B_stop.BackColor = SystemColors.Control;
            B_stop.Enabled = false;
            B_stop.ForeColor = Color.Black;
            B_stop.Location = new Point(438, 13);
            B_stop.Name = "B_stop";
            B_stop.Size = new Size(50, 23);
            B_stop.TabIndex = 6;
            B_stop.Text = "STOP";
            B_stop.UseVisualStyleBackColor = false;
            B_stop.Click += B_stop_Click;
            // 
            // Ti_proc
            // 
            Ti_proc.Interval = 500;
            Ti_proc.Tick += Ti_proc_Tick;
            // 
            // N_latSta
            // 
            N_latSta.DecimalPlaces = 1;
            N_latSta.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_latSta.Location = new Point(12, 465);
            N_latSta.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            N_latSta.Minimum = new decimal(new int[] { 180, 0, 0, int.MinValue });
            N_latSta.Name = "N_latSta";
            N_latSta.Size = new Size(50, 23);
            N_latSta.TabIndex = 7;
            N_latSta.Value = new decimal(new int[] { 20, 0, 0, 0 });
            // 
            // N_latEnd
            // 
            N_latEnd.DecimalPlaces = 1;
            N_latEnd.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_latEnd.Location = new Point(68, 465);
            N_latEnd.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            N_latEnd.Minimum = new decimal(new int[] { 180, 0, 0, int.MinValue });
            N_latEnd.Name = "N_latEnd";
            N_latEnd.Size = new Size(50, 23);
            N_latEnd.TabIndex = 8;
            N_latEnd.Value = new decimal(new int[] { 50, 0, 0, 0 });
            // 
            // N_lonSta
            // 
            N_lonSta.DecimalPlaces = 1;
            N_lonSta.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_lonSta.Location = new Point(124, 465);
            N_lonSta.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            N_lonSta.Minimum = new decimal(new int[] { 180, 0, 0, int.MinValue });
            N_lonSta.Name = "N_lonSta";
            N_lonSta.Size = new Size(50, 23);
            N_lonSta.TabIndex = 9;
            N_lonSta.Value = new decimal(new int[] { 120, 0, 0, 0 });
            // 
            // N_lonEnd
            // 
            N_lonEnd.DecimalPlaces = 1;
            N_lonEnd.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_lonEnd.Location = new Point(180, 465);
            N_lonEnd.Maximum = new decimal(new int[] { 180, 0, 0, 0 });
            N_lonEnd.Minimum = new decimal(new int[] { 180, 0, 0, int.MinValue });
            N_lonEnd.Name = "N_lonEnd";
            N_lonEnd.Size = new Size(50, 23);
            N_lonEnd.TabIndex = 10;
            N_lonEnd.Value = new decimal(new int[] { 150, 0, 0, 0 });
            // 
            // C_LockLatLon
            // 
            C_LockLatLon.AutoSize = true;
            C_LockLatLon.Checked = true;
            C_LockLatLon.CheckState = CheckState.Checked;
            C_LockLatLon.Location = new Point(236, 468);
            C_LockLatLon.Name = "C_LockLatLon";
            C_LockLatLon.Size = new Size(55, 19);
            C_LockLatLon.TabIndex = 11;
            C_LockLatLon.Text = "LOCK";
            C_LockLatLon.UseVisualStyleBackColor = true;
            // 
            // P_image
            // 
            P_image.BackgroundImageLayout = ImageLayout.Zoom;
            P_image.Dock = DockStyle.Fill;
            P_image.Location = new Point(0, 0);
            P_image.Name = "P_image";
            P_image.Size = new Size(500, 500);
            P_image.TabIndex = 12;
            P_image.TabStop = false;
            // 
            // B_zoomUp
            // 
            B_zoomUp.BackColor = SystemColors.Control;
            B_zoomUp.ForeColor = Color.Black;
            B_zoomUp.Location = new Point(290, 466);
            B_zoomUp.Name = "B_zoomUp";
            B_zoomUp.Size = new Size(23, 23);
            B_zoomUp.TabIndex = 13;
            B_zoomUp.Text = "+";
            B_zoomUp.UseVisualStyleBackColor = false;
            B_zoomUp.Click += B_zoomUp_Click;
            // 
            // B_zoomDown
            // 
            B_zoomDown.BackColor = SystemColors.Control;
            B_zoomDown.ForeColor = Color.Black;
            B_zoomDown.Location = new Point(315, 466);
            B_zoomDown.Name = "B_zoomDown";
            B_zoomDown.Size = new Size(23, 23);
            B_zoomDown.TabIndex = 14;
            B_zoomDown.Text = "-";
            B_zoomDown.UseVisualStyleBackColor = false;
            B_zoomDown.Click += B_zoomDown_Click;
            // 
            // B_moveUp
            // 
            B_moveUp.BackColor = SystemColors.Control;
            B_moveUp.ForeColor = Color.Black;
            B_moveUp.Location = new Point(348, 466);
            B_moveUp.Name = "B_moveUp";
            B_moveUp.Size = new Size(23, 23);
            B_moveUp.TabIndex = 15;
            B_moveUp.Text = "↑";
            B_moveUp.UseVisualStyleBackColor = false;
            B_moveUp.Click += B_moveUp_Click;
            // 
            // B_moveDown
            // 
            B_moveDown.BackColor = SystemColors.Control;
            B_moveDown.ForeColor = Color.Black;
            B_moveDown.Location = new Point(374, 466);
            B_moveDown.Name = "B_moveDown";
            B_moveDown.Size = new Size(23, 23);
            B_moveDown.TabIndex = 16;
            B_moveDown.Text = "↓";
            B_moveDown.UseVisualStyleBackColor = false;
            B_moveDown.Click += B_moveDown_Click;
            // 
            // B_moveLeft
            // 
            B_moveLeft.BackColor = SystemColors.Control;
            B_moveLeft.ForeColor = Color.Black;
            B_moveLeft.Location = new Point(400, 466);
            B_moveLeft.Name = "B_moveLeft";
            B_moveLeft.Size = new Size(23, 23);
            B_moveLeft.TabIndex = 17;
            B_moveLeft.Text = "←";
            B_moveLeft.UseVisualStyleBackColor = false;
            B_moveLeft.Click += B_moveLeft_Click;
            // 
            // B_moveRight
            // 
            B_moveRight.BackColor = SystemColors.Control;
            B_moveRight.ForeColor = Color.Black;
            B_moveRight.Location = new Point(426, 466);
            B_moveRight.Name = "B_moveRight";
            B_moveRight.Size = new Size(23, 23);
            B_moveRight.TabIndex = 18;
            B_moveRight.Text = "→";
            B_moveRight.UseVisualStyleBackColor = false;
            B_moveRight.Click += B_moveRight_Click;
            // 
            // C_autoGet
            // 
            C_autoGet.AutoSize = true;
            C_autoGet.Location = new Point(408, 42);
            C_autoGet.Name = "C_autoGet";
            C_autoGet.Size = new Size(80, 19);
            C_autoGet.TabIndex = 19;
            C_autoGet.Text = "AUTO GET";
            C_autoGet.UseVisualStyleBackColor = true;
            C_autoGet.CheckedChanged += C_autoGet_CheckedChanged;
            // 
            // Ti_autoGet
            // 
            Ti_autoGet.Interval = 5000;
            Ti_autoGet.Tick += Ti_autoGet_Tick;
            // 
            // L_message
            // 
            L_message.AutoSize = true;
            L_message.Location = new Point(12, 43);
            L_message.Name = "L_message";
            L_message.Size = new Size(0, 15);
            L_message.TabIndex = 20;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(400, 448);
            label1.Name = "label1";
            label1.Size = new Size(96, 15);
            label1.TabIndex = 21;
            label1.Text = "地図データ:気象庁";
            // 
            // N_tick
            // 
            N_tick.DecimalPlaces = 2;
            N_tick.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            N_tick.Location = new Point(455, 467);
            N_tick.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            N_tick.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            N_tick.Name = "N_tick";
            N_tick.Size = new Size(41, 23);
            N_tick.TabIndex = 22;
            N_tick.Value = new decimal(new int[] { 50, 0, 0, 131072 });
            N_tick.ValueChanged += N_tick_ValueChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(500, 500);
            Controls.Add(N_tick);
            Controls.Add(label1);
            Controls.Add(L_message);
            Controls.Add(C_autoGet);
            Controls.Add(B_moveRight);
            Controls.Add(B_moveLeft);
            Controls.Add(B_moveDown);
            Controls.Add(B_moveUp);
            Controls.Add(B_zoomDown);
            Controls.Add(B_zoomUp);
            Controls.Add(C_LockLatLon);
            Controls.Add(N_lonEnd);
            Controls.Add(N_lonSta);
            Controls.Add(N_latEnd);
            Controls.Add(N_latSta);
            Controls.Add(B_stop);
            Controls.Add(B_get);
            Controls.Add(B_start);
            Controls.Add(N_dep);
            Controls.Add(N_lon);
            Controls.Add(N_lat);
            Controls.Add(T_time);
            Controls.Add(P_image);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "PSWaveDistance.Test.RealTime";
            ((System.ComponentModel.ISupportInitialize)N_lat).EndInit();
            ((System.ComponentModel.ISupportInitialize)N_lon).EndInit();
            ((System.ComponentModel.ISupportInitialize)N_dep).EndInit();
            ((System.ComponentModel.ISupportInitialize)N_latSta).EndInit();
            ((System.ComponentModel.ISupportInitialize)N_latEnd).EndInit();
            ((System.ComponentModel.ISupportInitialize)N_lonSta).EndInit();
            ((System.ComponentModel.ISupportInitialize)N_lonEnd).EndInit();
            ((System.ComponentModel.ISupportInitialize)P_image).EndInit();
            ((System.ComponentModel.ISupportInitialize)N_tick).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox T_time;
        private NumericUpDown N_lat;
        private NumericUpDown N_lon;
        private NumericUpDown N_dep;
        private Button B_start;
        private Button B_get;
        private Button B_stop;
        private System.Windows.Forms.Timer Ti_proc;
        private NumericUpDown N_latSta;
        private NumericUpDown N_latEnd;
        private NumericUpDown N_lonSta;
        private NumericUpDown N_lonEnd;
        private CheckBox C_LockLatLon;
        private PictureBox P_image;
        private Button B_zoomUp;
        private Button B_zoomDown;
        private Button B_moveUp;
        private Button B_moveDown;
        private Button B_moveLeft;
        private Button B_moveRight;
        private CheckBox C_autoGet;
        private System.Windows.Forms.Timer Ti_autoGet;
        private Label L_message;
        private Label label1;
        private NumericUpDown N_tick;
    }
}
