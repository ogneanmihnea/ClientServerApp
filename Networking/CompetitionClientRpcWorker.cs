using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Model;
using Networking.dto;
using Service;

namespace Networking
{
    public class CompetitionClientRpcWorker : ICompetitionObserver
    {
        private ICompetitionServices _server;
        private TcpClient _connection;
        private NetworkStream _stream;
        private BinaryFormatter _formatter;
        private volatile bool _connected;

        public CompetitionClientRpcWorker(ICompetitionServices server, TcpClient connection)
        {
            _server = server;
            _connection = connection;
            try
            {
                _stream = connection.GetStream();
                _formatter = new BinaryFormatter();
                _connected = true;
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        public void Run()
        {
            try
            {
                while (_connected)
                {
                    if (_stream.CanRead && _stream.DataAvailable)
                    {
                        try
                        {
                            var request = _formatter.Deserialize(_stream);
                            var response = HandleRequest((Request)request);
                            if (response != null)
                            {
                                SendResponse((Response)response);
                            }
                        }
                        catch (System.Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                            _connected = false;
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                try
                {
                    _stream.Close();
                    _connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        private Response HandleRequest(Request request)
        {
            Response response = null;
            RequestType requestType = request._type;
            switch (requestType)
            {
                case RequestType.LOGIN:
                    Console.WriteLine("Login request");
                    User user = (User)request._data;
                    try
                    {
                        var userOptional = _server.Connect(user.username, user.password, this);
                        if (userOptional != null)
                        {
                            return new Response.Builder().SetType(ResponseType.OK).SetData(userOptional).Build();
                        }
                        else
                        {
                            _connected = false;
                            return new Response.Builder().SetType(ResponseType.ERROR)
                                .SetData("Invalid username or password").Build();
                        }
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return new Response.Builder().SetType(ResponseType.ERROR).SetData(e.Message).Build();
                    }
                case RequestType.LOGOUT:
                    Console.WriteLine("Logout request");
                    string username = (string)request._data;
                    try
                    {
                        _server.Logout(username);
                        _connected = false;
                        return new Response.Builder().SetType(ResponseType.OK).Build();
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return new Response.Builder().SetType(ResponseType.ERROR).SetData(e.Message).Build();
                    }
                // Add your functionalities here...
                case RequestType.GET_COMPETITIONS:
                    Console.WriteLine("Get competitions request");
                    try
                    {
                        var competitions = _server.GetAllCompetitions();
                        return new Response.Builder().SetType(ResponseType.OK).SetData(competitions).Build();
                    }
                    catch (Exception e)
                    {
                        return new Response.Builder().SetType(ResponseType.ERROR).SetData(e.Message).Build();
                    }
                case RequestType.GET_PARTICIPANTS_FOR_COMPETITION:
                    Console.WriteLine("Get participants by competition request");
                    try
                    {
                        var competitionId = (long)request._data;
                        var participants = _server.GetParticipantsForCompetition(competitionId);
                        return new Response.Builder().SetType(ResponseType.OK).SetData(participants).Build();
                    }
                    catch (Exception e)
                    {
                        return new Response.Builder().SetType(ResponseType.ERROR).SetData(e.Message).Build();
                    }
                case RequestType.REGISTER_PARTICIPANT:
                    Console.WriteLine("Register participant request");
                    try
                    {
                        var participant = (RegisterParticipantDTO)request._data;
                        _server.RegisterParticipant(participant.Name, participant.BirthDate, participant.Proba);
                        return new Response.Builder().SetType(ResponseType.OK).Build();
                    }
                    catch (Exception e)
                    {
                        return new Response.Builder().SetType(ResponseType.ERROR).SetData(e.Message).Build();
                    }
                    
                case RequestType.GET_NR_PARTICIPANTS:
                    Console.WriteLine("Get number of participants request");
                    try
                    {
                        var competition = (Competition)request._data;
                        var nrParticipants = _server.GetNrOfParticipants(competition);
                        return new Response.Builder().SetType(ResponseType.OK).SetData(nrParticipants).Build();
                    }
                    catch (Exception e)
                    {
                        return new Response.Builder().SetType(ResponseType.ERROR).SetData(e.Message).Build();
                    }
                default:
                    return new Response.Builder().SetType(ResponseType.ERROR).SetData("Invalid request").Build();
            }
        }

        private void SendResponse(Response response)
        {
            lock (_stream)
            {
                try
                {
                    if (_connection.Connected && _stream.CanWrite)
                    {
                        _formatter.Serialize(_stream, response);
                        _stream.Flush();
                    }
                    else
                    {
                        Console.WriteLine("Connection is not open or stream is not writable.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        public void registerParticipant()
        {
            Console.WriteLine("Bought tickets");
            Response response = new Response.Builder().SetType(ResponseType.UPDATE).SetData(null).Build();
            SendResponse(response);
        }
    }
}
