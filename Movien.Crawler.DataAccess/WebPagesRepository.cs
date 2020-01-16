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

  internal class WebPagesRepository : IWebPagesRepository {

    private readonly MovienDbContext db;

    public WebPagesRepository(MovienDbContext db) {

      this.db = db;
    }

    public async Task Add(WebPage page) {

      await db.Pages.AddAsync(page).ConfigureAwait(false);
    }

    public async Task<IList<WebPage>> Find(Expression<Func<WebPage, bool>> filter) {

      return await db.Pages
        .Include(f => f.Parameters).Include(f => f.Headers)
        .Where(filter).ToListAsync().ConfigureAwait(false);
    }

    public async Task SaveChanges() {

      await db.SaveChangesAsync().ConfigureAwait(false);
    }
  }
}
