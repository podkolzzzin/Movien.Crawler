using Movien.Crawler.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Movien.Crawler.DataAccess.Interfaces {

  public interface IWebPagesRepository {

    Task Add(WebPage page);

    Task<IList<WebPage>> Find(Expression<Func<WebPage, bool>> filter);

    Task SaveChanges();
  }
}