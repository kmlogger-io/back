using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;
public class AppRepository(KmloggerDbContext context) 
: BaseRepository<App>(context), IAppRepository;
