/* 
    결국 이전 버전에서는 ini파일을 초기에만 읽어와 Dictionary에 담는 방법을 썼는데
    서비스가 유지중인 곳에서 쓸때는 config파일의 내용을 수정하면 반영이 안되기 때문에
    .config 파일에 담아 그때 그때 읽어오도록 수정했다.

    참고로 string value = line[(line.IndexOf(":") + 1)..].Trim(); // 이 문법은 구버전에서는 작동하지 않을 수 있다. 그럴때에는 Substring()를 사용하면 된다.
*/

class ConfigHandler
{
    public static readonly string configFile = "설정파일";             // 설정파일경로\설정파일명

    // 설정파일에 a:b꼴로 저장한 값을 return
    public static string ReadConfig(string configInfo)
    {
        if (File.Exists(configFilePath))
        {
            try
            {
                string[] lines = File.ReadAllLines(configFilePath);
                
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                        continue;
    
                    if (line.Contains(':'))
                    {
                        string key = line[..line.IndexOf(":")].Trim() ?? throw new Exception($"ConfigHandler.ReadConfig - ReadConfig({configInfo}) failed."); 
    
                        if (key.Equals(configInfo))
                        {
                            string value = line[(line.IndexOf(":") + 1)..].Trim();    
                            Log.Information($"ConfigHandler.ReadConfig(\"{configInfo}\")={value}");
                            return value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"ConfigHandler.readConfig - An error occurred while reading the config file[{configFilePath}]: {ex.Message}");
            }
        }
        else
        {
            throw new Exception($"ConfigHandler.readConfig - [{configFilePath}] file not found.");
        }
        throw new Exception($"ConfigHandler.readConfig - maybe [{configInfo}] not exists in [{configFilePath}].");
    }


// 설정 파일에 a:asdf,wqer,fr 꼴로 여러개를 입력받아 배열로 만드는 메소드
public static string[] TransformConfigData(string configKey)
{
    string[] ConfigDataArr;
    string configValue = ReadConfig(configKey);
          
    if (!string.IsNullOrEmpty(configValue) && configValue.Contains(','))
    {
        ConfigDataArr = configValue.Split(',');
    }
    else if (!string.IsNullOrEmpty(configValue))
    {
        ConfigDataArr = new string[1];
        ConfigDataArr[0] = configValue;
    }
    else
    {
        throw new Exception($"the value that key is [{configKey}] in [{configFilePath}] is necessary.");
    }
    return ConfigDataArr;
}

