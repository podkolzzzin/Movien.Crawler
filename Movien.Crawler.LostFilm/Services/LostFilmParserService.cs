using HtmlAgilityPack;
using Movien.Crawler.Events;
using Movien.Crawler.Extensions;
using Movien.Crawler.Infrostructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Movien.Crawler.LostFilm.Services {

  public class LostFilmParserService {

    private readonly IEventQueue queue;

    public LostFilmParserService(IEventQueue queue) {

      this.queue = queue;
      queue.When<ContentLoadedEvent>(UrlIsMine).Subscribe(HandleHTML);
    }

    private void HandleHTML(ContentLoadedEvent obj) {

      ParseHTML(obj);
    }

    private bool UrlIsMine(ContentLoadedEvent evt) => !evt.RequestEvent.Url.Equals(Constants.AjaxSeriesesUrl);

    private void ParseHTML(ContentLoadedEvent obj) {

      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(obj.Content);

      // TODO Extract all links that
      // looks like: http://www.lostfilm.tv/series/<SERIES_NAME>/season_<SESAON_NUMBER>/episode_<EPISODE_NUMBER>/
      // Publish them as LoadPageEvents

      Regex episodeLink = new Regex(@"(?:goTo).'(\/series\/\w+\/\w+\/\w+\/)");
      string permaLink = "http://www.lostfilm.tv";
      MatchCollection matches = episodeLink.Matches(obj.Content);
      foreach (Match item in matches) {
        queue.Publish(new LoadPageEvent(
            new Uri(permaLink + item.Groups[1].Value),
            HttpMethod.Get, null));
      }
    }
  }
}
