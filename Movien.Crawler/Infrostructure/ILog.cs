using System;
using System.Collections.Generic;
using System.Text;

namespace Movien.Crawler.Infrostructure {

  public interface ILog {

    void WriteLine(string line);

    void WriteError(string description, Exception ex);
  }
}
