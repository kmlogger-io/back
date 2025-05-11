using System;
using Domain.Entities;
using Domain.Interfaces.Repositories.Hot;
using Infrastructure.Data.Hot;

namespace Infrastructure.Repositories.Hot;

public class LogRepository(HotDbContext context) 
    : BaseRepository<LogApp>(context), ILogRepository;
