using System.Net;

namespace ProceedWithTheAssignment;

class Program
{
    /// <summary>
    /// 메인 
    /// </summary>
    /// <param name="args">실행 변수</param>
    static void Main(string[] args)
    {
        Console.WriteLine("Level2 과제 진행하기");
    }
    
    /// <summary>
    /// 솔루션
    /// </summary>
    /// <param name="plans">과제의 내용들</param>
    /// <returns></returns>
    public string[] Solution(string[,] plans)
    {
        List<string> answer = new List<string>();
        
        // 행과 의 수를 가져온다.
        int rows = plans.GetLength(0);
        
        // 작업 정보가 저장될 리스트
        List<Task> tasks = new List<Task>();
        
        // 계획 정보를 전체 리스트로 변환한다.
        for (int i = 0; i < rows; i++)
        {
            Task add = new Task() {Name = plans[i, 0], StartTime = plans[i, 1] , PlayTime = int.Parse(plans[i,2])};
            
            // 시작 시간을 전체 분단위로 변환
            string[] parts = add.StartTime.Split(':');
            add.StartTimeMinute = int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
            tasks.Add(add);
        }

        // 시작 시간순으로 재정렬
        tasks = tasks.OrderBy(i => i.StartTime).ToList();
       
        // 잠시 멈출 작업을 보관한다.
        Stack<Task> paused = new Stack<Task>();

        // 모든 작업에 대해 처리한다.
        for (int i = 0; i < tasks.Count; i++)
        {
            // 현재 작업을 가져온다.
            Task task = tasks[i];
            
            // 다음 작업
            Task next = null;
            
            // 그 다음작업이 존재하는 경우 그다음 작업을 가져온다.
            if (i + 1 < tasks.Count)
                next = tasks[i + 1];
            
            // 다음 작업이 존재 하지 않는경우
            if (next == null)
            {
                answer.Add(task.Name);
                break;
            }
            
            // (현재 작업 + 필요한 시간) 이 다음번 작업 시작 시간보다 커서 미뤄야 하는경우
            if (task.StartTimeMinute + task.PlayTime > next.StartTimeMinute)
            {
                task.PlayedTime += next.StartTimeMinute - task.StartTimeMinute;
                paused.Push(task);
            }
            // 처리가 가능한 작업인경우 
            else
            {
                // 정답에 등록
                answer.Add(task.Name);
                
                // 남은 작업이 존재 할경우 
                if (paused.Count > 0)
                {
                    // 현재 진행된 시간을 가져온다.
                    int currentTime = (task.StartTimeMinute + task.PlayTime);
                    
                    // 멈췄던 작업을 하나 꺼내고 
                    Task pausedTask = paused.Pop();
                    
                    // 현재시간 + 남은 시간 = 필요한 시간
                    int requiredTime = currentTime + (pausedTask.PlayTime - pausedTask.PlayedTime);
                    
                    // 필요시간이 다음번 시작 시간보다 큰경우
                    if (requiredTime > next.StartTimeMinute)
                    {
                        pausedTask.PlayedTime += (requiredTime - next.StartTimeMinute);
                        paused.Push(pausedTask);
                    }
                    // 시간내에 처리가능한 경우 
                    else
                    {
                        answer.Add(pausedTask.Name);
                    }
                }
            }
        }
        
        // 남은 작업이 있다면 순서대로 넣는다.
        foreach (Task remain in paused)
            answer.Add(remain.Name);
        
        return answer.ToArray();
    }

    /// <summary>
    /// 작업 클래스
    /// </summary>
    class Task
    {
        /// <summary>
        /// 작업명
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 시작시간
        /// </summary>
        public string StartTime { get; set; }
        public int StartTimeMinute { get; set; }

        /// <summary>
        /// 과제를 마치는데 걸리는 시간
        /// </summary>
        public int PlayTime { get; set; }
        
        /// <summary>
        /// 작업된 시간
        /// </summary>
        public int PlayedTime { get; set; }
    }
}