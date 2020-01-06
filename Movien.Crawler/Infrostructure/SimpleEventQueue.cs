using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.Infrostructure {

  public class SimpleEventQueue : IEventQueue {

    private interface IQueue {
    }

    private class UnsubscribeHandle : IDisposable {

      private readonly Action disposeAction;

      public UnsubscribeHandle(Action disposeAction) => this.disposeAction = disposeAction;

      public void Dispose() => disposeAction();
    }

    private class GenericQueue<T> : IQueue {

      private Action<T> handlers;

      public void Publish(T @event) => handlers?.Invoke(@event);

      public IDisposable Subscribe(Action<T> handler) {

        handlers += handler;
        return new UnsubscribeHandle(() => handlers -= handler);
      }
    }

    private readonly Dictionary<Type, IQueue> queues = new Dictionary<Type, IQueue>();

    public void Publish<T>(T @event) => GetQueue<T>().Publish(@event);

    public IDisposable Subscribe<T>(Action<T> subscribtion) => GetQueue<T>().Subscribe(subscribtion);

    private GenericQueue<T> GetQueue<T>() {

      if (queues.TryGetValue(typeof(T), out var queue))
        return (GenericQueue<T>)queue;
      
      GenericQueue<T> result;
      queues[typeof(T)] = result = new GenericQueue<T>();

      return result;
    }
  }
}
