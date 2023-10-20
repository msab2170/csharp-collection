# csharp-collection
c#을 사용하며 기억해야 하는 내용과 특정라이브러리 용법 등을 정리하려고 만든 레파지토리

20230921 - 사용 언어별로 파일을 만들기로 결정하고 Handler 레파지토리와 하나로 합침


-------------------------------------------------


# 자주 사용하는 내용

-------------------------------------------------

 - BasicAboutFileSystem.cs

        아주 기초적인 FIleSystem 관련 내용을 담았다. 왜 이제야 올렸나 싶을 정도로 기초적이면서도 중요한 내용이고
        이렇게 따로 공부한 건 아니지만 필요에 의해 계속 쓰고 있던 내용들이다.
        Path.~, Environment.~, Directory.~, File.~, Stream 등 중요한 기초들을 담을 예정이다.

-------------------------------------------------

 - SimpleSwitch.cs
   
        아직 일하면서 써본적없지만 변수에 넣는 switch는 너무 매력적이다.

-------------------------------------------------
   
 - LogHandler.cs
   
        필요 nuget package는 Loghandler.cs 내 기입
        LogHandler.cs 상단부를 하나 만들고
        하단 부를 실제 사용할 Program.cs에 적어서 사용
        자바에서 쓰던 log4j와 유사한 log4net도 존재하는데 이를 찾아볼 생각하기전에
        사용하기 시작한 Serilog가 편해서 계속 사용하고 있음
        
        Log.Information($"쓸말 : {변수명}"); 형태로 사용할 수 있으며 서식문자열도 사용이 가능
   
        // 아래는 로그 단계 순서
        [Log.Verbose(), Log.Debug(), Log.Information(), Log.Warning(), Log.Error() ]

        최소 로그레벨을 지정해야 하므로 보여질 로그를 체크하여 설정하면 된다.
        설정파일 등으로 외부의 조건을 가져와서 지정할 수도 있다.


------------------------------------

- ConfigHandler.cs

          ConfigHandler.cs를 클래스로 만들고 사용
          설정파일을 읽어오는데 사용

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
          HTTP 메소드 중 주요 메소드라고 할 수 있는 것들은 아래 5가지 이다.
           - GET : 리소스 조회
           - POST:  요청 데이터 처리, 주로 등록에 사용
           - PUT : 리소스를 대체(덮어쓰기), 해당 리소스가 없으면 생성
           - PATCH : 리소스 부분 변경(PUT은 전체, PATCH는 일부 변경)
           - DELETE : 리소스 삭제
        
         POST 이하는 비슷하므로 requestbody의 타입별로 작성하고 있다.
          - application/json
          - application/x-www-form-urlencoded
          - mulitpart/form-data
  
 
------------------------------------------------------------------




------------------------------------------------------------------

# 특정 경우에만 사용했던 내용

- UsingMsgReader.cs

        MsgReader 라이브러리를 통해
        이메일파일(.msg확장자파일, .eml확장자파일)의 정보와
        첨부파일의 정보를 추출하고, 첨부파일을 다운받을 수 있다.

- OutlookAddinProject.cs

        OutlookAddin 프로젝트의 구성 파악을 위해 작성한 파일
        Outlook 실행시 동작할 로직, 메일 전송버튼을 눌렀을때 동작할 로직의 위치 정도를 적어둔 것.


