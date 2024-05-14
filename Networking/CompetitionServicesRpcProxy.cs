using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Model;
using Networking.dto;
using Service;

namespace Networking
{
    public class CompetitionServicesRpcProxy: ICompetitionServices
    {
        private string host;
        private int port;
        private ICompetitionObserver client;
        private TcpClient connection;
        private NetworkStream stream;
        private IFormatter formatter;
        private volatile bool finished;
        private BlockingCollection<Response> queueResponses;

        public CompetitionServicesRpcProxy(string host, int port)
        {
            this.host = host;
            this.port = port;
            this.formatter = new BinaryFormatter();
            queueResponses = new BlockingCollection<Response>();
        }

        private void InitializeConnection()
        {
            try
            {
                connection = new TcpClient(host, port);
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                finished = false;
                StartReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private void StartReader()
        {
            Thread tw = new Thread(Run);
            tw.Start();
        }

        public virtual void Run()
        {
            while (!finished)
            {
                try
                {
                    if (stream.CanRead && stream.DataAvailable)
                    {
                        Response response = (Response)formatter.Deserialize(stream);
                        Console.WriteLine("Response received " + response);
                        if (IsUpdate(response))
                        {
                            HandleUpdate(response);
                        }
                        else
                        {
                            queueResponses.Add(response);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    CloseConnection();
                    finished = true;
                }
            }
        }

        private void CloseConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                connection.Close();
                client = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public User Connect(string username, string password, ICompetitionObserver client)
        {
            InitializeConnection();
            User user = new User(username, password);
            Request request = new Request.Builder().Type(RequestType.LOGIN).Data(user).Build();
            SendRequest(request);
            Response response = ReadResponse();
            if (response.type == ResponseType.OK)
            {
                this.client = client;
                // return (User) response.data;
                return new User(username, password);
            }
            if (response.type == ResponseType.ERROR)
            {
                string message = (string)response.data;
                CloseConnection();
                throw new Exception(message);
            }

            return null;
        }

        private Response ReadResponse()
        {
            Response response = null;
            try
            {
                response = queueResponses.Take();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            return response;
        }

        private void SendRequest(Request request)
        {

            try
            {
                formatter.Serialize(stream, request);
                stream.Flush();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Logout(string username)
        {
            Request request = new Request.Builder().Type(RequestType.LOGOUT).Data(username).Build();
            SendRequest(request);
            Response response = ReadResponse();
            if (response.type == ResponseType.ERROR)
            {
                String message = (String)response.data;
                throw new Exception(message);
            }

        }
        
        public void RegisterParticipant(string name, DateTime birthDate, string proba)
        {
            RegisterParticipantDTO participant = new RegisterParticipantDTO(name, birthDate, proba);
            Request request = new Request.Builder().Type(RequestType.REGISTER_PARTICIPANT).Data(participant).Build();
            SendRequest(request);
            Response response = ReadResponse();
            if (response.type == ResponseType.ERROR)
            {
                string error = (string)response.data;
                throw new ArgumentException(error);
            }
        }
        
        public int GetNrOfParticipants(Competition competition)
        {
            Request request = new Request.Builder().Type(RequestType.GET_NR_PARTICIPANTS).Data(competition).Build();
            SendRequest(request);
            Response response = ReadResponse();
            if (response.type == ResponseType.ERROR)
            {
                string error = (string)response.data;
                throw new ArgumentException(error);
            }
            return (int)response.data;
        }

        public IEnumerable<User> GetAllUsers()
        {
            // Return an empty enumerable
            return Enumerable.Empty<User>();
        }
        
        public IEnumerable<Competition> GetAllCompetitions()
        {
            Request request = new Request.Builder().Type(RequestType.GET_COMPETITIONS).Build();
            SendRequest(request);
            Response response = ReadResponse();
            if (response.type == ResponseType.ERROR)
            {
                string error = (string)response.data;
                throw new ArgumentException(error);
            }
            return (IEnumerable<Competition>) response.data;
        }
        
        public IterableDTO GetParticipantsForCompetition(long competitionId)
        {
            Request request = new Request.Builder().Type(RequestType.GET_PARTICIPANTS_FOR_COMPETITION).Data(competitionId).Build();
            SendRequest(request);
            Response response = ReadResponse();
            if (response.type == ResponseType.ERROR)
            {
                string error = (string)response.data;
                throw new ArgumentException(error);
            }
            return (IterableDTO) response.data;
        }
        
        
        private bool IsUpdate(Response response)
        {
            return response.type == ResponseType.UPDATE;
        }

        private void HandleUpdate(Response response)
        {
            if (response.type == ResponseType.UPDATE)
            {
                client.registerParticipant();
            }
        }
    }
}