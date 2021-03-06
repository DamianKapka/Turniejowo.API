﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.EntityFrameworkCore.Internal;
using Turniejowo.API.Contracts.Responses;
using Turniejowo.API.Models;

namespace Turniejowo.API.Helpers.Manager
{
    public class BracketManager : IBracketManager
    {
        private IMapper mapper;

        public BracketManager(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<BracketData> FillInBracketWithDataAsync(BracketData data, List<Match> matches)
        {
            return await Task.Run(() =>
            {
                int[] eachRoundMatchQty = ComputeMatchQtyForTournamentRounds(data.Rounds.Count,data.NumberOfTeams);

                int[] eachRoundMaxIndex = ComputeRoundMaxIndex(eachRoundMatchQty,data.NumberOfTeams);

                data.Rounds.ForEach(r => r.Matches = new List<MatchResponse>());

                for (int i = 1; i <= data.NumberOfTeams; i++)
                {
                    foreach (var t in eachRoundMaxIndex)
                    {
                        if(i > t){ continue;}

                        var match = matches.FirstOrDefault(m => m.BracketIndex == i);

                        if (match == null)
                        {
                            data.Rounds[eachRoundMaxIndex.IndexOf(t)].Matches.Add(new MatchResponse());
                            break;
                        }

                        data.Rounds[eachRoundMaxIndex.IndexOf(t)].Matches.Add(mapper.Map<MatchResponse>(match));
                        break;
                    }
                }

                return data;
            });
        }

        public async Task<BracketData> FillInBracketWithBlankDataAsync(BracketData data)
        {
            return await Task.Run(() =>
            {
                for (int i = 1; i <= data.Rounds.Count; i++)
                {
                    var numOfIterationDivide = (int) Math.Pow(2, i);
                    var iterationQty = data.NumberOfTeams / numOfIterationDivide;
                    data.Rounds[i - 1].Matches = new List<MatchResponse>();
                    Parallel.For(0, iterationQty,
                        (index) => { data.Rounds[i - 1].Matches.Add(new MatchResponse());});
                }

                return data;
            });
        }

        public Task<int> FindFirstEmptyBracketSlotAsync(ICollection<Match> matches, int teamsQty)
        {
            return Task.Run(() =>
            {
                for (int i = 1; i < teamsQty; i++)
                {
                    if (matches.FirstOrDefault(m => m.BracketIndex == i) == null)
                    {
                        return i;
                    }
                }

                throw new ArgumentOutOfRangeException();
            });
        }

        public Task<Match> AutoGenerateBracketMatchAsync(Match correspondingBracketMatch, Match currentlyAddedMatch, int tournamentParticipantsQty)
        {
            /*
             * TODO AUTOGENERATEDBRACKET MATCH BUILDER Z INTERFACEM DO TYCH METOD NA DOLE
             */

            return Task.Run(() =>
            {

                var autoGeneratedMatch = new Match
                {
                    MatchDateTime = new DateTime(2000,1,1),
                    IsFinished = false
                };

                if (correspondingBracketMatch.BracketIndex > currentlyAddedMatch.BracketIndex)
                {
                    autoGeneratedMatch.HomeTeamId =
                        currentlyAddedMatch.GuestTeamPoints > currentlyAddedMatch.HomeTeamPoints
                            ? currentlyAddedMatch.GuestTeamId
                            : currentlyAddedMatch.HomeTeamId;

                    autoGeneratedMatch.GuestTeamId =
                        correspondingBracketMatch.GuestTeamPoints > correspondingBracketMatch.HomeTeamPoints
                            ? correspondingBracketMatch.GuestTeamId
                            : correspondingBracketMatch.HomeTeamId;

                    autoGeneratedMatch.BracketIndex = ComputeAutoGeneratedBracketIndex(tournamentParticipantsQty, correspondingBracketMatch.BracketIndex);
                }
                else
                {
                    autoGeneratedMatch.GuestTeamId =
                        currentlyAddedMatch.GuestTeamPoints > currentlyAddedMatch.HomeTeamPoints
                            ? currentlyAddedMatch.GuestTeamId
                            : currentlyAddedMatch.HomeTeamId;

                    autoGeneratedMatch.HomeTeamId =
                        correspondingBracketMatch.GuestTeamPoints > correspondingBracketMatch.HomeTeamPoints
                            ? correspondingBracketMatch.GuestTeamId
                            : correspondingBracketMatch.HomeTeamId;

                    autoGeneratedMatch.BracketIndex = ComputeAutoGeneratedBracketIndex(tournamentParticipantsQty, currentlyAddedMatch.BracketIndex);
                }

                return autoGeneratedMatch;
            });
        }

        /*
         * TODO: DO OSOBNEGO INTERFACEU + TESTY
         */
        private int[] ComputeMatchQtyForTournamentRounds(int roundsQty, int teamQty)
        {
            int[] roundMatchesQty = new int[roundsQty];

            for (int i = 0; i < roundsQty; i++)
            {
                roundMatchesQty[i] = (int)(teamQty / Math.Pow(2, i + 1));
            }

            return roundMatchesQty;
        }

        /*
         * TODO: DO OSOBNEGO INTERFACEU + TESTY
         */
        private int[] ComputeRoundMaxIndex(int[] eachRoundMatchQty, int teamQty)
        {
            var roundMaxIndexArray = new int[eachRoundMatchQty.Length];

            for (int j = 0; j < eachRoundMatchQty.Length; j++)
            {
                int max = 0;

                for (int i = 0; i < j+1; i++)
                {
                    max += eachRoundMatchQty[i];
                }

                roundMaxIndexArray[j] = max;
            }

            return roundMaxIndexArray;
        }

        private int ComputeAutoGeneratedBracketIndex(int tournamentParticipantsQty, int childMatchBracketIndex)
        {
            return (tournamentParticipantsQty + childMatchBracketIndex) / 2;
        }
    }
}
