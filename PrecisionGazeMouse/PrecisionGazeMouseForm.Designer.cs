namespace PrecisionGazeMouse
{
    partial class PrecisionGazeMouseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrecisionGazeMouseForm));
            PositionLabel = new System.Windows.Forms.Label();
            QuitButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            HeadRotationLabel = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            StatusLabel = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            ClickOnKey = new System.Windows.Forms.Label();
            notifyIcon = new System.Windows.Forms.NotifyIcon(components);
            PauseOnKey = new System.Windows.Forms.Label();
            PauseOnKeyInput = new System.Windows.Forms.TextBox();
            ClickOnKeyInput = new System.Windows.Forms.TextBox();
            SensitivityInput = new System.Windows.Forms.TrackBar();
            MovementOnKeyPressInput = new System.Windows.Forms.TextBox();
            OnKeyPressButton = new System.Windows.Forms.RadioButton();
            ContinuousButton = new System.Windows.Forms.RadioButton();
            gazeTracker = new System.Windows.Forms.CheckBox();
            warpBar = new System.Windows.Forms.CheckBox();
            ModeBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(SensitivityInput)).BeginInit();
            SuspendLayout();
            // 
            // PositionLabel
            // 
            PositionLabel.AutoSize = true;
            PositionLabel.Location = new System.Drawing.Point(146, 358);
            PositionLabel.Name = "PositionLabel";
            PositionLabel.Size = new System.Drawing.Size(45, 20);
            PositionLabel.TabIndex = 8;
            PositionLabel.Text = "(0, 0)";
            // 
            // QuitButton
            // 
            QuitButton.Location = new System.Drawing.Point(170, 442);
            QuitButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            QuitButton.Name = "QuitButton";
            QuitButton.Size = new System.Drawing.Size(84, 29);
            QuitButton.TabIndex = 9;
            QuitButton.Text = "Quit";
            QuitButton.UseVisualStyleBackColor = true;
            QuitButton.Click += new System.EventHandler(QuitButton_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(27, 358);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(108, 20);
            label1.TabIndex = 10;
            label1.Text = "Gaze Position";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(24, 395);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(113, 20);
            label2.TabIndex = 11;
            label2.Text = "Head Rotation";
            // 
            // HeadRotationLabel
            // 
            HeadRotationLabel.AutoSize = true;
            HeadRotationLabel.Location = new System.Drawing.Point(146, 395);
            HeadRotationLabel.Name = "HeadRotationLabel";
            HeadRotationLabel.Size = new System.Drawing.Size(45, 20);
            HeadRotationLabel.TabIndex = 12;
            HeadRotationLabel.Text = "(0, 0)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(78, 322);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(56, 20);
            label3.TabIndex = 13;
            label3.Text = "Status";
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Location = new System.Drawing.Point(146, 322);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new System.Drawing.Size(65, 20);
            StatusLabel.TabIndex = 14;
            StatusLabel.Text = "Starting";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(25, 25);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(106, 20);
            label5.TabIndex = 17;
            label5.Text = "Tracker Mode";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(51, 71);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(83, 20);
            label4.TabIndex = 22;
            label4.Text = "Movement";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(56, 260);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(79, 20);
            label7.TabIndex = 27;
            label7.Text = "Sensitivity";
            // 
            // ClickOnKey
            // 
            ClickOnKey.AutoSize = true;
            ClickOnKey.Location = new System.Drawing.Point(38, 158);
            ClickOnKey.Name = "ClickOnKey";
            ClickOnKey.Size = new System.Drawing.Size(97, 20);
            ClickOnKey.TabIndex = 28;
            ClickOnKey.Text = "Click On Key";
            // 
            // notifyIcon
            // 
            notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            notifyIcon.Text = "Precision Gaze Mouse";
            notifyIcon.Click += new System.EventHandler(notifyIcon_Click);
            // 
            // PauseOnKey
            // 
            PauseOnKey.AutoSize = true;
            PauseOnKey.Location = new System.Drawing.Point(29, 205);
            PauseOnKey.Name = "PauseOnKey";
            PauseOnKey.Size = new System.Drawing.Size(109, 20);
            PauseOnKey.TabIndex = 30;
            PauseOnKey.Text = "Pause On Key";
            // 
            // PauseOnKeyInput
            // 
            PauseOnKeyInput.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "PauseOnKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            PauseOnKeyInput.Location = new System.Drawing.Point(146, 202);
            PauseOnKeyInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            PauseOnKeyInput.Name = "PauseOnKeyInput";
            PauseOnKeyInput.Size = new System.Drawing.Size(114, 26);
            PauseOnKeyInput.TabIndex = 31;
            PauseOnKeyInput.Text = global::PrecisionGazeMouse.Properties.Settings.Default.PauseOnKey;
            PauseOnKeyInput.KeyDown += new System.Windows.Forms.KeyEventHandler(PauseOnKeyInput_KeyDown);
            // 
            // ClickOnKeyInput
            // 
            ClickOnKeyInput.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "ClickOnKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            ClickOnKeyInput.Location = new System.Drawing.Point(148, 155);
            ClickOnKeyInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ClickOnKeyInput.Name = "ClickOnKeyInput";
            ClickOnKeyInput.Size = new System.Drawing.Size(112, 26);
            ClickOnKeyInput.TabIndex = 29;
            ClickOnKeyInput.Text = global::PrecisionGazeMouse.Properties.Settings.Default.ClickOnKey;
            ClickOnKeyInput.KeyDown += new System.Windows.Forms.KeyEventHandler(OnClickKeyPressInput_KeyDown);
            // 
            // SensitivityInput
            // 
            SensitivityInput.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::PrecisionGazeMouse.Properties.Settings.Default, "Sensitivity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            SensitivityInput.Location = new System.Drawing.Point(142, 249);
            SensitivityInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            SensitivityInput.Maximum = 20;
            SensitivityInput.Name = "SensitivityInput";
            SensitivityInput.Size = new System.Drawing.Size(302, 69);
            SensitivityInput.TabIndex = 26;
            SensitivityInput.Value = global::PrecisionGazeMouse.Properties.Settings.Default.Sensitivity;
            SensitivityInput.Scroll += new System.EventHandler(SensitivityInput_Scroll);
            // 
            // MovementOnKeyPressInput
            // 
            MovementOnKeyPressInput.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "MovementKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            MovementOnKeyPressInput.Location = new System.Drawing.Point(288, 105);
            MovementOnKeyPressInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            MovementOnKeyPressInput.Name = "MovementOnKeyPressInput";
            MovementOnKeyPressInput.Size = new System.Drawing.Size(112, 26);
            MovementOnKeyPressInput.TabIndex = 25;
            MovementOnKeyPressInput.Text = global::PrecisionGazeMouse.Properties.Settings.Default.MovementKey;
            MovementOnKeyPressInput.KeyDown += new System.Windows.Forms.KeyEventHandler(MovementOnKeyPressButton_Click);
            // 
            // OnKeyPressButton
            // 
            OnKeyPressButton.AutoSize = true;
            OnKeyPressButton.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.OnKeyPressMovement;
            OnKeyPressButton.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "OnKeyPressMovement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            OnKeyPressButton.Location = new System.Drawing.Point(148, 105);
            OnKeyPressButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            OnKeyPressButton.Name = "OnKeyPressButton";
            OnKeyPressButton.Size = new System.Drawing.Size(129, 24);
            OnKeyPressButton.TabIndex = 24;
            OnKeyPressButton.TabStop = true;
            OnKeyPressButton.Text = "On Key Press";
            OnKeyPressButton.UseVisualStyleBackColor = true;
            OnKeyPressButton.Click += new System.EventHandler(OnKeyPressButton_Click);
            // 
            // ContinuousButton
            // 
            ContinuousButton.AutoSize = true;
            ContinuousButton.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.ContinuousMovement;
            ContinuousButton.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "ContinuousMovement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            ContinuousButton.Location = new System.Drawing.Point(148, 71);
            ContinuousButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ContinuousButton.Name = "ContinuousButton";
            ContinuousButton.Size = new System.Drawing.Size(115, 24);
            ContinuousButton.TabIndex = 23;
            ContinuousButton.Text = "Continuous";
            ContinuousButton.UseVisualStyleBackColor = true;
            ContinuousButton.Click += new System.EventHandler(ContinuousButton_Click);
            // 
            // gazeTracker
            // 
            gazeTracker.AutoSize = true;
            gazeTracker.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.ShowGazeTracker;
            gazeTracker.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "ShowGazeTracker", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            gazeTracker.Location = new System.Drawing.Point(268, 356);
            gazeTracker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            gazeTracker.Name = "gazeTracker";
            gazeTracker.Size = new System.Drawing.Size(175, 24);
            gazeTracker.TabIndex = 21;
            gazeTracker.Text = "Show Gaze Tracker";
            gazeTracker.UseVisualStyleBackColor = true;
            gazeTracker.CheckedChanged += new System.EventHandler(gazeTracker_CheckedChanged);
            // 
            // warpBar
            // 
            warpBar.AutoSize = true;
            warpBar.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.ShowWarpBar;
            warpBar.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "ShowWarpBar", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            warpBar.Location = new System.Drawing.Point(268, 322);
            warpBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            warpBar.Name = "warpBar";
            warpBar.Size = new System.Drawing.Size(146, 24);
            warpBar.TabIndex = 20;
            warpBar.Text = "Show Warp Bar";
            warpBar.UseVisualStyleBackColor = true;
            warpBar.CheckedChanged += new System.EventHandler(warpBar_CheckedChanged);
            // 
            // ModeBox
            // 
            ModeBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "TrackerMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            ModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ModeBox.FormattingEnabled = true;
            ModeBox.Items.AddRange(new object[] {
            "EyeX and eViacam",
            "EyeX and TrackIR",
            "EyeX and SmartNav",
            "EyeX Only",
            "TrackIR Only",
            "eViacam Only"});
            ModeBox.Location = new System.Drawing.Point(148, 22);
            ModeBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            ModeBox.Name = "ModeBox";
            ModeBox.Size = new System.Drawing.Size(294, 28);
            ModeBox.TabIndex = 18;
            ModeBox.Text = global::PrecisionGazeMouse.Properties.Settings.Default.TrackerMode;
            ModeBox.SelectedIndexChanged += new System.EventHandler(ModeBox_SelectedIndexChanged);
            // 
            // PrecisionGazeMouseForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(468, 496);
            Controls.Add(PauseOnKeyInput);
            Controls.Add(PauseOnKey);
            Controls.Add(ClickOnKeyInput);
            Controls.Add(ClickOnKey);
            Controls.Add(label7);
            Controls.Add(SensitivityInput);
            Controls.Add(MovementOnKeyPressInput);
            Controls.Add(OnKeyPressButton);
            Controls.Add(ContinuousButton);
            Controls.Add(label4);
            Controls.Add(gazeTracker);
            Controls.Add(warpBar);
            Controls.Add(ModeBox);
            Controls.Add(label5);
            Controls.Add(StatusLabel);
            Controls.Add(label3);
            Controls.Add(HeadRotationLabel);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(QuitButton);
            Controls.Add(PositionLabel);
            DoubleBuffered = true;
            Icon = ((System.Drawing.Icon)(resources.GetObject("$Icon")));
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "PrecisionGazeMouseForm";
            Text = "Precision Gaze Mouse";
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(PrecisionGazeMouseForm_FormClosing);
            Shown += new System.EventHandler(PrecisionGazeMouseForm_Shown);
            Resize += new System.EventHandler(PrecisionGazeMouseForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(SensitivityInput)).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label HeadRotationLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ModeBox;
        private System.Windows.Forms.CheckBox warpBar;
        private System.Windows.Forms.CheckBox gazeTracker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton ContinuousButton;
        private System.Windows.Forms.RadioButton OnKeyPressButton;
        private System.Windows.Forms.TextBox MovementOnKeyPressInput;
        private System.Windows.Forms.TrackBar SensitivityInput;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label ClickOnKey;
        private System.Windows.Forms.TextBox ClickOnKeyInput;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.TextBox PauseOnKeyInput;
        private System.Windows.Forms.Label PauseOnKey;
    }
}

