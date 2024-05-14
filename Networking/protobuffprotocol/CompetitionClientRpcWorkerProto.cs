using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Model;
using Networking.dto;
using Service;
using Google.Protobuf;

namespace Networking.protobuffprotocol
{
    public class CompetitionClientRpcWorkerProto : ICompetitionObserver
    {
        private ICompetitionServices _server;
        private TcpClient _connection;
        private NetworkStream _stream;
        private volatile bool _connected;
        
        
        public CompetitionClientRpcWorkerProto(ICompetitionServices server, TcpClient connection)
        {
            _server = server;
            _connection = connection;
            try
            {
                _stream = connection.GetStream();
                //_formatter = new BinaryFormatter();
                _connected = true;
            }
            catch (IOException e)
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
                            //var request = _formatter.Deserialize(_stream);
                            Proto.Request request = Proto.Request.Parser.ParseDelimitedFrom(_stream);
                            Proto.Response response = HandleRequest(request);
                            if (response != null)
                            {
                                SendResponse(response);
                            }
                        }
                        catch (Exception e)
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
            finally
            {
                try
                {
                    _stream.Close();
                    _connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error " + e);
                }
            }
        }
        
        
        private Proto.Response HandleRequest(Proto.Request request)
        {
            Proto.Response response = null;
            Proto.Request.Types.RequestType requestType = request.RequestType;
            switch (requestType)
            {
                case Proto.Request.Types.RequestType.Login:
                    Console.WriteLine("Login request");
                    User user = ProtoUtils.GetUser(request);
                    try
                    {
                        User userOptional;
                        
                        userOptional = _server.Connect(user.username, user.password, this);
                        
                        if (userOptional != null)
                        {
                            return ProtoUtils.CreateOkResponse(userOptional);
                        }
                        else
                        {
                            _connected = false;
                            return ProtoUtils.CreateErrorResponse("Invalid username or password");
                        }
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return ProtoUtils.CreateErrorResponse("Authentication failed " + e.Message);
                    }
                case Proto.Request.Types.RequestType.Logout:
                    Console.WriteLine("Logout request");
                    string username = request.Username;
                    try
                    {
                        _server.Logout(username);
                        _connected = false;
                        return ProtoUtils.CreateOkResponse();
                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return ProtoUtils.CreateErrorResponse("Logout failed " + e.Message);
                    }
                case Proto.Request.Types.RequestType.GetCompetitions:
                    Console.WriteLine("Get competitions request");
                    try
                    {
                        return ProtoUtils.CreateGetCompetitionsResponse(_server.GetAllCompetitions());

                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return ProtoUtils.CreateErrorResponse("Error getting competitions " + e.Message);
                    }
                case Proto.Request.Types.RequestType.GetParticipantsByCompetition:
                    Console.WriteLine("Get participants by id request");
                    long id = ProtoUtils.GetId(request);
                    try
                    {
                        return ProtoUtils.CreateGetParticipantsByCompetitionResponse(_server.GetParticipantsForCompetition(id).Iterable);

                    }
                    catch (Exception e)
                    {
                        _connected = false;
                        return ProtoUtils.CreateErrorResponse("Error getting shows by date " + e.Message);
                    }
                case Proto.Request.Types.RequestType.RegisterParticipant:
                    Console.WriteLine("Buy tickets request");
                    RegisterParticipantDTO ticketsDTO = ProtoUtils.GetRegisterParticipantDTO(request);
                    try
                    {
                        
                            _server.RegisterParticipant(ticketsDTO.Name, ticketsDTO.BirthDate, ticketsDTO.Proba);
                            return ProtoUtils.CreateOkResponse();
                    }
                    catch (Exception e)
                    {
                        // _connected = false;
                        return ProtoUtils.CreateErrorResponse("Error registering participant " + e.Message);
                    }
                case Proto.Request.Types.RequestType.GetNrOfParticipants:
                    Console.WriteLine("Get number of participants request");
                    Competition competition = ProtoUtils.GetCompetition(request);
                    competition.Id = ProtoUtils.GetId(request);
                    try
                    {
                        return ProtoUtils.CreateGetNrOfParticipantsResponse(_server.GetNrOfParticipants(competition));
                    }
                    catch (Exception e)
                    {
                        // _connected = false;
                        return ProtoUtils.CreateErrorResponse("Error registering participant " + e.Message);
                    }
                default:
                    return ProtoUtils.CreateErrorResponse("Invalid request type");
            }
        }
        
        
        private void SendResponse(Proto.Response response)
        {
            lock (_stream)
            {
                try
                {
                    response.WriteDelimitedTo(_stream);
                    //_formatter.Serialize(_stream, response);
                    _stream.Flush();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        
        
        
        
        
        
        
        public void registerParticipant()
        {
            Console.WriteLine("registered participant");
            Proto.Response response = ProtoUtils.CreateUpdateResponse();
            SendResponse(response);
        }
    }
}