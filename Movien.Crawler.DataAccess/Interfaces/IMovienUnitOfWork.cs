using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movien.Crawler.DataAccess.Interfaces {

  public interface IMovienUnitOfWork : IDisposable {

    public Task Setup();

    public IWebPagesRepository WebPages { get; }
  }
}
