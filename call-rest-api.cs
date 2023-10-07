// 파일 계속 수정중 - 231007
// HttpClient를 메소드 밖에서 선언해서 싱글톤 형태로 사용,(메모리를 많이 잡아 먹는 등 성능상의 이유로 매 메소드마다 선언하면 좋지 않다!)
// 원래는 baseAddress 속성이있는데 보고 잠깐 쓰는거라 다음번에 시간 날때 제대로 수정해놓을 예정
HttpClient client = new("https://abcd.dcba.com");   

public async Task<int> HTTPGet(타입1 변수1,...){  // 가장 단순한 Get 방식
  int result = 0;

  string endPoint = "/get/endpoint"; 
  string queryString=$"?변수1={변수1}&변수2={변수2}...";
  
  try
  {
    
    HttpResponseMessage response = await client.GetAsync(endPoint);  //requestUrl 경로로 Get 방식으로 request 후 response를 받음
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
