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
        logFilePath = $"logs/{todayYMD[0]}/{todayYMD[0]}-{todayYMD[1]}/프로젝트명-log-.txt";
    }
}

//-------------------------------------------------------
// Program.cs 

using Serilog;

LogHandler logHandler = new();
Log.Logger = new LoggerConfiguration()
    //.WriteTo.Console()                         // Serilog.Sinks.Console를 설치한 경우 사용가능
    .WriteTo.File(logHandler.logFilePath,        // Serilog.Sinks.File를 설치한 경우 사용가능
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: null,
        fileSizeLimitBytes: 50 * 1024 * 1024,
        rollOnFileSizeLimit: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {Message}{NewLine}{Exception}")
    
    .MinimumLevel.Information()    // 로그 최저레벨을 설정한다.                
    // .MinimumLevel.Is(Serilog.Events.LogEventLevel.Information) // 바로 윗 문장을 제거하고 이 방법을 사용하면 설정 등으로 가져와서 최저레벨을 지정할 수도 있다.
    .CreateLogger();
