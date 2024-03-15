namespace ProceedWithTheAssignment_2;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
    
    /// <summary>
    /// 솔루션
    /// </summary>
    /// <param name="plans">과제 정보</param>
    /// <returns>과제의 순서가 담긴 String[] 결과</returns>
    public string[] Solution(string[,] plans) 
    {
        // 정답 정보 
        List<string> answer = new List<string>();
        
        // 진행 전체 row 를 가져온다
        Test[] process = new Test[plans.GetLength(0)];
        
        // 잠시 멈춰놓을 작업 
        Stack<Test> paused = new Stack<Test>();

        // 들어온 작업을 전체 변환한다.
        for(int i = 0; i < plans.GetLength(0); ++i)
            process[i] = new Test(plans[i, 0], plans[i, 1], plans[i, 2]);

        // 시작시간 순으로 작업을 재정렬 한다.
        process = process.OrderBy(i => i.start).ToArray();

        // 진행된 시간
        int currentTime = 0;
        
        // 시작 인덱스
        int index = 0;
        
        // 모든 프로세스가 종료되기 전까지 반복한다.
        while(index < plans.GetLength(0))
        {
            // 현재 테스트 정보를 가져온다.
            Test currentTest = process[index];

            // 마지막 루프인지 확인한다. 
            bool isLast = index + 1 == plans.GetLength(0);
            
            // 진행 가능한 시간 여부 판별 
            int availableTime = isLast ? int.MaxValue : process[index + 1].start - currentTest.start;
            
            // 현재 테스트 작업을 우선 Pause 에 넣는다.
            paused.Push(currentTest);
            
            // Pause 가 존재 하는 경우 
            while(paused.Count > 0)
            {
                // 가장 첫번째 작업을 먼저 가져온다.
                Test top = paused.Peek();
                
                // 쓸 수 있는 시간이 남은 시간보다 큰경우 ( 과제가 완료가 가능한 경우 )
                if(availableTime >= top.remain) 
                {
                    // 가능한 시간에서 남은 시간을 뺀다
                    availableTime -= top.remain;
                    answer.Add(top.name);
                    
                    // 스택에서 제거 
                    paused.Pop();

                    // 가능한 시간이 0 인경우 
                    if(availableTime == 0)
                        break;
                }
                else // 꺼낸 과제를 완료하기에는 시간이 부족함.
                {
                    top.remain -= availableTime;
                    break;
                }
            }

            ++index;
        }

        return answer.ToArray();
    }
}


public class Test
{
    public string name;
    public int start;
    public int remain;

    public Test(string name, string timeStr, string playTimeStr)
    {
        this.name = name;
        string[] splited = timeStr.Split(':');
        start = int.Parse(splited[0]) * 60 + int.Parse(splited[1]);
        remain = int.Parse(playTimeStr);
    }
}
