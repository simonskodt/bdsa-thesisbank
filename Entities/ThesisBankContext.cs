
﻿﻿using System;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ThesisBankContext : DbContext, IThesisBankContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Thesis> Theses => Set<Thesis>();
    public DbSet<Apply> Applies => Set<Apply>();

    public ThesisBankContext(DbContextOptions<ThesisBankContext> options) : base(options) { }

}
