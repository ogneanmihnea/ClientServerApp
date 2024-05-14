using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using Service;
using log4net.Config;

namespace Client
{
    public partial class LogIn : Form
    {
        private ICompetitionServices _service;
        public LogIn(ICompetitionServices service)
        {
            this._service = service;
            InitializeComponent();
        }
        
        public string GetConnectionStringByName(string name)
        {
            string returnValue = null;
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        private void logInButton_Click(object sender, EventArgs e)
        {
            string username = usernameField.Text;
            string password = passwordField.Text;
            
            // XmlConfigurator.Configure(new System.IO.FileInfo("D:\\info\\ANUL 2\\MPP\\C#\\ClientServerApp\\Service\\log4net.config"));
            //
            //
            // IDictionary<String, string> props = new SortedList<String, String>();
            //
            // props.Add("ConnectionString", GetConnectionStringByName("competitionDB"));

            var userView = new userView(_service, this);

            try
            {
                var user = _service.Connect(username, password, userView);
                if (user != null)
                {
                    MessageBox.Show("Log in successful!");
                    this.Hide();
                    userView.SetUser(user);
                    //userView userView = new userView(_service,this);
                    // userView.ShowDialog();
                    userView.Show();
                }
                else
                {
                    MessageBox.Show("Log in failed!");
                }
            }
            catch (Exception ex)
            {
                userView.Close();
                MessageBox.Show(ex.Message);
            }

            usernameField.Clear();
            passwordField.Clear();
        }
    }
}