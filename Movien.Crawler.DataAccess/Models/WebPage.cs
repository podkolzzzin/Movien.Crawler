using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Text;

namespace Movien.Crawler.DataAccess.Models {
  public class WebPage {

    public int Id { get; set; }

    public Uri Url { get; set; }

    public string HttpMethod { get; set; }

    public string Content { get; set; }

    public DateTime LoadedAt { get; set; }

    public ICollection<Header> Headers { get; set; }
    public ICollection<Parameter> Parameters { get; set; }
  }

  public class Header {

    public int Id { get; set; }
    public int WebPageId { get; set; }

    public WebPage Page { get; set; }

    public string Name { get; set; }
    public string Value { get; set; }
  }

  public class Parameter {

    public int Id { get; set; }
    public int WebPageId { get; set; }
    public WebPage Page { get; set; }

    public string Key { get; set; }
    public string Value { get; set; }
  }
}
