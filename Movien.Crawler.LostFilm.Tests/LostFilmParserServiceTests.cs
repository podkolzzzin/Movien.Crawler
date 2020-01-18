using Movien.Crawler.Events;
using Movien.Crawler.LostFilm.Services;
using Movien.Crawler.LostFilm.Tests.Moqs;
using NUnit.Framework;
using System;
using System.Net.Http;

namespace Movien.Crawler.LostFilm.Tests {

  public class LostFilmParserServiceTests {

    [SetUp]
    public void Setup() {
    }

    [Test]
    public void Test1() {

      var queue = new EventQueueMoq();
      var parser = new LostFilmParserService(queue);
      //queue.Subscribe<LoadPageEvent>(evt => {

      //  Assert.AreEqual("https://lostfilm.tv/series/The_Young_Pope/season_1/episode_10/", evt.Url.ToString());
      //});

      queue.PublishRealEvent(new ContentLoadedEvent(
        new LoadPageEvent(new System.Uri("https://lostfil"), HttpMethod.Get, null, null),
        "onclick=\"goTo('/series/The_Young_Pope/season_1/episode_10/', false)\"",
        DateTime.Now
      ));


      Assert.AreEqual(1, queue.Events.Count);

      var evt = (LoadPageEvent)queue.Events[0];
      Assert.AreEqual("https://lostfilm.tv/series/The_Young_Pope/season_1/episode_10/", evt.Url.ToString());
    }
  }
}