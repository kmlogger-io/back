using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CategoryRepository(KmloggerDbContext context)
    : BaseRepository<Category>(context),
     ICategoryRepository;
