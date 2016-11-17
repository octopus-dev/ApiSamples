using System;
using System.Windows.Forms;

namespace OctopusAPIDataExporter
{
    public partial class GetToken : Form
    {
        public static APIRequester _apiRequester = null;
        public static DataExporter exporter = null;
        public GetToken()
        {
            InitializeComponent();
            string _tokenurl = Properties.Settings.Default["tokenUrl"] as string;
            if(string.IsNullOrEmpty(_tokenurl))
                _tokenurl = "http://localhost:9000/token";
            string _username = Properties.Settings.Default["userName"] as string;
            if (string.IsNullOrEmpty(_username))
                _username = "";
            string _password = Properties.Settings.Default["password"] as string;
            if (string.IsNullOrEmpty(_username))
                _password = "";
            textBox_tokenUrl.Text = _tokenurl;
            textBox_userName.Text = _username;
            textBox_password.Text = _password;
            _apiRequester = new APIRequester();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Continue")
            {
                this.Hide();
                exporter = new DataExporter(_apiRequester);
                DialogResult dr = exporter.ShowDialog();
                if (dr == DialogResult.Abort || dr == DialogResult.Cancel || dr == DialogResult.No)
                    Application.Exit();
                button1.Text = "Get a token";
                this.Show();
                return;
            }
            if (!string.IsNullOrEmpty(textBox_tokenUrl.Text.Trim()) && !string.IsNullOrEmpty(textBox_userName.Text.Trim()) && !string.IsNullOrEmpty(textBox_password.Text.Trim()))
            {
                _apiRequester.AssignUserAndUrls(new APIUser(textBox_userName.Text.Trim(), textBox_password.Text.Trim(), textBox_tokenUrl.Text.Trim()));
                textBox_tokenText.Text = _apiRequester.user.GetToken();
                if (string.IsNullOrEmpty(textBox_tokenText.Text))
                {
                    textBox_tokenText.Text = "Failed to get a token, please check your username or password";
                    return;
                }
                if (null != _apiRequester.user.tokenUrl)
                    _apiRequester.AssignUrlsFromTokenUrl();
                button1.Text = "Continue";
            }
            else
                return;
        }
    }
}
