using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Movien.Crawler.Infrostructure {

  public class SimpleConsoleLog : ILog {

    public void WriteError(string description, Exception ex) {

      WriteLine(description);
      using (var reader = new StringReader(ex.ToString())) {
        string line = null;
        while ((line = reader.ReadLine()) != null)
          Console.WriteLine(new string(' ', 10) + line);
      }
    }

    public void WriteLine(string line) {

      Console.WriteLine((DateTime.Now.ToString("HH:mm:ss") + "|").PadRight(10) + line);
    }
  }
}
