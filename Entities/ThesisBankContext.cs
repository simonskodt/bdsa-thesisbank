﻿using System;
using Microsoft.EntityFrameworkCore;

namespace Entities;

public class ThesisBankContext : DbContext, IThesisBankContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Thesis> Theses => Set<Thesis>();
    public DbSet<Apply> Applies => Set<Apply>();

    public ThesisBankContext(DbContextOptions<ThesisBankContext> options) : base(options) { }


    // protected override void OnModelCreating(ModelBuilder modelBuilder) { }

    public static void Seed(ThesisBankContext context)
    {
        context.Database.EnsureCreated();
        context.Database.ExecuteSqlRaw("DELETE dbo.Applies");
        context.Database.ExecuteSqlRaw("DELETE dbo.Students");
        context.Database.ExecuteSqlRaw("DELETE dbo.Teachers");
        context.Database.ExecuteSqlRaw("DELETE dbo.Theses");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Applies', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Students', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Teachers', RESEED, 0)");
        context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('dbo.Theses', RESEED, 0)");

        var ahmed = new Student("Ahmed Galal", "ahmed@itu.dk");
        var leonora = new Student("Léonora Théorêt", "leonora@itu.dk");
        var alyson = new Student("Alyson D'Souza", "alyson@itu.dk");
        var victor = new Student("Victor Brorson", "victor@itu.dk");
        var simon = new Student("Simon Skødt", "simon@itu.dk");

        var thore = new Teacher("Thore", "thore@itu.dk");
        var rasmus = new Teacher("Rasmus", "rasmus@itu.dk");

        var thesis1 = new Thesis("How ITU mentally ruin students", thore);
        var thesis2 = new Thesis("Why singletons are an anti-pattern", rasmus);
        var thesis3 = new Thesis("A study on why notepad is the best IDE", thore);

        var applies1 = new Apply(thesis1, ahmed);
        var applies2 = new Apply(thesis2, leonora) { Status = Status.Denied };
        var applies3 = new Apply(thesis2, simon);
        var applies4 = new Apply(thesis2, alyson) { Status = Status.Denied };
        var applies5 = new Apply(thesis2, victor) { Status = Status.Denied };

        context.Teachers.AddRange(
           rasmus,
           thore
       );

        context.Theses.AddRange(
           thesis1,
           thesis2,
           thesis3
       );

        context.Students.AddRange(
            ahmed,
            leonora,
            alyson,
            victor,
            simon
        );

        context.Applies.AddRange(
            applies1,
            applies2,
            applies3,
            applies4,
            applies5
        );

        context.SaveChangesAsync();
    }
}