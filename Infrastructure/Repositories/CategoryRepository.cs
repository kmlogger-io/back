using System;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class CategoryRepository(KmloggerDbContext context)
    : BaseRepository<Category>(context),
     ICategoryRepository;
