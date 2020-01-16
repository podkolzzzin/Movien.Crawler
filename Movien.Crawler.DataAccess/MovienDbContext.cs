using Microsoft.EntityFrameworkCore;
using Movien.Crawler.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.DataAccess {

  internal class MovienDbContext : DbContext {

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {

      optionsBuilder.UseSqlite("Data Source=cache.db");
      base.OnConfiguring(optionsBuilder);
    }

    public DbSet<WebPage> Pages { get; set; }
    public DbSet<Header> Headers { get; set; }
    public DbSet<Parameter> Parameters { get; set; }
  }
}
