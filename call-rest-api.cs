public async Task<int> HTTPGet(타입1 변수1,...){  // 가장 단순한 Get 방식
  int result = 0;

  string domain = "https://abcd.dcba.com";
  string endPoint = "/get/endpoint"; 
  string url=$"{domain}{endPoint}";
  string queryString=$"?변수1={변수1}&변수2={변수2}...";

  string reqeustUrl = $"{url}{queryString}";
  // 이 위의 작업들은 Uri 클래스를 활용해도 좋더라구요!
  
  try
  {
    HttpClient client = new();    // Header 관련도 곧 추가 예정
    HttpResponseMessage response = await client.GetAsync(reqeustUrl);  //requestUrl 경로로 Get 방식으로 request 후 response를 받음
    var responseContent = await response.Content.ReadAsStringAsync();    // 문자열로 받아온 responseBody     
    Log.Information($"[{response.StatusCode}]{responseContent}");
    
    if (response.StatusCode == HttpStatusCode.OK)  // 응답상태코드가 200이면
    {      
      result = 1;
      var 받은변수 = JsonConvert.DeserializeObject<받을타입model이 있다면 여기에>(responseContent);  // 받아온 문자열이 제이슨 형태라면 Deserialize하여 타입변수에 넣을 수 있음
      // 요청이 성공해야 원하는 형태의 데이터가 오기때문에 분기로 거른다음에 받아야함
    }
    else
    {
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
