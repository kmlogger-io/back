using System;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public sealed class RoleRepository(KmloggerDbContext context)
    : BaseRepository<Role>(context),
     IRoleRepository;