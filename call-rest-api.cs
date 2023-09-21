public async Task<int> HTTPGet(타입1 변수1,...){
  int result = 0;

  string domain = "https://abcd.dcba.com";
  string endPoint = "/get/endpoint"; 
  string url=$"{domain}{endPoint}";
  string queryString=$"?변수1={변수1}&변수2={변수2}...";

  string reqeustUrl = $"{url}{queryString}";
  // 이 위의 작업들은 Uri 클래스를 활용해도 좋더라구요!
  
  try
  {
    HttpClient client = new();
    HttpResponseMessage response = await client.GetAsync(reqeustUrl);
  
    if (response.StatusCode == HttpStatusCode.OK)
    {
      var responseContent = await response.Content.ReadAsStringAsync();
      var 받은변수 = JsonConvert.DeserializeObject<받을타입model이 있다면 여기에>(responseContent);
      Log.Information($"[{response.StatusCode}]{responseContent}");
      result = 1;
    }
    else
    {
      result = -1;
      throw new Exception($"response status code[{response.StatusCode}]");
    }   
  }
  catch (Exception ex)
  {
    result = -1;
    Log.Error(ex.ToString());
  }
  return result;  
}
