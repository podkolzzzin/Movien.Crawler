using HtmlAgilityPack;
using Movien.Crawler.Events;
using Movien.Crawler.Infrostructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Movien.Crawler.LostFilm.Services {

  public class LoadFilmParserService {

    private readonly IEventQueue queue;

    public LoadFilmParserService(IEventQueue queue) {

      this.queue = queue;
      queue.Subscribe<ContentLoadedEvent>(HandleHTML);
    }

    private void HandleHTML(ContentLoadedEvent obj) {

      if (obj.RequestEvent.Url.AbsolutePath.Contains("ajax"))
        ParseJSON(obj);
      else
        ParseHTML(obj);
    }

        private void ParseHTML(ContentLoadedEvent obj)
        {

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(obj.Content);

            // TODO Extract all links that
            // looks like: http://www.lostfilm.tv/series/<SERIES_NAME>/season_<SESAON_NUMBER>/episode_<EPISODE_NUMBER>/
            // Publish them as LoadPageEvents

            Regex episodeLink = new Regex(@"(?:goTo).'(\/series\/\w+\/\w+\/\w+\/)");
            string permaLink = "http://www.lostfilm.tv";
            MatchCollection matches = episodeLink.Matches(obj.Content);
            foreach (Match item in matches){
                queue.Publish(new LoadPageEvent(
                    new Uri(permaLink + item.Groups[1].Value), 
                    HttpMethod.Get, null));
            }
        }

    private void ParseJSON(ContentLoadedEvent obj) {

      var content = JObject.Parse(obj.Content);
      var data = (JArray)content.GetValue("data");
      foreach (JToken item in data) {
        queue.Publish(new LoadPageEvent(
          new Uri(obj.RequestEvent.Url, item.Value<string>("link") + "/seasons/"),
          HttpMethod.Get, null));
      }
    }
  }
}
