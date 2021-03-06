﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Turniejowo.API.Helpers.Factory;
using Turniejowo.API.Helpers.Manager;
using Turniejowo.API.MappingProfiles;
using Turniejowo.API.Repositories;
using Turniejowo.API.Services;
using Turniejowo.API.UnitOfWork;

namespace Turniejowo.API.Installers
{
    public class DIInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IDisciplineRepository, DisciplineRepository>();
            services.AddScoped<IPointsRepository, PointsRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITournamentService, TournamentService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IDisciplineService, DisciplineService>();
            services.AddScoped<IPointsService,PointsService>();
            services.AddScoped<IScheduleGeneratorService, ScheduleGeneratorService>();

            services.AddTransient<IScheduleGeneratorManager, ScheduleGeneratorManager>();
            services.AddTransient<IBracketManager, BracketManager>();
        }
    }
}
