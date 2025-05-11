using System;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories.Cold;
using Infrastructure.Data;
using Infrastructure.Data.Cold;

namespace Infrastructure.Repositories.Cold;

public class CategoryRepository(ColdDbContext context)
    : BaseRepository<Category>(context),
     ICategoryRepository;
