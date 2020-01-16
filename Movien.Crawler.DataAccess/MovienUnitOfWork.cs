using Microsoft.EntityFrameworkCore;
using Movien.Crawler.DataAccess.Interfaces;
using Movien.Crawler.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Movien.Crawler.DataAccess {

  public class MovienUnitOfWork : IMovienUnitOfWork {

    private readonly MovienDbContext db;

    public IWebPagesRepository WebPages { get; }

    public MovienUnitOfWork() {

      db = new MovienDbContext();
      WebPages = new WebPagesRepository(db);
    }

    public async Task Setup() {

      await db.Database.MigrateAsync().ConfigureAwait(false);
    }

    public void Dispose() {
      
      db.Dispose();
    }
  }
}
