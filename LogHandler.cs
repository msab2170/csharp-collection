/*
    참고로 Log4j가 아닌 Serilog를 사용했다. 자바할때 slf4j나 log4j를 많이 사용했는데 serilog를 알게된 뒤에 찾아볼 생각을해서.... 
    일단 써보니 편했다. Log4j가 C#에도 있다고 들었지만 나중에 많은 여유가 생기면 그것도 업로드 해보겠다..
    
    Serilog : nuget패키지 관리자에서 다운받을 수 있는 패키지로
     - 로그를 파일로 남기려면 : Serilog.Sinks.File
       을 같이 받아야 함
    
     - 콘솔 앱의 경우 콘솔에도 로그를 찍고 싶다면 : Serilog.Sinks.Console
       를 같이 받아야 한다.

*/

class LogHandler
{
    public string today;
    public string[] todayYMD;
    public string logFilePath;

    public LogHandler()
    {
        today = DateTime.Now.ToString("yyyy-MM-dd");
        todayYMD = today.Split('-');
        logFilePath = $"프로젝트명-logs/{todayYMD[0]}/{todayYMD[0]}-{todayYMD[1]}/프로젝트명-log-.txt";
    }
}

//-------------------------------------------------------
// Program.cs 
using Serilog;

LogHandler logHandler = new();    
// 잘읽어보면 파악가능하지만 아래 "로그를 지정할 파일 경로" 자리만 채워주면 logHandler선언은 필요없다. 
// LogHandler클래스는 로그 파일 경로를 일관되게 규격화하기 위한 조치로 만들었다. 
// 그래서 실제로 사용할때는 LogHandler클래스의 생성자 내 "프로젝트명" 자리에 nameof를 이용해서 일관되게 프로젝트명이 들어갈 수 있도록 조치하고 있다.

// 프로젝트명은 필요할때 그때그때 Assembly.GetEntryAssembly()?.GetName().Name 로 가져오거나, 
// 메인메소드에 static string projectName = Assembly.GetEntryAssembly()?.GetName().Name;으로 선언해두고 가져오기도 하고,
// 메인메소드에 static string으로 프로젝트명을 복사 붙여넣기해다가 선언하기도 한다. (static string prjectName = "프로젝트명";

Log.Logger = new LoggerConfiguration()
    //.WriteTo.Console()                          // Serilog.Sinks.Console를 설치한 경우 사용가능
    .WriteTo.File(                                // Serilog.Sinks.File를 설치한 경우 사용가능
        logHandler.logFilePath,                   // 로그를 지정할 파일 경로 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: null,
        fileSizeLimitBytes: 50 * 1024 * 1024,
        rollOnFileSizeLimit: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")   
    .MinimumLevel.Information()    // 로그 최저레벨을 설정한다.                
    // .MinimumLevel.Is(Serilog.Events.LogEventLevel.Information) // 바로 위 .MinimumLevel.Information()를 제거하고 이 방법을 사용하면 설정 등으로 가져와서 최저레벨을 지정할 수도 있다.
    .CreateLogger();
