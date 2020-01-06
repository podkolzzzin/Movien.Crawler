using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.LostFilm.Events {

  public readonly struct RawLinksEvent {

    public Uri[] Urls { get; }

    public RawLinksEvent(Uri[] urls) => (Urls) = urls;
  }
}
