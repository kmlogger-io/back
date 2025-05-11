using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Cold;
using Infrastructure.Data;
using Infrastructure.Data.Cold;

namespace Infrastructure.Repositories.Cold;
public class LogRepository(ColdDbContext context) 
    : BaseRepository<LogApp>(context), ILogRepository;
