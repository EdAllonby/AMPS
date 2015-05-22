using System;
using System.Collections.Generic;
using Client.Service;
using log4net;
using Shared.Domain;

namespace ConsoleClient
{
    public static class SimulatorUtilities
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (SimulatorUtilities));

        public static void CreateTenFiveMemberBands(List<IClientService> clients)
        {
            const int BandSize = 5;

            for (int bandCount = 0; bandCount < 10; bandCount++)
            {
                Log.Debug("------New Band------");

                IClientService leader = clients[bandCount*BandSize];

                List<int> participantIds = new List<int>();

                for (int bandMemberIndex = bandCount*BandSize; bandMemberIndex < (bandCount*BandSize) + BandSize; bandMemberIndex++)
                {
                    participantIds.Add(clients[bandMemberIndex].ClientUserId);
                }

                foreach (int participantId in participantIds)
                {
                    Log.Debug(participantId);
                }

                CreateBand(leader, participantIds, "$Band" + (bandCount + 1), leader.ClientUserId);
            }
        }

        public static void CreateTwentyTasksPerBand(IEnumerable<IClientService> clients)
        {
            const int TasksToCreate = 4;

            foreach (IClientService client in clients)
            {
                for (int i = 0; i < TasksToCreate; i++)
                {
                    int bandId = 0;

                    if (client.ClientUserId >= 1 && client.ClientUserId <= 5)
                        bandId = 1;
                    if (client.ClientUserId >= 6 && client.ClientUserId <= 10)
                        bandId = 2;
                    if (client.ClientUserId >= 11 && client.ClientUserId <= 15)
                        bandId = 3;
                    if (client.ClientUserId >= 16 && client.ClientUserId <= 20)
                        bandId = 4;
                    if (client.ClientUserId >= 21 && client.ClientUserId <= 25)
                        bandId = 5;
                    if (client.ClientUserId >= 26 && client.ClientUserId <= 30)
                        bandId = 6;
                    if (client.ClientUserId >= 31 && client.ClientUserId <= 35)
                        bandId = 7;
                    if (client.ClientUserId >= 36 && client.ClientUserId <= 40)
                        bandId = 8;
                    if (client.ClientUserId >= 41 && client.ClientUserId <= 45)
                        bandId = 9;
                    if (client.ClientUserId >= 46 && client.ClientUserId <= 50)
                        bandId = 10;

                    if (bandId == 0)
                    {
                        Console.WriteLine("wat!");
                    }
                    else
                    {
                        client.AddTaskToBacklog(bandId, "$task" + i, "$description", 5, client.ClientUserId, TaskCategory.Other);
                    }
                }
            }
        }

        private static void CreateBand(IClientService client, List<int> participantIds, string bandName, int leaderId)
        {
            client.CreateBand(participantIds, bandName, leaderId);
        }
    }
}