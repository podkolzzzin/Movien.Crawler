using Movien.Crawler.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Movien.Crawler.Infrostructure {

  public interface ICache {
    
    Task Save(ContentLoadedEvent evt);

    Task<ContentLoadedEvent?> Retrieve(LoadPageEvent evt);
  }
}
