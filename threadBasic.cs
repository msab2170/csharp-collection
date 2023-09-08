using System.Diagnostics;
using System.Net;

/*
  new Thread( 뒤에 () => {}); 를 붙이면 변수를 필요로 하는 메소드를 반복 실행할 수 있다.
  async를 붙일때는 {} 안에 await를 사용할 비동기 메소드가 있는 경우이다.
  
  이 경우 비동기 함수는 선언시 async void 메소드명() 꼴로 선언해야하며
  리턴 값을 가지고 싶다면 Task<T>에 제네릭으로 적으면 된다.

  아래 예제에서는 Task<bool>을 리턴 받아 성공이면 프로그램을 종료한다.

  thread.Start()로 스레드를 시작하고 난 뒤에 오는 소스들은
  스레드가 모두 종료될때까지는 프로그램을 유지시키는 역할을 한다.

  성능을 위해 Sleep을 넣어 너무 자주 찍히지 않게 했다.
  원래는 5초마다 돌아가고 있다는 로그를 남겼지만 주석처리했다.
*/


bool IsSuccess = false;
var thread = new Thread(async () => {   
                    while (!IsSuccess)
                    {
                        IsSuccess = await 계속돌릴메소드(변수1, 변수2, ...); 
                        if (IsSuccess)
                        {
                            Console.Write($"all done, thread join()");
                            return; // Environment.Exit(0); 콘솔의 경우고 응용프로그램이면 Application.Exit() - 맞죠?
                        }
                        Console.Write($"after 1(sec) restart thread...");
                        Thread.Sleep(1000);
                        }
                });

thread.Start();
bool anyAliveThreads = true;
while (anyAliveThreads) {
   // Console.Write($"program running");
    Thread.Sleep(5000);
    var runningThreads = Process.GetCurrentProcess().Threads.OfType<ProcessThread>();
    anyAliveThreads = runningThreads.Any(thread => thread.ThreadState == System.Diagnostics.ThreadState.Running);
}
thread.Join();
