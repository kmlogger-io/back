using Domain.Entities;
using Domain.Interfaces.Repositories.Cold;
using Infrastructure.Data.Cold;

namespace Infrastructure.Repositories.Cold;
public class AppRepository(ColdDbContext context) 
: BaseRepository<App>(context), IAppRepository;
