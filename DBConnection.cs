// 맨날쓰다보니 오히려 아무것도 올리지 않은 db연결

// 사용 db나 연결문자열 별로 사용해야하는 Connection, Command 관련 클래스가 조금씩다르다.
// db 연결자체를 적으려고 한것이라 IEnumerable이나 IEnumerator로 받진 않았다.

using System.Data;
using System.Data.OleDb;  // 참고로 OleDbConnection의 경우 버전 몇부터인지 까먹었는데, 최근버전에는 기본적으로 제공하고 있지는 않아 nuget으로 받아야한다.

// 1. OLEDB
// 1-1. OLEDB - 프로시저 사용법

List<클래스> 인스턴스들 = new();
string connectionString = "연결문자열이다.";

using var connection = new OleDbConnection(connectionString);
string storedProcedure = @"스키마명.프로시저명";
connection.Open();

// 트랜잭션을 사용하려거든 
//OleDbTransaction transaction = connection.BeginTransaction(); // 를 이 위치에 추가하고, 

// 쿼리를 반영시키려면  
//transaction.Commit(); //를, 

// 롤백시키려면 
// transaction.Rollback(); // 를 원하는 시점에 적어주면 된다.

using OleDbCommand command = new OleDbCommand(storedProcedure, connection);
command.CommandType = CommandType.StoredProcedure;
command.Parameters.AddWithValue("@프로시저 내 변수명", 넣을 값); // 변수를 넣으려거든 사용하시오!

// 바로 아래는 insert delete update 시의 성공여부를 받아옴, 
// 영향이 가해진 행의 갯수만큼 반환하기 때문에 프로시저 내에 여러 행에 쿼리를 날린다면 갯수를 체크해서 트랜젝션을 걸어주는게 좋다.
// int successCount = command.ExecuteNonQuery();
 
// 아래는 셀렉문, 하나의 테이블값을 가져올때 사용했던 것이다. 두개 이상은 table로 받는것이 나을 것이다, 
// 다만 나중에 성능을 고려해서 IEnumerable<T>로 yield return 할때는 reader로 주로 사용해서 reader를 남겼다.

using OleDbDataReader reader = command.ExecuteReader();
//  dataTable.Load(reader); // 테이블 받는방법 이건 써본적은 없다! - 참고로 [ DataTable dataTable = new DataTable(); ] 이것을 적어도 읽기전에 선언해야 받아올 수 있다.
// table로 받는 방법 2 - using OleDbDataReader reader = command.ExecuteReader(); 문장을 지우고 사용해야한다.
/*
 OleDbDataAdapter adapter = new OleDbDataAdapter(query, connection);
 adapter.Fill(dataTable);
*/
// select문이 여러개 담겨있다면 DataSet에 adapter.Fill(dataSet); 하면 된다.

while (reader.Read())
{
  // 이 부분은 세가지 정도가 있다. 
  // 첫 - 기본
  var 인스턴스 = new 클래스();
  인스턴스.프로퍼티1 = Convert.ToInt32(reader["컬럼명1"]);
  인스턴스.프로퍼티2 = (string)reader["컬럼명2"];
  인스턴스들.Add(인스턴스);

  // 두 - 선언시 입력
  클래스 인스턴스 = new (){
    인스턴스.프로퍼티1 = Convert.ToInt32(reader["컬럼명1"]),
    인스턴스.프로퍼티2 = (string)reader["컬럼명2"],
  };
  인스턴스들.Add(인스턴스);

  // 셋 - 로그 따로 안남길꺼면 한번에 넣기
  인스턴스들.Add(new 클래스(){
    인스턴스.프로퍼티1 = Convert.ToInt32(reader["컬럼명1"]),
    인스턴스.프로퍼티2 = (string)reader["컬럼명2"],
  });
}


// 1-2. OLEDB - 쿼리 select/ insert,update,delete 사용법


// 2. Sql - 보안이 강화됨에 따라 ssl 인증이 디폴트이다. 그래서 같은 db에 연결할때도 oledb로 연결하는 것과 connection문이 다르다.
//          oledb에서는 provider를 적어줘야했는데 SqlConnection으로 연결할때는 
//          provider를 안적어줘도 되지만 encrypt=false;trustServerCertificate=true를 적어줘야한다. ssl인증할때보다 보안이 취약해지지만 사실은 oledb가 그렇게 동작한다.
// 2-1. Sql - 프로시저 사용법


// 2-2. Sql - 쿼리 select/ insert,update,delete 사용법

string connectionString = "연결문자열이다."; // 웹의 경우 GetConnectionString이라던지 다른 프로젝트면 config에서 읽어오거나 복호화는 각자...



// 아직 적을지 고민중이지만 그외 sqlite, oracle등 도 감안중
