using Movien.Crawler.Events;
using Movien.Crawler.Infrostructure;
using System;

namespace Movien.Crawler.Services {

  public class CachingService : IDisposable {

    private readonly ICache cache;
    private readonly IDisposable subscribtion;

    public CachingService(IEventQueue queue, ICache cache) {

      this.cache = cache;
      this.subscribtion = queue.Subscribe<ContentLoadedEvent>(CacheItem);
    }

    public void Dispose() {

      subscribtion.Dispose();
    }

    private async void CacheItem(ContentLoadedEvent obj) {

      var cachedItem = await cache.Retrieve(obj.RequestEvent);

      // If item wasn't cached or was cached long time ago.
      if (cachedItem == null || (obj.LoadedAt - cachedItem.Value.LoadedAt) > TimeSpan.FromSeconds(1))
        await cache.Save(obj);
    }
  }
}
