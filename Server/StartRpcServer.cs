

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Sockets;
using System.Threading;
using log4net.Config;
using Networking;
using Networking.protobuffprotocol;
using Networking.utils;
using Persistence;
using Service;

namespace Server
{
    public class StartRpcServer
    {
        public static void Main(string[] args)
        {
            IUserRepository userRepository;
            ICompetitionRepository competitionRepository;
            IParticipantRepository participantRepository;
            IRegistrationRepository registrationRepository;
            
            
            XmlConfigurator.Configure(new System.IO.FileInfo("App.config"));
            
            IDictionary<string, string> props = new SortedList<String, String>();
            
            props.Add("ConnectionString", GetConnectionStringByName("competitionDB"));
            
            userRepository = new UserDbRepository(props);
            competitionRepository = new CompetitionDbRepository(props);
            participantRepository = new ParticipantDbRepository(props);
            registrationRepository = new RegistrationDbRepository(props);
            
            ICompetitionServices competitionServices = new CompetitionServices(userRepository, competitionRepository, participantRepository, registrationRepository);
            SerialServerProto server=new SerialServerProto("127.0.0.1",55555,competitionServices);
            // SerialServer server=new SerialServer("127.0.0.1",44444,competitionServices);
            server.Start();
            Console.WriteLine("Server started...");
            Console.ReadLine();
            
        }
        
        
        
        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
        
        // public class SerialServer : ConcurrentServer
        // {
        //     private ICompetitionServices server;
        //     
        //     private CompetitionClientRpcWorkerProto worker;
        //
        //     public SerialServer(string host, int port, ICompetitionServices server) : base(host, port)
        //     {
        //         this.server = server;
        //         Console.WriteLine("SerialServer...");
        //     }
        //
        //     protected override Thread createWorker(TcpClient client)
        //     {
        //         // worker = new CompetitionClientRpcWorker(server, client);
        //         worker = new CompetitionClientRpcWorkerProto(server, client);
        //         return new Thread(new ThreadStart(worker.Run));
        //     }
        // }
        
        public class SerialServerProto : ConcurrentServer
        {
            private ICompetitionServices server;
            private CompetitionClientRpcWorkerProto worker;
        
            public SerialServerProto(string host, int port, ICompetitionServices server) : base(host, port)
            {
                this.server = server;
                Console.WriteLine("SerialServerProto...");
            }
        
            protected override Thread createWorker(TcpClient client)
            {
                worker = new CompetitionClientRpcWorkerProto(server, client);
                return new Thread(new ThreadStart(worker.Run));
            }
        }
    }
}