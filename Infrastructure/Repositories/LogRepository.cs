using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;
public class LogRepository(KmloggerDbContext context) 
    : BaseRepository<LogApp>(context), ILogRepository;
