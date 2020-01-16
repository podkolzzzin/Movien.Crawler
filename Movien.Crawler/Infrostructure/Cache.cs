using Movien.Crawler.DataAccess.Interfaces;
using Movien.Crawler.DataAccess.Models;
using Movien.Crawler.Events;
using Movien.Crawler.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movien.Crawler.Infrostructure {

  public class Cache : ICache {

    private readonly IWebPagesRepository repository;

    public Cache(IWebPagesRepository repository) {

      this.repository = repository;
    }

    public async Task Save(ContentLoadedEvent evt) {

      await repository.Add(new WebPage() {
        Url = evt.RequestEvent.Url,
        HttpMethod = evt.RequestEvent.Method.ToString(),
        Content = evt.Content,
        Headers = new List<Header>(),
        LoadedAt = evt.LoadedAt,
        Parameters = evt.RequestEvent.FormData.ToDictionary().Select(f => new Parameter() {
          Key = f.Key,
          Value = f.Value
        }).ToList()
      });
      await repository.SaveChanges();
    }

    public async Task<ContentLoadedEvent?> Retrieve(LoadPageEvent evt) {

      var method = evt.Method.ToString();
      var candidates = await repository.Find(f => f.Url.Equals(evt.Url) && f.HttpMethod == method);
      var parameters = evt.FormData.ToDictionary();
      var headers = new Dictionary<string, string>();

      var page = candidates.FirstOrDefault(f => IsCandidateSuits(f, parameters, headers));
      if (page == null)
        return null;

      return new ContentLoadedEvent(evt, page.Content, page.LoadedAt);
    }

    private bool IsCandidateSuits(WebPage candidate, Dictionary<string, string> parameters, Dictionary<string, string> headers) {

      var dbParams = candidate.Parameters.ToDictionary(f => f.Key, f => f.Value);
      var dbHeaders = candidate.Headers.ToDictionary(f => f.Name, f => f.Value);

      foreach(var p in parameters) {
        if (!dbParams.TryGetValue(p.Key, out var val) || val != p.Value)
          return false;
      }

      foreach (var p in headers) {
        if (!dbHeaders.TryGetValue(p.Key, out var val) || val != p.Value)
          return false;
      }

      return true;
    }
  }
}
