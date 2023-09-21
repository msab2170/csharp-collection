class ConfigHandler
{
    public static readonly string configFile = "설정파일";             // 설정파일경로\설정파일명
    public static Dictionary<string, string> configData = new(); // 설정 정보 저장을 위한 Dictionary

    // 설정파일 a:b꼴로 저장한 값을 읽어와 configData에 저장
    public static void ReadConfig()
    {
        if (File.Exists(configFilePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(configFilePath);

                foreach (string line in lines)
                {
                    // 빈 줄이면 패스, #는 config.txt 주석 처리
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;

                    if (line.Contains(":"))
                    {
                        string key = line.Substring(0, line.IndexOf(":")).Trim();
                        string value = line.Substring(line.IndexOf(":") + 1).Trim();
                        configData[key] = value;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"ConfigHandler.readConfig - An error occurred while reading file[{configFile}]: {ex.Message}");
            }
        }
        else
        {
            throw new Exception($"ConfigHandler.readConfig - [{configFile}] file not found.");
        }
    }
}

// 설정 파일에 a:asdf,wqer,fr 꼴로 여러개를 입력받아 배열로 만드는 메소드
public static string[] transformConfigData(string configString)
{
    string[] ConfigDataArr;
    if (!string.IsNullOrEmpty(HandleConfiguration.configData[configString])
        && HandleConfiguration.configData[configString].Contains(","))
    {
        ConfigDataArr = HandleConfiguration.configData[configString].Split(',');
    }
    else if (!string.IsNullOrEmpty(HandleConfiguration.configData[configString]))
    {
        ConfigDataArr = new string[1];
        ConfigDataArr[0] = HandleConfiguration.configData[configString];
    }
    else
    {
        throw new Exception($"{configString} is null or Empty!");
    }
    return ConfigDataArr;
}
// Split() 을 편하게 쓰고자 배열로 만들었고, 
// 필자가 작업시에는 해당정보가 없으면 프로그램이 돌아가면 안되는 경우가 보통이라서 null or empty일때 exception을 던지는 경우가 많다. 
// 각자 사용하기 편한 자료형으로 만들면 될듯!

//-------------------------------------------------------
// Program.cs 
ConfigHandler.ReadConfig(); 
string a = ConfigHandler.configData["아무변수"];

// 서버를 재시작하지 않고 설정파일을 변경하고 싶은경우 ReadConfig()에 변수를 담아 메소드가 실행될때마다 읽어오게 하면 된다.
