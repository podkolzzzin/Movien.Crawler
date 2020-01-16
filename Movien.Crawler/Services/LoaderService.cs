using Movien.Crawler.Events;
using Movien.Crawler.Helpers;
using Movien.Crawler.Infrostructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Movien.Crawler.Services {

  public class LoaderService : IDisposable {

    private readonly IEventQueue queue;
    private readonly IDisposable eventSubscribtion;
    private readonly TimeSpan throttling;
    private readonly ILog log;
    private readonly ICache cache;
    private readonly SemaphoreSlim syncObject = new SemaphoreSlim(1);

    private DateTime lastLoaded;

    public LoaderService(IEventQueue queue, TimeSpan throttling, ILog log = null, ICache cache = null) {

      this.queue = queue;
      this.throttling = throttling;
      this.log = log;
      this.cache = cache;

      eventSubscribtion = queue.Subscribe<LoadPageEvent>(LoadPage);
    }

    public void Dispose() => eventSubscribtion.Dispose();

    private async void LoadPage(LoadPageEvent uri) {

      await syncObject.WaitAsync();
      try {
        await Throttle();
        await LoadPageInternal(uri);
      }
      catch (Exception ex) {
        log?.WriteError("Error during load: " + uri.Url, ex);
      }
      finally {
        syncObject.Release();
      }
    }

    private async Task Throttle() {

      var diff = DateTime.Now - lastLoaded;
      if (diff > throttling)
        return;
      await Task.Delay(throttling - diff);
    }

    private async Task LoadPageInternal(LoadPageEvent uri) {

      const string logMessage = "Recieve URL to load:";
      log?.WriteLine(logMessage + uri.Url);

      if (await CheckCache(uri, logMessage.Length))
        return;

      HttpClient client = new HttpClient();
      HttpResponseMessage result;

      if (uri.Method == HttpMethod.Get) {
        result = await client.GetAsync(uri.Url);
      }
      else {
        result = await client.PostAsync(uri.Url, new FormUrlEncodedContent(uri.FormData.ToDictionary()));
      }
      result.EnsureSuccessStatusCode();
      var content = await result.Content.ReadAsStringAsync();

      lastLoaded = DateTime.Now;
      log?.WriteLine("URL loaded: ".PadRight(logMessage.Length) + uri.Url);
      queue.Publish(new ContentLoadedEvent(uri, content, lastLoaded));
    }

    private async Task<bool> CheckCache(LoadPageEvent request, int logMessageLength) {

      if (cache != null) {
        var cachedItem = await cache?.Retrieve(request);
        if (cachedItem != null) {
          log?.WriteLine("Item loaded from cache. HTTPRequest skipped".PadRight(logMessageLength));
          queue.Publish(cachedItem.Value);
          return true;
        }
        else {
          log?.WriteLine("Cache does not contain suitable item".PadRight(logMessageLength));
          return false;
        }
      }
      return false;
    }
  }
}
