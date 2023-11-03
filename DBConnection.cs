// 맨날쓰다보니 오히려 아무것도 올리지 않은 db연결

// 사용 db나 연결문자열 별로 사용해야하는 Connection, Command 관련 클래스가 조금씩다르다.
// db 연결자체를 적으려고 한것이라 IEnumerable이나 IEnumerator로 받진 않았다.

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

// 바로 아래는 insert delete update
// int mustBe1 = command.ExecuteNonQuery();
 
// 아래는 셀렉문, 하나의 테이블값을 가져올때 사용했던 것인고 두개 이상은 table로 받는 것이 좋다.
using OleDbDataReader reader = command.ExecuteReader();
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
