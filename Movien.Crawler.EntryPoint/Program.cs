using Movien.Crawler.DataAccess;
using Movien.Crawler.Events;
using Movien.Crawler.Infrostructure;
using Movien.Crawler.LostFilm;
using Movien.Crawler.LostFilm.Services;
using Movien.Crawler.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Movien.Crawler.EntryPoint {
  class Program {
    static async Task Main(string[] args) {

      var queue = new SimpleEventQueue();
      var simpleLog = new SimpleConsoleLog();
      using (var unitOfWork = new MovienUnitOfWork()) {
        await unitOfWork.Setup();

        var cache = new Cache(unitOfWork.WebPages);

        using (var cachingService = new CachingService(queue, cache))
        using (var loader = new LoaderService(queue, throttling: TimeSpan.FromSeconds(1), simpleLog, cache)) {
          var lostFilmLoader = new LostFilmParserService(queue);
          var lostFilmSeriesLoader = new LostFilmSeriesesLoader(queue);

          lostFilmSeriesLoader.Start();
          Console.ReadLine();
        }
      }
    }
  }
}
