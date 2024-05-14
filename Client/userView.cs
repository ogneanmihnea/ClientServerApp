using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Model;
using Service;

namespace Client
{
    public partial class userView : Form, ICompetitionObserver
    {
        private ICompetitionServices _service;
        private LogIn _logIn;
        private User _user;
        
        public userView(ICompetitionServices service, LogIn logIn)
        {
            this._service = service;
            this._logIn = logIn;
            
            InitializeComponent();
        }
        public userView()
        {
            InitializeComponent();
        }
        
        public void SetUser(User user)
        {
            _user = user;
            init();           
        }
        
        private long competitionId;
        
        public void init()
        {
            listCompetitions.Items.Clear();
            IEnumerable<Competition> competitions = _service.GetAllCompetitions();
            foreach (Competition entry in competitions)
            {
                int numberOfParticipants = _service.GetNrOfParticipants(entry);
                listCompetitions.Items.Add(entry.Id + ". | " + entry.trial + " | " + entry.startingAgeInterval + " | " + entry.endingAgeInterval + " | Number of participants: " + numberOfParticipants);
            }
            
            // c.getId() + " | " + c.getTrial() + " | "+ c.getStartingAgeInterval() + " - " + c.getEndingAgeInterval() + " |" + " Number of participants: " + numberOfParticipants

            listCompetitions.SelectionMode = SelectionMode.One;
            listCompetitions.MouseClick += (sender, e) =>
            {
                if (listCompetitions.SelectedItem != null)
                {
                    string selectedCompetitionString = listCompetitions.SelectedItem.ToString();
                    competitionId = long.Parse(selectedCompetitionString.Split('.')[0]); // Extract the selected competition's ID
                    DisplayParticipants(competitionId);
                }
            };
            listCompetitions.SelectionMode = SelectionMode.MultiExtended;
        }
        
        private void DisplayParticipants(long id)
        {
            IEnumerable<Participant> participants = _service.GetParticipantsForCompetition(id).Iterable;
            listParticipants.Items.Clear();
            foreach (Participant entry in participants)
            {
                // int age = DateTime.Now.Year - entry.Key.BirthDate.Year;
                // if (entry.Key.BirthDate > DateTime.Now.AddYears(-age)) age--;
                listParticipants.Items.Add("Name: " + entry.name);
            }
        }
        
        private void logoutButton_Click_1(object sender, EventArgs e)
        {
            _service.Logout(_user.username);
            this.Close();
            _logIn.Show();
        }

        private void registerButton_Click(object sender, EventArgs e)
        {
            string name = nameField.Text;
            DateTime birthDate = birthDateField.Value;
            string proba = probaField.Text;
            if (name=="" || proba=="" || birthDate==null)
            {
                MessageBox.Show("Please fill in all fields!");
                return;
            }
            
            _service.RegisterParticipant(name,birthDate,proba);
            init();
            nameField.Clear();
            birthDateField.Value = DateTime.Now;
        }

        public void registerParticipant()
        {
            if (InvokeRequired)
            {
                Console.WriteLine(_user.username );
                BeginInvoke((MethodInvoker)delegate
                {
                    init();
                    DisplayParticipants(competitionId);
                });
            }
        }
    }
}