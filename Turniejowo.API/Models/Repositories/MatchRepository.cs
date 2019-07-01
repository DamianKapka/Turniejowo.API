﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.Models.GenericRepository;

namespace Turniejowo.API.Models.Repositories
{
    public class MatchRepository : Repository<Match>,IMatchRepository
    {
        public MatchRepository(TurniejowoDbContext context) : base(context)
        {

        }
    }
}
