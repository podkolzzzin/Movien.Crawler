using Movien.Crawler.Infrostructure;
using System;
using System.Collections.Generic;

namespace Movien.Crawler.LostFilm.Tests.Moqs {
  public class EventQueueMoq : IEventQueue {

    private Delegate handler;

    public List<object> Events { get; } = new List<object>();

    public void Publish<T>(T @event) {

      Events.Add(@event);
    }

    public void PublishRealEvent<T>(T @event) {

      handler?.DynamicInvoke(@event);
    }

    public IDisposable Subscribe<T>(Action<T> handler) {

      this.handler = handler;
      return new DisposableMoq();
    }
  }
}