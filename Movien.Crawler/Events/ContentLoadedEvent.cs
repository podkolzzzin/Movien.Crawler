using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.Events {

  public readonly struct ContentLoadedEvent {

    public LoadPageEvent RequestEvent { get; }

    public string Content { get; }

    public DateTime LoadedAt { get; }

    public ContentLoadedEvent(LoadPageEvent request, string content, DateTime loadedAt) 
      => (RequestEvent, Content, LoadedAt) = (request, content, loadedAt);
  }
}
