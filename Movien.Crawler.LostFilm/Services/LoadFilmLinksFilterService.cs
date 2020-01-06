using Movien.Crawler.Events;
using Movien.Crawler.Infrostructure;
using Movien.Crawler.LostFilm.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.LostFilm.Services {

  public class LoadFilmLinksFilterService {

    private readonly IEventQueue queue;

    public LoadFilmLinksFilterService(IEventQueue queue) {

      this.queue = queue;
      queue.Subscribe<RawLinksEvent>(HandleMultipleUrls);
    }

    private void HandleMultipleUrls(RawLinksEvent urls) {
      
      // Publish an event for each url of series
    }
  }
}
