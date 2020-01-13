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
      using (var loader = new LoaderService(queue, throttling: TimeSpan.FromSeconds(1))) {
        var lostFilmLoader = new LostFilmParserService(queue);
        var lostFilmSeriesLoader = new LostFilmSeriesesLoader(queue);

        lostFilmSeriesLoader.Start();
        Console.ReadLine();
      }
    }
  }
}
