using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Movien.Crawler.Events {

  public readonly struct LoadPageEvent {

    public Uri Url { get; }

    public HttpMethod Method { get; }

    public dynamic FormData { get; }

    public LoadPageEvent(Uri url, HttpMethod method, dynamic formData) => (Url, Method, FormData) = (url, method, formData);
  }
}
