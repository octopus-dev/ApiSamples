using System;
using System.Threading;
using System.Windows.Forms;

namespace OctopusAPIDataExporter
{
    public delegate void ControlChanger(string progressText);
    public delegate void ProgressChanger(int current, int total);
    public partial class DataExporter : Form
    {
        public void ChangeGroupProgressText(string text)
        {
            if (label_groupProgress.InvokeRequired)
            {
                ControlChanger outdelegate = new ControlChanger(ChangeGroupProgressText);
                this.BeginInvoke(outdelegate, new object[] { text });
                return;
            }
            label_groupProgress.Text = text;
        }
        public void ChangeTaskProgressText(string text)
        {
            if (label_groupProgress.InvokeRequired)
            {
                ControlChanger outdelegate = new ControlChanger(ChangeTaskProgressText);
                this.BeginInvoke(outdelegate, new object[] { text });
                return;
            }
            label_taskProgress.Text = text;
        }

        public void AddGroupsIntoListView(string text)
        {
            if (listView_taskGroup.InvokeRequired)
            {
                ControlChanger outdelegate = new ControlChanger(AddGroupsIntoListView);
                this.BeginInvoke(outdelegate, new object[] { text });
                return;
            }
            listView_taskGroup.Items.Add(new ListViewItem(text));
        }

        public void AddProgressIntoListView(string text)
        {
            if (listView_progressInfo.InvokeRequired)
            {
                ControlChanger outdelegate = new ControlChanger(AddProgressIntoListView);
                this.BeginInvoke(outdelegate, new object[] { text });
                return;
            }
            listView_progressInfo.Items.Add(new ListViewItem(text));
        }

        public void SetProgressBarValue(int current, int total)
        {
            if (progressBar1.InvokeRequired)
            {
                ProgressChanger outdelegate = new ProgressChanger(SetProgressBarValue);
                this.BeginInvoke(outdelegate, new object[] { current, total });
                return;
            }
            progressBar1.Value = (int)(100 * current / total);
        }

        public void SetGroupProgressBarValue(int current, int total)
        {
            if (progressBarGroupProgress.InvokeRequired)
            {
                ProgressChanger outdelegate = new ProgressChanger(SetGroupProgressBarValue);
                this.BeginInvoke(outdelegate, new object[] { current, total });
                return;
            }
            progressBarGroupProgress.Value = (int)(100 * current / total);
        }

        public static GetToken TokenWindow = null;
        public static APIRequester _apiRequester = null;
        public DataExporter(APIRequester apiRequester)
        {
            _apiRequester = apiRequester;
            InitializeComponent();
            textBox_tokenText.Text = _apiRequester.user.token;
            textBox_userName.Text = _apiRequester.user.userName;
            _apiRequester.DelegGroupProgressTextChange = new ControlChanger(ChangeGroupProgressText);
            _apiRequester.DelegTaskProgressTextChange = new ControlChanger(ChangeTaskProgressText);
            _apiRequester.DelegAddGroupIntoListView = new ControlChanger(AddGroupsIntoListView);
            _apiRequester.DelegAddProgressInfoIntoListView = new ControlChanger(AddProgressIntoListView);
            _apiRequester.DelegProgressChange = new ProgressChanger(SetProgressBarValue);
            _apiRequester.DelegGroupProgressChange = new ProgressChanger(SetGroupProgressBarValue);
            textBox_savePath.Text = System.Environment.CurrentDirectory;
            label_groupProgress.Text = "";
            label_taskProgress.Text = "";
        }

        private void button_reGetToken_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Hide();
        }

        public Thread taskgroupThread = null;
        private void button_GetTaskGroup_Click(object sender, EventArgs e)
        {
            button_GetTaskGroup.Enabled = false;
            listView_taskGroup.CheckBoxes = true;
            listView_taskGroup.Items.Clear();
            _apiRequester.user.taskGroups = null;
            listView_progressInfo.Items.Clear();
            SetGroupProgressBarValue(0, 100);
            taskgroupThread = new Thread(new ThreadStart(_apiRequester.GetTaskGroups));
            taskgroupThread.IsBackground = true;
            taskgroupThread.Start();
        }

        public Thread GetCheckedGroupAndTaskThread = null;

        private void button_startExport_Click(object sender, EventArgs e)
        {
            APIRequester.TaskDataConfig config = new APIRequester.TaskDataConfig() { 
                pageIndex = (int)numericUpDown_pageIndex.Value, 
                pageSize = (int)numericUpDown_pageSize.Value }; ;
            if (radioButton_dataType.Checked)
                config.dataType = 0;
            if (radioButton_dataType2.Checked)
                config.dataType = 1;
            config.savePath = textBox_savePath.Text;
            if (listView_taskGroup.Items.Count > 0)
            {
                button_startExport.Enabled = false;
                listView_taskGroup.Enabled = false;
                SetProgressBarValue(0, 100);
                GetCheckedGroupAndTaskThread = new Thread(new ParameterizedThreadStart(GetCheckedGroupAndTask));
                GetCheckedGroupAndTaskThread.Start(config);
            }
        }

        private void GetCheckedGroupAndTask(object _config)
        {
            APIRequester.TaskDataConfig config = _config as APIRequester.TaskDataConfig;
            try
            {
                foreach (ListViewItem item in listView_taskGroup.Items)
                {
                    if (item.Checked)
                    {
                        config.groupIndex = item.Index;
                        _apiRequester.GetDataByGroupAndSave(config);
                    }
                }
            }
            catch (Exception e)
            {

            }
            button_startExport.Enabled = true;
            listView_taskGroup.Enabled = true;

        }


        private void button_pause_Click(object sender, EventArgs e)
        {
            if (null != GetCheckedGroupAndTaskThread)
            {
                if (button_pause.Text == "Continue")
                {
                    button_pause.Text = "Pause";
                    GetCheckedGroupAndTaskThread.Resume();

                }
                else
                {
                    GetCheckedGroupAndTaskThread.Suspend();
                    button_pause.Text = "Continue";
                }
            }
        }

        private void button_stop_Click(object sender, EventArgs e)
        {
            if (null != GetCheckedGroupAndTaskThread)
            {
                if (GetCheckedGroupAndTaskThread.ThreadState == ThreadState.Running)
                    GetCheckedGroupAndTaskThread.Abort();
                button_startExport.Text = "Restart";
                button_startExport.Enabled = true;
                listView_taskGroup.Enabled = true;
                GetCheckedGroupAndTaskThread = null;
            }
        }

        private void button_selectSavePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Please choose a folder";
            if (dialog.ShowDialog() == DialogResult.OK || dialog.ShowDialog() == DialogResult.Yes)
            {
                textBox_savePath.Text = dialog.SelectedPath;
            }
        }

        private void button_groupPause_Click(object sender, EventArgs e)
        {
            if (null != taskgroupThread)
            {
                if (button_groupPause.Text == "Continue")
                {
                    button_groupPause.Text = "Pause";
                    taskgroupThread.Resume();
                }
                else
                {
                    taskgroupThread.Suspend();
                    button_groupPause.Text = "Continue";
                }
            }
        }

        private void button_groupStop_Click(object sender, EventArgs e)
        {
            if (null != taskgroupThread)
            {
                if (taskgroupThread.ThreadState == ThreadState.Suspended)
                    taskgroupThread.Resume();
                if (taskgroupThread.ThreadState == ThreadState.Running)
                    taskgroupThread.Abort();
                button_GetTaskGroup.Text = "Restart";
                button_GetTaskGroup.Enabled = true;
                taskgroupThread = null;
            }
        }

        ~DataExporter()
        {
            button_groupStop_Click(null, null);
            button_stop_Click(null, null);
            Environment.Exit(0);
        }
    }
}
