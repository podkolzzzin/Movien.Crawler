using Movien.Crawler.Events;
using Movien.Crawler.Infrostructure;
using Movien.Crawler.LostFilm;
using Movien.Crawler.LostFilm.Services;
using Movien.Crawler.Services;
using System;
using System.Net.Http;

namespace Movien.Crawler.EntryPoint {
  class Program {
    static void Main(string[] args) {

      var queue = new SimpleEventQueue();
      using (var loader = new LoaderService(queue)) {
        var lostFilmLoader = new LoadFilmParserService(queue);


        queue.Publish(new LoadPageEvent(
          new Uri("http://lostfilm.tv/ajaxik.php"), 
          HttpMethod.Post,
          new {
            act = "serial",
            type = "search",
            o = 0,
            s = 3,
            t = 0
          }));
        Console.ReadLine();
      }
    }
  }
}
