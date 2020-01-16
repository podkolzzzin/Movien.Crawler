using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Movien.Crawler.Helpers {
  public static class Extensions {

    public static Dictionary<string, string> ToDictionary(this object obj) {

      Dictionary<string, string> result = new Dictionary<string, string>();
      foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj)) {
        object obj2 = descriptor.GetValue(obj);
        result.Add(descriptor.Name, obj2.ToString());
      }

      return result;
    }
  }
}
