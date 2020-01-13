using Movien.Crawler.Infrostructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.Extensions {

  public class SubscribtionBuilder<T> {

    private readonly IEventQueue queue;
    private readonly Func<T, bool> filter;

    public SubscribtionBuilder(IEventQueue queue, Func<T, bool> filter) {

      this.queue = queue;
      this.filter = filter;
    }

    public IDisposable Subscribe(Action<T> handler) {

      return queue.Subscribe<T>(evt => {
        if (filter(evt))
          handler(evt);
      });
    }
  }

  public static class EventQueueExtensions {

    public static SubscribtionBuilder<T> When<T>(this IEventQueue queue, Func<T, bool> filter) {

      return new SubscribtionBuilder<T>(queue, filter);
    }
  }
}
