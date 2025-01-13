using System;
using System.Collections.Generic;
using Interface.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context;

public class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DataPointModel> DataPoints { get; set; }

    public virtual DbSet<RouteModel> Routes { get; set; }

    public virtual DbSet<UserModel> Users { get; set; }
    
    public virtual DbSet<FriendModel> Friends { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();
    
}
