// 이 소스는 1개 메소드를 ~~~~가 각각 전부 성공할떄까지 계속 반복실행하는 소스
  
  
  // 디테일한 thread 기초에 관한 설명은 threadBasic 참고 여기서는 멀티 스레드 환경에서 필요한 부분만 적음

    // 각 스레드와 스레드 성공여부를 반환할 배열 생성 (list든 뭐든 놓으면 됨)
    var 사용할 갯 수 = 5;

    Thread[] threads = new Thread[사용할 갯 수];
    bool[] isSuccess = new bool[사용할 갯 수];

    /* 
      currentIndex를 사용하는 이유 :
      
      i를 new Thread(... 뒤에서 그대로 쓰게 되면 
      클로져되어 for문에서 i가 증가하면 
      증가하기전 실행된 스레드 내의 i도 같이 증가하여 원하는 결과를 얻을 수 없게 됨
    */
    
    for (int i = 0; i < 사용할 갯 수; i++)
    {
        isSuccess[i] = false;
        int currentIndex = i;

        threads[i] = new Thread(async () =>
        {
            Log.Information($"threads[{currentIndex}] start");
            while (!isSuccess[currentIndex])
            {
                isSuccess[currentIndex] = await 사용할 함수(변수1, 변수2, 배열1[currentIndex],...);
                if (isSuccess[currentIndex])
                {
                    Log.Information($"threads[{currentIndex}] done");
                }
                else
                {
                    Log.Information($"after 1(sec) restart threads[{currentIndex}]...");
                    Thread.Sleep(1000);
                }
            }           
        });
        threads[currentIndex].Start();
    }

    // 원하는 실행결과를 얻지 못한 케이스가 있으면 프로그램이 유지됨
    while (isSuccess.Any(b => b == false))
    {
        Log.Information($"some threads are running");
        Thread.Sleep(5000);
    }


//------------------------------------------위와 같은 기능을 하는 멀티스레드 예제이다. tasks로 쓰려다가 아예 Parallel을 사용했다.
ThreadPool.SetMaxThreads(50, 10);  // 스레드풀 관리하는 함수인데 참고적으로 남겨둠

bool[] IsSuccesses = new bool[사용할 갯 수];
var countdownEvent = new CountdownEvent(사용할 갯 수);  // 스레드에 index를 부여하는 대신 돌아가는 스레드의 갯수를 채워 넣고  Parallel내에서 감소시킨다. 필수적요소는 아니나, 모든 동작이 완료했음을 캐치하기 위함이다.
string[] arr = str.Split(',');

Parallel.ForEach(arr, async (str, state, index) =>
{
    bool isSuccess = false;
    Log.Information($"threads[{index}] start");

    while (!isSuccess)
    {
        isSuccess = await 사용할 함수(변수1, 변수2, str, (int)index, ...);
        if (isSuccess)
        {
            Log.Information($"threads[{index}] done");
        }
        else
        {
            Log.Information($"after 1(sec) restart threads[{index}]...");
            await Task.Delay(1000);
        }
    }
    countdownEvent.Signal();
});
countdownEvent.Wait();
Log.Information($"All threads done.");
