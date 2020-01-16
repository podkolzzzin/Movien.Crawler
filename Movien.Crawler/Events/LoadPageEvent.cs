using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Movien.Crawler.Events {

  public readonly struct LoadPageEvent {

    public Uri Url { get; }

    public HttpMethod Method { get; }

    public object FormData { get; }

    public object Headers { get; }

    public LoadPageEvent(Uri url, HttpMethod method, dynamic formData, dynamic headers = null) => (Url, Method, FormData, Headers) = (url, method, formData, headers);
  }
}
