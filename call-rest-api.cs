using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Text;

// HttpClient를 메소드 밖에서 선언해서 싱글톤 형태로 사용,(메모리를 많이 잡아 먹는 등 성능상의 이유로 매 메소드마다 선언하면 좋지 않다!)

private static readonly Uri _Server = new("https://~~ 서버의 주소"); 
private static readonly HttpClient _HttpClient = new()
{
    Timeout = TimeSpan.FromSeconds(30),
    BaseAddress = _Server
};


// GET 방식
public async Task<받은변수의 Model class> HTTPGet(클래스1 변수1,...){  // 가장 단순한 Get 방식
  string endPoint = "/get/endpoint"; 
  string queryString=$"?변수1={변수1}&변수2={변수2}...";
  
  try
  {    
    HttpResponseMessage response = await client.GetAsync(endPoint);  //requestUrl 경로로 Get 방식으로 request 후 response를 받음, _HttpClient에 BaseAddress를 넣어놓으면 endpoint만 입력해주면 된다.
    var responseContent = await response.Content.ReadAsStringAsync();    // 문자열로 받아온 responseBody     
    Log.Information($"[{response.StatusCode}]{responseContent}");

      // 응답상태코드가 200이면 (응답코드별로 HttpStatusCode가 존재한다. api서버의 스펙에 맞춰서 분기를 타서 어떤 결과를 내릴지 결정하면된다. 
      // 이 예제처럼 딱 성공했을때 외에는 프로그램 로직이 멈춰야하는 경우라면 exception을 던져주는 것도 방법이고 
      // 실패시 특정값을 넣어주는것도 방법이며, 
      // api서버에 응답코드별로 response 내용이 다르다면 그 값들을 활용하여 결과를 내려주는 것이 좋다.
    if (response.StatusCode == HttpStatusCode.OK)  
    {      
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
    Log.Error(ex.ToString());
  }
  return 받은변수;  
}


// POST 방식 (request body에 application/json)
public async static Task<ResponseResult?> HTTPPost(클래스1 변수1,...)
{
  Log.Information($"HTTPPost() start");  // 이건 꼭 찍을 필요없는데 다른 파일에서 설명했던 Serilog로 찍은 로그이고 메소드에서 입력받은 변수도 json형태처럼 보이게 찍기도 함
    string endpoint = $"/변수1/{변수1}/...";
    ResponseResult? responseResult = new();

    var 요청변수 = new 요청클래스형태()
    {
        요청변수내변수 = new 요청클래스 내부에 프로퍼티()
        {
            a1 = b,
            a2 = c,...
        }
    };

    try
    {
        _HttpClient.DefaultRequestHeaders.Accept.Clear();
        _HttpClient.DefaultRequestHeaders.Clear();
        if (!_HttpClient.DefaultRequestHeaders.Contains("토큰이 필요하다면"))
        {
            _HttpClient.DefaultRequestHeaders.Add("토큰이 필요하다면", 토큰을 선언해서 넣으면된다.);
        }
      
        // MS 공식에는 Json 변환 + PostAsync 를 PostAsJsonAsync로 쓸 수 있다고 나오는데 여러번 시도 했는데 잘안되서 다 씀 ㅠㅠ
        // 그래도 이렇게 다쓰는 것도 다른 형식과 비교할때 편하긴 함
      
        var json요청변수 = JsonConvert.SerializeObject(요청변수, Formatting.None, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
        HttpContent httpContent = new StringContent(json요청변수, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _HttpClient.PostAsync(endpoint, httpContent);  
              
        var responseContent = await response.Content.ReadAsStringAsync();
      
        Log.Information($"[{response.StatusCode}]{responseContent}");  // 이 로그는 성공여부 확인전에 찍어주는게 좋은게 api 서버에서 내려주는 값이 무엇인지 로그를 통해서 확인하면 의도된 에러이거나 코드값인지 파악하기 좋음
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        {
            responseResult = JsonConvert.DeserializeObject<ResponseResult>(responseContent);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.ToString());
    }
    finally
    {
        Log.Information($"HTTPPost() Exit {{변수1: {변수1}, 변수2: {변수2}, ...}}");  //  json형태처럼 보이게 찍을때 이런형태를 쓰기도 하고 여기도 JsonConvert.SerializeObject를 쓰기도 함
    }
    return responseResult;
}


// POST 방식(request body에 application/x-www-form-urlencoded)
public async static Task<ResponseResult?> HTTPPost(클래스1 변수1,...)
{
  Log.Information($"HTTPPost() start");  // 이건 꼭 찍을 필요없는데 다른 파일에서 설명했던 Serilog로 찍은 로그이고 메소드에서 입력받은 변수도 json형태처럼 보이게 찍기도 함
    string endpoint = $"/변수1/{변수1}/...";
    string requestBody =$"a={a}&b={b}";

    try
    {
        // 앞에쓴 헤더 생략      
        HttpContent content = new StringContent(requestBody, Encoding.UTF8, "application/x-www-form-urlencoded");
        HttpResponseMessage response = await _HttpClient.PostAsync(tokenEndpoint, content);          
        var responseContent = await response.Content.ReadAsStringAsync();
      
        Log.Information($"[{response.StatusCode}]{responseContent}");  // 이 로그는 성공여부 확인전에 찍어주는게 좋은게 api 서버에서 내려주는 값이 무엇인지 로그를 통해서 확인하면 의도된 에러이거나 코드값인지 파악하기 좋음
        if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
        {
            responseResult = JsonConvert.DeserializeObject<ResponseResult>(responseContent);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex.ToString());
    }
    finally
    {
        Log.Information($"HTTPPost() Exit {{변수1: {변수1}, 변수2: {변수2}, ...}}");  //  json형태처럼 보이게 찍을때 이런형태를 쓰기도 하고 여기도 JsonConvert.SerializeObject를 쓰기도 함
    }
    return responseResult;
}
