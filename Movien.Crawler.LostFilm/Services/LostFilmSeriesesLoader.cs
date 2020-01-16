using Movien.Crawler.Events;
using Movien.Crawler.Extensions;
using Movien.Crawler.Infrostructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Movien.Crawler.LostFilm.Services {
  public class LostFilmSeriesesLoader : IDisposable {

    private readonly IDisposable subscribtion;
    private readonly IEventQueue queue;

    public LostFilmSeriesesLoader(IEventQueue queue) {

      this.queue = queue;
      subscribtion = queue.When<ContentLoadedEvent>(UrlIsMine).Subscribe(PageHandler);
    }

    public void Start() => LoadNext(new ContentLoadedEvent(), 0);

    public void Dispose() => subscribtion.Dispose();

    private void PageHandler(ContentLoadedEvent evt) => ParseJSON(evt);

    private bool UrlIsMine(ContentLoadedEvent evt) => evt.RequestEvent.Url.Equals(Constants.AjaxSeriesesUrl);

    private void ParseJSON(ContentLoadedEvent obj) {

      var content = JObject.Parse(obj.Content);
      var data = (JArray)content.GetValue("data");

      if (data.Count > 0)
        LoadNext(obj, data.Count);

      foreach (JToken item in data) {
        queue.Publish(new LoadPageEvent(
          new Uri(obj.RequestEvent.Url, item.Value<string>("link") + "/seasons/"),
          HttpMethod.Get, null));
      }
    }

    private void LoadNext(ContentLoadedEvent evt, int loadedCount) {

      int offset = ((dynamic)evt.RequestEvent.FormData)?.o + loadedCount ?? loadedCount;

      queue.Publish(new LoadPageEvent(Constants.AjaxSeriesesUrl, HttpMethod.Post,
       new {
         act = "serial",
         type = "search",
         o = offset,
         s = 1,
         t = 0
       }));
    }
  }
}
