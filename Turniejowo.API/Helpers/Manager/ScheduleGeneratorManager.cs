﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public class ScheduleGeneratorManager : IScheduleGeneratorManager
    {
        public async Task<List<DateTime>> GetPossibleMatchDateTimesAsync(GeneratorScheduleOutlines outlines)
        {
            return await Task.Run(() =>
            {

                var startDateTime = DateTime.Parse(outlines.StartDate);
                var endDateTime = DateTime.Parse(outlines.EndDate);

                var list = new List<DateTime>();

                while (startDateTime <= endDateTime)
                {
                    {
                        if (outlines.DaysOfWeek.Contains((int)startDateTime.DayOfWeek))
                        {
                            for (int i = int.Parse(outlines.StartTime.Split(':')[0]);
                                i <= int.Parse(outlines.EndTime.Split(':')[0]);
                                i++)
                            {
                                for (int j = 0; j < outlines.SimultaneousMatches; j++)
                                {
                                    list.Add(new DateTime(startDateTime.Year, startDateTime.Month, startDateTime.Day, i,
                                        0, 0));
                                }
                            }
                        }
                    }
                    startDateTime = startDateTime.AddDays(1);
                }

                return list;
            });
        }

        public async Task<Dictionary<int, List<int>>> GetPossibleMatchMatrix(List<Team> teams)
        {
            return await Task.Run(() =>
            {
                var matchDict = new Dictionary<int, List<int>>();

                var teamIdArray = teams.Select(t => t.TeamId).ToArray();

                for (int i = 0; i < teamIdArray.Length - 1; i++)
                {
                    var opponents = new List<int>();

                    for (int j = i + 1; j < teamIdArray.Length; j++)
                    {
                        opponents.Add(teamIdArray[j]);
                    }

                    matchDict.Add(teamIdArray[i], opponents);
                }

                return matchDict;
            });
        }

        public async Task<List<Match>> GenerateSchedule(bool isBracket, List<DateTime> dateTimes, Dictionary<int, List<int>> matchMatrix)
        {
            return await Task.Run(() =>
            {
                if (isBracket)
                {
                    var matchList = new List<Match>();

                    for (int i = 0; i < matchMatrix.Count; i++)
                    {
                        var valueListLength = matchMatrix.ElementAt(i).Value.Count;

                        var rnd = new Random((int) DateTime.Now.Ticks % 327 * i + 1);
                        var rndOpponentRange = rnd.Next(valueListLength);

                        var randomizedOpponent = matchMatrix.ElementAt(i).Value[rndOpponentRange];

                        rnd = new Random((int) DateTime.Now.Ticks % 150 * i + 1);
                        var rndDate = rnd.Next(dateTimes.Count);

                        var date = dateTimes[rndDate];

                        matchList.Add(new Match
                        {
                            MatchDateTime = date,
                            HomeTeamId = matchMatrix.ElementAt(i).Key,
                            GuestTeamId = randomizedOpponent,
                            IsFinished = false,
                            BracketIndex = i + 1
                        });

                        var kv = matchMatrix.FirstOrDefault(m => m.Key == randomizedOpponent);
                        matchMatrix.Remove(kv.Key);

                        for (int j = i + 1; j < matchMatrix.Count; j++)
                        {
                            int? opponentDuplicate = matchMatrix.ElementAt(j).Value
                                .FirstOrDefault(m => m == randomizedOpponent);

                            if (opponentDuplicate != null)
                            {
                                matchMatrix.ElementAt(j).Value.Remove((int) (opponentDuplicate));
                            }
                        }

                        dateTimes.Remove(date);
                    }

                    return matchList;
                }
                else
                {
                    return null;
                }
            });
        }
    }
}
