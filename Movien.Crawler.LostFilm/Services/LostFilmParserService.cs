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

    private bool UrlIsMine(ContentLoadedEvent evt) => evt.RequestEvent.Url.Equals(Constants.AjaxSeriesesUrl);

    private void ParseHTML(ContentLoadedEvent obj) {

      HtmlDocument doc = new HtmlDocument();
      doc.LoadHtml(obj.Content);

      // TODO Extract all links that
      // looks like: http://www.lostfilm.tv/series/<SERIES_NAME>/season_<SESAON_NUMBER>/episode_<EPISODE_NUMBER>/
      // Publish them as LoadPageEvents

      Regex regex = new Regex(@"((?:https|http):\/\/)\w{0,3}.lostfilm.\w+\/\w+\S+.\/season_\w{0,3}.\/episode_\w{0,3}.\/");

      List<LoadPageEvent> links = new List<LoadPageEvent>();

      MatchCollection matches = regex.Matches(obj.Content);

      foreach (var match in matches) {
        queue.Publish(new LoadPageEvent(new Uri(match.ToString()), HttpMethod.Get, null));
      }

    }
  }
}
