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
        
        // 행과 열의 수를 가져온다.
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

        // 시작 시간순으로 재 정렬
        tasks = tasks.OrderBy(i => i.StartTime).ToList();
        
        // 큐에 시간순서대로 모든 작업을 바인딩한다
        Queue<Task> process = new Queue<Task>();
        tasks.ForEach(i => process.Enqueue(i));

        Task task = null;
        Task nextTask = null;
        
        // 모든 작업이 완료될때 까지 반복한다.
        while (process.Count > 0)
        {
            // 작업을 꺼내온다.
            if(task == null)
                task = process.Dequeue();
            
            // 그 다음 해야할 작업을 가져온다.
            if(nextTask == null)
                nextTask = process.Dequeue();
            
            // 완료하는데 필요한 시간이 다음 실행 시간보다 부족하다면 
            if ((task.StartTimeMinute + task.PlayTime) < nextTask.StartTimeMinute)
            {
                // 작업 정보를 업데이트 한다.
                task.PlayedTime += nextTask.StartTimeMinute - (task.StartTimeMinute + task.PlayTime);
                task.StartTimeMinute += task.PlayedTime;
                
                // 뒤에서 다시 처리 할 수 있도록 다시 큐에 넣는다.
                process.Enqueue(task);
            }
            
            // 진행해야할 다음 작업이 남아있지 않은경우 
            if (process.Count == 0)
            {
                answer.Add(task.Name);
                answer.Add(nextTask.Name);
                break;
            }
            
            // 완료된 작업을 등록한다.
            answer.Add(task.Name);
            
            // 진행 큐 정보 교체
            task = nextTask;
            nextTask = null;
        }
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