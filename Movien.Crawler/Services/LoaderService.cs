using Movien.Crawler.Events;
using Movien.Crawler.Infrostructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;

namespace Movien.Crawler.Services {

  public class LoaderService : IDisposable {

    private readonly IEventQueue queue;
    private readonly IDisposable eventSubscribtion;

    public LoaderService(IEventQueue queue) {

      this.queue = queue;

      eventSubscribtion = queue.Subscribe<LoadPageEvent>(LoadPage);
    }

    public void Dispose() => eventSubscribtion.Dispose();

    private async void LoadPage(LoadPageEvent uri) {

      Console.WriteLine("Recieve URL to load:" + uri.Url);
      HttpClient client = new HttpClient();
      try {
        HttpResponseMessage result;
        if (uri.Method == HttpMethod.Get) {
          result = await client.GetAsync(uri.Url);
        }
        else {
          result = await client.PostAsync(uri.Url, new FormUrlEncodedContent(GetFormData(uri.FormData)));
        }
        result.EnsureSuccessStatusCode();
        var content = await result.Content.ReadAsStringAsync();
        Console.WriteLine("URL loaded: " + uri.Url);
        queue.Publish(new ContentLoadedEvent(uri.Url, content, DateTime.Now));
      }
      catch (Exception ex) {
        Console.WriteLine(ex.ToString());
      }
    }

    private IEnumerable<KeyValuePair<string, string>> GetFormData(dynamic formData) {

      Dictionary<string, string> result = new Dictionary<string, string>();
      foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(formData)) {
        object obj2 = descriptor.GetValue(formData);
        result.Add(descriptor.Name, obj2.ToString());
      }

      return result;
    }
  }
}
