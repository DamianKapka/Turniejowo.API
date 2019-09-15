﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Turniejowo.API.GenericRepository;
using Turniejowo.API.Models;

namespace Turniejowo.API.Repositories
{
    public class DisciplineRepository : Repository<Discipline>,IDisciplineRepository
    {
        public DisciplineRepository(TurniejowoDbContext context) : base(context)
        {
            
        }
    }
}
