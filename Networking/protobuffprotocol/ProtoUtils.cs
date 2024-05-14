
using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using Networking.dto;
using User=Model.User;


namespace Networking.protobuffprotocol
{
    public class ProtoUtils
    {
        
        public static User GetUser(Proto.Request request)
        {
            User user = new User(request.User.Username, request.User.Password);
            return user;
        }
        
        
        public static Proto.Response CreateOkResponse()
        {
            Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.Ok };
            return response;
        }
        
        public static Proto.Response CreateOkResponse(User user)
        {
            Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.Ok };
            Proto.User protoUser = new Proto.User { Username = user.username, Password = user.password };
            response.User = protoUser;
            return response;
        }
        
        public static Proto.Response CreateErrorResponse(string message)
        {
            Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.Error, Error = message };
            return response;
        }
        
        public static Proto.Response CreateUpdateResponse()
        {
            Proto.Response response = new Proto.Response{ResponseType = Proto.Response.Types.ResponseType.Update};
            return response;
        }
        public static Proto.Response CreateGetCompetitionsResponse(IEnumerable<Competition> competitions)
        {
            Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.GetCompetitions };
            Proto.AllCompetitionsDTO allCompetitionsDTO = new Proto.AllCompetitionsDTO();
            foreach (var competition in competitions)
            {
                Proto.Competition competitionDTO = new Proto.Competition
                {
                    Trial = competition.trial,
                    StartingAgeInterval = competition.startingAgeInterval,
                    EndingAgeInterval = competition.endingAgeInterval

                };

                Proto.CompetitionEntry competitionEntry = new Proto.CompetitionEntry
                {
                    Key = competitionDTO,
                    Value = (int)competition.Id
                };

                allCompetitionsDTO.Competitions.Add(competitionEntry);
            }
            response.AllCompetitions=allCompetitionsDTO;
            return response;
            
            // Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.GetCompetitions };
            // Proto.AllCompetitionsDTO allCompetitionsDTO = new Proto.AllCompetitionsDTO();
            // foreach (var competition in competitions.getCompetitions().Keys)
            // {
            //     
            //     Proto.Competition competitionDTO = new Proto.Competition
            //     {
            //         Id = (int)competition.Id,
            //         Style = competition.Style,
            //         Distance = competition.Distance
            //
            //     };
            //
            //     Proto.CompetitionEntry competitionEntry = new Proto.CompetitionEntry
            //     {
            //         Key = competitionDTO,
            //         Value = competitions.getCompetitions()[competition]
            //     };
            //
            //     allCompetitionsDTO.Competitions.Add(competitionEntry);
            // }
            // response.AllCompetitions=allCompetitionsDTO;
            // return response;
        }
        
        
        public static Proto.Response CreateGetNrOfParticipantsResponse(int nrOfCompetitions)
        {
            Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.GetNrOfParticipants };
            response.NoParticipants = nrOfCompetitions;
            return response;
        }
        
        public static Proto.Response CreateGetParticipantsByCompetitionResponse(IEnumerable <Participant> participants)
        {
            Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.GetParticipantsForCompetition };
            
            foreach (var participant in participants)
            {
                Proto.Participant pariticipantdto = new Proto.Participant
                {
                    Name = participant.name,
                    BirthDate = participant.birthDate.ToString()
                };
                Proto.ParticipantEntry participantEntryBuilder = new Proto.ParticipantEntry();
                participantEntryBuilder.Participant=pariticipantdto;
                response.Participants.Add(participantEntryBuilder);
            }

            return response;
        }
        // {
        //     Proto.Response response = new Proto.Response { ResponseType = Proto.Response.Types.ResponseType.GetParticipantsByCompetition };
        //
        //     foreach (var participant in participants.Keys)
        //     {
        //         Proto.Participant pariticipantdto = new Proto.Participant
        //         {
        //             Name = participant.Name,
        //             BirthDate = participant.BirthDate.ToString()
        //         };
        //         Proto.ParticipantEntry participantEntryBuilder = new Proto.ParticipantEntry
        //         {
        //             // Participant = pariticipantdto
        //         };
        //         
        //         participantEntryBuilder.Participant=pariticipantdto;
        //
        //         foreach (var id in participants[participant])
        //         {
        //             participantEntryBuilder.Competitions.Add((int)id);
        //         }
        //
        //         response.Participants.Add(participantEntryBuilder);
        //     }
        //
        //     return response;
        
        
        
        
        
        
        
        
        public static RegisterParticipantDTO GetRegisterParticipantDTO(Proto.Request request)
        {
            var register = request.RegisterParticipant;
            List<long> competitionsId = new List<long>();
            var proba = request.RegisterParticipant.Proba;
            return new RegisterParticipantDTO(register.Name, DateTime.Parse(register.BirthDate), proba);
        }
        
        public static Competition GetCompetition(Proto.Request request)
        {
            var competition = request.Competition;
            return new Competition(competition.Trial, competition.StartingAgeInterval, competition.EndingAgeInterval);
        }
        
        public static long GetId(Proto.Request request)
        {
            return  (long) request.CompetitionId;
        } 
        
        
    }
}