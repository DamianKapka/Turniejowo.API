﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<BracketData> FillInBracketWithData(BracketData data, List<Match> matches)
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

        public async Task<BracketData> FillInBracketWithBlankData(BracketData data)
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
    }
}
