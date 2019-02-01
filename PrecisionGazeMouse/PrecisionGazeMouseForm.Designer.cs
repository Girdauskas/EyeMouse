﻿namespace PrecisionGazeMouse
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrecisionGazeMouseForm));
            this.PositionLabel = new System.Windows.Forms.Label();
            this.QuitButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.HeadRotationLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.ClickOnKey = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.PauseOnKey = new System.Windows.Forms.Label();
            this.PauseOnKeyInput = new System.Windows.Forms.TextBox();
            this.ClickOnKeyInput = new System.Windows.Forms.TextBox();
            this.SensitivityInput = new System.Windows.Forms.TrackBar();
            this.MovementOnKeyPressInput = new System.Windows.Forms.TextBox();
            this.OnKeyPressButton = new System.Windows.Forms.RadioButton();
            this.ContinuousButton = new System.Windows.Forms.RadioButton();
            this.gazeTracker = new System.Windows.Forms.CheckBox();
            this.warpBar = new System.Windows.Forms.CheckBox();
            this.ModeBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.SensitivityInput)).BeginInit();
            this.SuspendLayout();
            // 
            // PositionLabel
            // 
            this.PositionLabel.AutoSize = true;
            this.PositionLabel.Location = new System.Drawing.Point(146, 358);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(45, 20);
            this.PositionLabel.TabIndex = 8;
            this.PositionLabel.Text = "(0, 0)";
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(170, 442);
            this.QuitButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(84, 29);
            this.QuitButton.TabIndex = 9;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            this.QuitButton.Click += new System.EventHandler(this.QuitButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 358);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "Gaze Position";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 395);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Head Rotation";
            // 
            // HeadRotationLabel
            // 
            this.HeadRotationLabel.AutoSize = true;
            this.HeadRotationLabel.Location = new System.Drawing.Point(146, 395);
            this.HeadRotationLabel.Name = "HeadRotationLabel";
            this.HeadRotationLabel.Size = new System.Drawing.Size(45, 20);
            this.HeadRotationLabel.TabIndex = 12;
            this.HeadRotationLabel.Text = "(0, 0)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(78, 322);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 13;
            this.label3.Text = "Status";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(146, 322);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(65, 20);
            this.StatusLabel.TabIndex = 14;
            this.StatusLabel.Text = "Starting";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 20);
            this.label5.TabIndex = 17;
            this.label5.Text = "Tracker Mode";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 20);
            this.label4.TabIndex = 22;
            this.label4.Text = "Movement";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(56, 260);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 20);
            this.label7.TabIndex = 27;
            this.label7.Text = "Sensitivity";
            // 
            // ClickOnKey
            // 
            this.ClickOnKey.AutoSize = true;
            this.ClickOnKey.Location = new System.Drawing.Point(38, 158);
            this.ClickOnKey.Name = "ClickOnKey";
            this.ClickOnKey.Size = new System.Drawing.Size(97, 20);
            this.ClickOnKey.TabIndex = 28;
            this.ClickOnKey.Text = "Click On Key";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Precision Gaze Mouse";
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // PauseOnKey
            // 
            this.PauseOnKey.AutoSize = true;
            this.PauseOnKey.Location = new System.Drawing.Point(29, 205);
            this.PauseOnKey.Name = "PauseOnKey";
            this.PauseOnKey.Size = new System.Drawing.Size(109, 20);
            this.PauseOnKey.TabIndex = 30;
            this.PauseOnKey.Text = "Pause On Key";
            // 
            // PauseOnKeyInput
            // 
            this.PauseOnKeyInput.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "PauseOnKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.PauseOnKeyInput.Location = new System.Drawing.Point(146, 202);
            this.PauseOnKeyInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PauseOnKeyInput.Name = "PauseOnKeyInput";
            this.PauseOnKeyInput.Size = new System.Drawing.Size(114, 26);
            this.PauseOnKeyInput.TabIndex = 31;
            this.PauseOnKeyInput.Text = global::PrecisionGazeMouse.Properties.Settings.Default.PauseOnKey;
            this.PauseOnKeyInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PauseOnKeyInput_KeyDown);
            // 
            // ClickOnKeyInput
            // 
            this.ClickOnKeyInput.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "ClickOnKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ClickOnKeyInput.Location = new System.Drawing.Point(148, 155);
            this.ClickOnKeyInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ClickOnKeyInput.Name = "ClickOnKeyInput";
            this.ClickOnKeyInput.Size = new System.Drawing.Size(112, 26);
            this.ClickOnKeyInput.TabIndex = 29;
            this.ClickOnKeyInput.Text = global::PrecisionGazeMouse.Properties.Settings.Default.ClickOnKey;
            this.ClickOnKeyInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnClickKeyPressInput_KeyDown);
            // 
            // SensitivityInput
            // 
            this.SensitivityInput.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::PrecisionGazeMouse.Properties.Settings.Default, "Sensitivity", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SensitivityInput.Location = new System.Drawing.Point(142, 249);
            this.SensitivityInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SensitivityInput.Maximum = 20;
            this.SensitivityInput.Name = "SensitivityInput";
            this.SensitivityInput.Size = new System.Drawing.Size(302, 69);
            this.SensitivityInput.TabIndex = 26;
            this.SensitivityInput.Value = global::PrecisionGazeMouse.Properties.Settings.Default.Sensitivity;
            this.SensitivityInput.Scroll += new System.EventHandler(this.SensitivityInput_Scroll);
            // 
            // MovementOnKeyPressInput
            // 
            this.MovementOnKeyPressInput.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "MovementKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MovementOnKeyPressInput.Location = new System.Drawing.Point(288, 105);
            this.MovementOnKeyPressInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MovementOnKeyPressInput.Name = "MovementOnKeyPressInput";
            this.MovementOnKeyPressInput.Size = new System.Drawing.Size(112, 26);
            this.MovementOnKeyPressInput.TabIndex = 25;
            this.MovementOnKeyPressInput.Text = global::PrecisionGazeMouse.Properties.Settings.Default.MovementKey;
            this.MovementOnKeyPressInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MovementOnKeyPressButton_Click);
            // 
            // OnKeyPressButton
            // 
            this.OnKeyPressButton.AutoSize = true;
            this.OnKeyPressButton.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.OnKeyPressMovement;
            this.OnKeyPressButton.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "OnKeyPressMovement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.OnKeyPressButton.Location = new System.Drawing.Point(148, 105);
            this.OnKeyPressButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.OnKeyPressButton.Name = "OnKeyPressButton";
            this.OnKeyPressButton.Size = new System.Drawing.Size(129, 24);
            this.OnKeyPressButton.TabIndex = 24;
            this.OnKeyPressButton.TabStop = true;
            this.OnKeyPressButton.Text = "On Key Press";
            this.OnKeyPressButton.UseVisualStyleBackColor = true;
            this.OnKeyPressButton.Click += new System.EventHandler(this.OnKeyPressButton_Click);
            // 
            // ContinuousButton
            // 
            this.ContinuousButton.AutoSize = true;
            this.ContinuousButton.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.ContinuousMovement;
            this.ContinuousButton.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "ContinuousMovement", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ContinuousButton.Location = new System.Drawing.Point(148, 71);
            this.ContinuousButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ContinuousButton.Name = "ContinuousButton";
            this.ContinuousButton.Size = new System.Drawing.Size(115, 24);
            this.ContinuousButton.TabIndex = 23;
            this.ContinuousButton.Text = "Continuous";
            this.ContinuousButton.UseVisualStyleBackColor = true;
            this.ContinuousButton.Click += new System.EventHandler(this.ContinuousButton_Click);
            // 
            // gazeTracker
            // 
            this.gazeTracker.AutoSize = true;
            this.gazeTracker.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.ShowGazeTracker;
            this.gazeTracker.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "ShowGazeTracker", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.gazeTracker.Location = new System.Drawing.Point(268, 356);
            this.gazeTracker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gazeTracker.Name = "gazeTracker";
            this.gazeTracker.Size = new System.Drawing.Size(175, 24);
            this.gazeTracker.TabIndex = 21;
            this.gazeTracker.Text = "Show Gaze Tracker";
            this.gazeTracker.UseVisualStyleBackColor = true;
            this.gazeTracker.CheckedChanged += new System.EventHandler(this.gazeTracker_CheckedChanged);
            // 
            // warpBar
            // 
            this.warpBar.AutoSize = true;
            this.warpBar.Checked = global::PrecisionGazeMouse.Properties.Settings.Default.ShowWarpBar;
            this.warpBar.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::PrecisionGazeMouse.Properties.Settings.Default, "ShowWarpBar", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.warpBar.Location = new System.Drawing.Point(268, 322);
            this.warpBar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.warpBar.Name = "warpBar";
            this.warpBar.Size = new System.Drawing.Size(146, 24);
            this.warpBar.TabIndex = 20;
            this.warpBar.Text = "Show Warp Bar";
            this.warpBar.UseVisualStyleBackColor = true;
            this.warpBar.CheckedChanged += new System.EventHandler(this.warpBar_CheckedChanged);
            // 
            // ModeBox
            // 
            this.ModeBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::PrecisionGazeMouse.Properties.Settings.Default, "TrackerMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ModeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeBox.FormattingEnabled = true;
            this.ModeBox.Items.AddRange(new object[] {
            "EyeX and eViacam",
            "EyeX and TrackIR",
            "EyeX and SmartNav",
            "EyeX Only",
            "TrackIR Only",
            "eViacam Only"});
            this.ModeBox.Location = new System.Drawing.Point(148, 22);
            this.ModeBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ModeBox.Name = "ModeBox";
            this.ModeBox.Size = new System.Drawing.Size(294, 28);
            this.ModeBox.TabIndex = 18;
            this.ModeBox.Text = global::PrecisionGazeMouse.Properties.Settings.Default.TrackerMode;
            this.ModeBox.SelectedIndexChanged += new System.EventHandler(this.ModeBox_SelectedIndexChanged);
            // 
            // PrecisionGazeMouseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 496);
            this.Controls.Add(this.PauseOnKeyInput);
            this.Controls.Add(this.PauseOnKey);
            this.Controls.Add(this.ClickOnKeyInput);
            this.Controls.Add(this.ClickOnKey);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SensitivityInput);
            this.Controls.Add(this.MovementOnKeyPressInput);
            this.Controls.Add(this.OnKeyPressButton);
            this.Controls.Add(this.ContinuousButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.gazeTracker);
            this.Controls.Add(this.warpBar);
            this.Controls.Add(this.ModeBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.HeadRotationLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QuitButton);
            this.Controls.Add(this.PositionLabel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PrecisionGazeMouseForm";
            this.Text = "Precision Gaze Mouse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrecisionGazeMouseForm_FormClosing);
            this.Shown += new System.EventHandler(this.PrecisionGazeMouseForm_Shown);
            this.Resize += new System.EventHandler(this.PrecisionGazeMouseForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.SensitivityInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

