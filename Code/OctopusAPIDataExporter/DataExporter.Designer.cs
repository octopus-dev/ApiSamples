namespace OctopusAPIDataExporter
{
    partial class DataExporter
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataExporter));
            this.textBox_tokenText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_userName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_reGetToken = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_groupStop = new System.Windows.Forms.Button();
            this.progressBarGroupProgress = new System.Windows.Forms.ProgressBar();
            this.button_groupPause = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.listView_progressInfo = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button_GetTaskGroup = new System.Windows.Forms.Button();
            this.listView_taskGroup = new System.Windows.Forms.ListView();
            this.TaskGroups = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label_groupProgress = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_pageSize = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_pageIndex = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_selectSavePath = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radioButton_Json = new System.Windows.Forms.RadioButton();
            this.radioButton_Xml = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_dataType = new System.Windows.Forms.RadioButton();
            this.radioButton_dataType2 = new System.Windows.Forms.RadioButton();
            this.textBox_savePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_startExport = new System.Windows.Forms.Button();
            this.button_pause = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label_taskProgress = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pageIndex)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox_tokenText
            // 
            this.textBox_tokenText.Location = new System.Drawing.Point(272, 6);
            this.textBox_tokenText.Multiline = true;
            this.textBox_tokenText.Name = "textBox_tokenText";
            this.textBox_tokenText.ReadOnly = true;
            this.textBox_tokenText.Size = new System.Drawing.Size(402, 21);
            this.textBox_tokenText.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(231, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "Token";
            // 
            // textBox_userName
            // 
            this.textBox_userName.Location = new System.Drawing.Point(101, 6);
            this.textBox_userName.Name = "textBox_userName";
            this.textBox_userName.ReadOnly = true;
            this.textBox_userName.Size = new System.Drawing.Size(124, 21);
            this.textBox_userName.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "User Name";
            // 
            // button_reGetToken
            // 
            this.button_reGetToken.Location = new System.Drawing.Point(680, 4);
            this.button_reGetToken.Name = "button_reGetToken";
            this.button_reGetToken.Size = new System.Drawing.Size(92, 23);
            this.button_reGetToken.TabIndex = 14;
            this.button_reGetToken.Text = "Get New Token";
            this.button_reGetToken.UseVisualStyleBackColor = true;
            this.button_reGetToken.Click += new System.EventHandler(this.button_reGetToken_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_groupStop);
            this.groupBox1.Controls.Add(this.progressBarGroupProgress);
            this.groupBox1.Controls.Add(this.button_groupPause);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.listView_progressInfo);
            this.groupBox1.Controls.Add(this.button_GetTaskGroup);
            this.groupBox1.Controls.Add(this.listView_taskGroup);
            this.groupBox1.Location = new System.Drawing.Point(11, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(761, 306);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "State of TaskGroup and Task";
            // 
            // button_groupStop
            // 
            this.button_groupStop.Location = new System.Drawing.Point(283, 16);
            this.button_groupStop.Name = "button_groupStop";
            this.button_groupStop.Size = new System.Drawing.Size(75, 23);
            this.button_groupStop.TabIndex = 22;
            this.button_groupStop.Text = "Stop";
            this.button_groupStop.UseVisualStyleBackColor = true;
            this.button_groupStop.Click += new System.EventHandler(this.button_groupStop_Click);
            // 
            // progressBarGroupProgress
            // 
            this.progressBarGroupProgress.Location = new System.Drawing.Point(144, 274);
            this.progressBarGroupProgress.Name = "progressBarGroupProgress";
            this.progressBarGroupProgress.Size = new System.Drawing.Size(611, 23);
            this.progressBarGroupProgress.TabIndex = 6;
            // 
            // button_groupPause
            // 
            this.button_groupPause.Location = new System.Drawing.Point(200, 16);
            this.button_groupPause.Name = "button_groupPause";
            this.button_groupPause.Size = new System.Drawing.Size(75, 23);
            this.button_groupPause.TabIndex = 21;
            this.button_groupPause.Text = "Pause";
            this.button_groupPause.UseVisualStyleBackColor = true;
            this.button_groupPause.Click += new System.EventHandler(this.button_groupPause_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 279);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "Progress of TaskGroup";
            // 
            // listView_progressInfo
            // 
            this.listView_progressInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.listView_progressInfo.Location = new System.Drawing.Point(222, 45);
            this.listView_progressInfo.Name = "listView_progressInfo";
            this.listView_progressInfo.Size = new System.Drawing.Size(533, 223);
            this.listView_progressInfo.TabIndex = 4;
            this.listView_progressInfo.UseCompatibleStateImageBehavior = false;
            this.listView_progressInfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "State of requiring task(s)";
            this.columnHeader1.Width = 1000;
            // 
            // button_GetTaskGroup
            // 
            this.button_GetTaskGroup.Location = new System.Drawing.Point(8, 16);
            this.button_GetTaskGroup.Name = "button_GetTaskGroup";
            this.button_GetTaskGroup.Size = new System.Drawing.Size(186, 23);
            this.button_GetTaskGroup.TabIndex = 1;
            this.button_GetTaskGroup.Text = "Get task information";
            this.button_GetTaskGroup.UseVisualStyleBackColor = true;
            this.button_GetTaskGroup.Click += new System.EventHandler(this.button_GetTaskGroup_Click);
            // 
            // listView_taskGroup
            // 
            this.listView_taskGroup.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TaskGroups});
            this.listView_taskGroup.Location = new System.Drawing.Point(7, 45);
            this.listView_taskGroup.Name = "listView_taskGroup";
            this.listView_taskGroup.Size = new System.Drawing.Size(207, 223);
            this.listView_taskGroup.TabIndex = 0;
            this.listView_taskGroup.UseCompatibleStateImageBehavior = false;
            this.listView_taskGroup.View = System.Windows.Forms.View.Details;
            // 
            // TaskGroups
            // 
            this.TaskGroups.Text = "TaskGroups";
            this.TaskGroups.Width = 1000;
            // 
            // label_groupProgress
            // 
            this.label_groupProgress.AutoSize = true;
            this.label_groupProgress.Location = new System.Drawing.Point(9, 593);
            this.label_groupProgress.Name = "label_groupProgress";
            this.label_groupProgress.Size = new System.Drawing.Size(83, 12);
            this.label_groupProgress.TabIndex = 2;
            this.label_groupProgress.Text = "GroupProgress";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDown_pageSize);
            this.groupBox2.Controls.Add(this.numericUpDown_pageIndex);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.button_selectSavePath);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.textBox_savePath);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(11, 346);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(761, 130);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration for data export";
            // 
            // numericUpDown_pageSize
            // 
            this.numericUpDown_pageSize.Location = new System.Drawing.Point(318, 37);
            this.numericUpDown_pageSize.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_pageSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_pageSize.Name = "numericUpDown_pageSize";
            this.numericUpDown_pageSize.Size = new System.Drawing.Size(91, 21);
            this.numericUpDown_pageSize.TabIndex = 16;
            this.numericUpDown_pageSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDown_pageIndex
            // 
            this.numericUpDown_pageIndex.Location = new System.Drawing.Point(551, 40);
            this.numericUpDown_pageIndex.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_pageIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_pageIndex.Name = "numericUpDown_pageIndex";
            this.numericUpDown_pageIndex.Size = new System.Drawing.Size(91, 21);
            this.numericUpDown_pageIndex.TabIndex = 15;
            this.numericUpDown_pageIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "Page size";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(440, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "Page index";
            // 
            // button_selectSavePath
            // 
            this.button_selectSavePath.Location = new System.Drawing.Point(649, 91);
            this.button_selectSavePath.Name = "button_selectSavePath";
            this.button_selectSavePath.Size = new System.Drawing.Size(86, 23);
            this.button_selectSavePath.TabIndex = 10;
            this.button_selectSavePath.Text = "Select...";
            this.button_selectSavePath.UseVisualStyleBackColor = true;
            this.button_selectSavePath.Click += new System.EventHandler(this.button_selectSavePath_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButton_Json);
            this.groupBox4.Controls.Add(this.radioButton_Xml);
            this.groupBox4.Location = new System.Drawing.Point(10, 74);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(197, 49);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Format of file";
            // 
            // radioButton_Json
            // 
            this.radioButton_Json.AutoSize = true;
            this.radioButton_Json.Checked = true;
            this.radioButton_Json.Location = new System.Drawing.Point(6, 20);
            this.radioButton_Json.Name = "radioButton_Json";
            this.radioButton_Json.Size = new System.Drawing.Size(47, 16);
            this.radioButton_Json.TabIndex = 4;
            this.radioButton_Json.TabStop = true;
            this.radioButton_Json.Text = "Json";
            this.radioButton_Json.UseVisualStyleBackColor = true;
            // 
            // radioButton_Xml
            // 
            this.radioButton_Xml.AutoSize = true;
            this.radioButton_Xml.Location = new System.Drawing.Point(82, 20);
            this.radioButton_Xml.Name = "radioButton_Xml";
            this.radioButton_Xml.Size = new System.Drawing.Size(41, 16);
            this.radioButton_Xml.TabIndex = 5;
            this.radioButton_Xml.Text = "XML";
            this.radioButton_Xml.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_dataType);
            this.groupBox3.Controls.Add(this.radioButton_dataType2);
            this.groupBox3.Location = new System.Drawing.Point(10, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(197, 49);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Scope of data";
            // 
            // radioButton_dataType
            // 
            this.radioButton_dataType.AutoSize = true;
            this.radioButton_dataType.Checked = true;
            this.radioButton_dataType.Location = new System.Drawing.Point(8, 20);
            this.radioButton_dataType.Name = "radioButton_dataType";
            this.radioButton_dataType.Size = new System.Drawing.Size(71, 16);
            this.radioButton_dataType.TabIndex = 1;
            this.radioButton_dataType.TabStop = true;
            this.radioButton_dataType.Text = "All data";
            this.radioButton_dataType.UseVisualStyleBackColor = true;
            // 
            // radioButton_dataType2
            // 
            this.radioButton_dataType2.AutoSize = true;
            this.radioButton_dataType2.Location = new System.Drawing.Point(82, 20);
            this.radioButton_dataType2.Name = "radioButton_dataType2";
            this.radioButton_dataType2.Size = new System.Drawing.Size(113, 16);
            this.radioButton_dataType2.TabIndex = 2;
            this.radioButton_dataType2.Text = "Unexported data";
            this.radioButton_dataType2.UseVisualStyleBackColor = true;
            // 
            // textBox_savePath
            // 
            this.textBox_savePath.Location = new System.Drawing.Point(318, 93);
            this.textBox_savePath.Name = "textBox_savePath";
            this.textBox_savePath.ReadOnly = true;
            this.textBox_savePath.Size = new System.Drawing.Size(325, 21);
            this.textBox_savePath.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(226, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "File save path";
            // 
            // button_startExport
            // 
            this.button_startExport.Location = new System.Drawing.Point(10, 20);
            this.button_startExport.Name = "button_startExport";
            this.button_startExport.Size = new System.Drawing.Size(75, 23);
            this.button_startExport.TabIndex = 17;
            this.button_startExport.Text = "Start";
            this.button_startExport.UseVisualStyleBackColor = true;
            this.button_startExport.Click += new System.EventHandler(this.button_startExport_Click);
            // 
            // button_pause
            // 
            this.button_pause.Location = new System.Drawing.Point(92, 20);
            this.button_pause.Name = "button_pause";
            this.button_pause.Size = new System.Drawing.Size(75, 23);
            this.button_pause.TabIndex = 18;
            this.button_pause.Text = "Pause";
            this.button_pause.UseVisualStyleBackColor = true;
            this.button_pause.Click += new System.EventHandler(this.button_pause_Click);
            // 
            // button_stop
            // 
            this.button_stop.Location = new System.Drawing.Point(179, 20);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 23);
            this.button_stop.TabIndex = 19;
            this.button_stop.Text = "Stop";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button_stop_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.button_stop);
            this.groupBox5.Controls.Add(this.progressBar1);
            this.groupBox5.Controls.Add(this.button_pause);
            this.groupBox5.Controls.Add(this.label_taskProgress);
            this.groupBox5.Controls.Add(this.button_startExport);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Location = new System.Drawing.Point(11, 482);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(761, 108);
            this.groupBox5.TabIndex = 20;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Data export";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "Current data requiring for a task";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(119, 71);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(635, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // label_taskProgress
            // 
            this.label_taskProgress.AutoSize = true;
            this.label_taskProgress.Location = new System.Drawing.Point(117, 46);
            this.label_taskProgress.Name = "label_taskProgress";
            this.label_taskProgress.Size = new System.Drawing.Size(77, 12);
            this.label_taskProgress.TabIndex = 3;
            this.label_taskProgress.Text = "TaskProgress";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Progress of a task group";
            // 
            // DataExporter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 612);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_reGetToken);
            this.Controls.Add(this.textBox_tokenText);
            this.Controls.Add(this.label_groupProgress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_userName);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 650);
            this.MinimumSize = new System.Drawing.Size(800, 650);
            this.Name = "DataExporter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "API Data Export Tool 1.0";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_pageIndex)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_tokenText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_userName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_reGetToken;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_GetTaskGroup;
        private System.Windows.Forms.ListView listView_taskGroup;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_dataType2;
        private System.Windows.Forms.RadioButton radioButton_dataType;
        private System.Windows.Forms.RadioButton radioButton_Xml;
        private System.Windows.Forms.RadioButton radioButton_Json;
        private System.Windows.Forms.Button button_selectSavePath;
        private System.Windows.Forms.TextBox textBox_savePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_startExport;
        private System.Windows.Forms.Button button_pause;
        private System.Windows.Forms.Button button_stop;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label_groupProgress;
        private System.Windows.Forms.ListView listView_progressInfo;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader TaskGroups;
        private System.Windows.Forms.NumericUpDown numericUpDown_pageSize;
        private System.Windows.Forms.NumericUpDown numericUpDown_pageIndex;
        private System.Windows.Forms.Label label_taskProgress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_groupStop;
        private System.Windows.Forms.ProgressBar progressBarGroupProgress;
        private System.Windows.Forms.Button button_groupPause;
        private System.Windows.Forms.Label label8;
    }
}