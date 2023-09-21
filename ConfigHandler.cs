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

//-------------------------------------------------------
// Program.cs 
ConfigHandler.ReadConfig(); 
string a = ConfigHandler.configData["아무변수"];

// 서버를 재시작하지 않고 설정파일을 변경하고 싶은경우 ReadConfig()에 변수를 담아 메소드가 실행될때마다 읽어오게 하면 된다.
