using System;
using System.Threading;

using DotNetify;

namespace frontend_vue
{
  public class HelloWorld : BaseVM
  {
    private readonly Timer timer;
    public string Greetings => "Hello World!";
    public DateTime ServerTime => DateTime.Now;

    public HelloWorld()
    {
      timer = new Timer(_ =>
      {
        Changed(nameof(ServerTime));
        PushUpdates();
      }, null, 0, 1000);
    }

    public override void Dispose() => timer.Dispose();
  }
}
