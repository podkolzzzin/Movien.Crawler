using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.Events {

  public readonly struct ContentLoadedEvent {

    public Uri Url { get; }

    public string Content { get; }

    public DateTime LoadedAt { get; }

    public ContentLoadedEvent(Uri url, string content, DateTime loadedAt) => (Url, Content, LoadedAt) = (url, content, loadedAt);
  }
}
