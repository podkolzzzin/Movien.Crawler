using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.Infrostructure {
  public interface IEventQueue {

    IDisposable Subscribe<T>(Action<T> handler);

    void Publish<T>(T @event);
  }
}
