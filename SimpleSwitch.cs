// 이 파일 예제는 단순한 switch를 다룸
// 이 문법이 자바,코틀린,js,dart중에서도 있는 언어가 있는지 모르겠지만
// 첨봤고, 언젠가 써먹기도 좋은듯한 느낌!

// switch-case문 말고 변수로 직접 넣는 switch

// 예제 1, + 재귀하여 점화식으로 수열 만들기 - 너무 획기적이다. 미쳤다.
int Fibonacci(int index) =>
  index switch {
    1 => 0,
    2 => 1,
    _ => Fibonacci(index - 2) + Fibonacci(index - 1)
};

// 예제 2, switch-case 안에 switch
string NumberOrdinal(int number){  // 간단예제로 2자리까지만으로 가정
  switch (number) {
    case 11:
    case 12:
    case 13:
      return $"{number}th";  //1,2,3으로 끝나지만 규칙을 만족하지 못하는 예외 케이스 거르기
    default:
      int lastNum = number % 10;
      string suffix = lastNum switch{  // _라는 조건은 미쳤다. switch-case에서 default와 기능적으로는 동일할 지 모르나, 변수에 즉시 들어가는 값이라는게 미쳤다....
        1 => "st",
        2 => "nd",
        3 => "rd",
        _ => "th"
      };
      return $"{number}{suffix}";
  }
}
