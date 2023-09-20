# csharp-collection
c#을 사용하며 기억해야 하는 내용들을 정리하려고 만든 레파지토리


------------------------------------------------------------------

  
- ThreadBasic.cs

  new Thread( 뒤에 () => {}); 를 붙이면 변수를 필요로 하는 메소드를 반복 실행할 수 있다.
  async를 붙일때는 {} 안에 await를 사용할 비동기 메소드가 있는 경우
  
  이 경우 비동기 함수는 선언시 async void 메소드명() 꼴로 선언해야하며
  리턴 값을 가지고 싶다면 Task<T>에 제네릭으로 작성

  아래 예제에서는 Task<bool>을 리턴 받아 성공이면 프로그램을 종료

  thread.Start()로 스레드를 시작

  그 뒤 while 문은 스레드가 모두 종료될때까지는 프로그램을 유지시키는 역할

  처음에는 gpt의 조언을 따라 live thread를 찾아서 사용하였는데
  너무 단순하게도 성공여부를 받아오는 변수가 이미 있어 수정
  
------------------------------------------------------------------


- multiThreads.cs
  전부 성공할때까지 계속 메소드를 실행하는 소스

  1. 클로저 개념을 이해하여 증가하는 값을 변수를 만들어 사용한다.
  2. .Any를 이용하여 하나라도 실패면 프로그램이 유지하도록 한다.
------------------------------------------------------------------

- call-rest-api.cs

  api 서버의 조건과 결과 기댓값을 알때 호출하여 결과값을 내려받는다.
------------------------------------------------------------------
