using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace ProceedWithTheAssignment.Tests;

/// <summary>
/// 과제 진행하기 테스트 클래스
/// </summary>
[TestSubject(typeof(Program))]
public class ProgramTest
{
    /// <summary>
    /// 전체 테스트
    /// </summary>
    [Fact]
    public void Tests()
    {
        // 실행할 인스턴스를 만든다.
        ProceedWithTheAssignment.Program program = new ProceedWithTheAssignment.Program();
        
        // 테스트를 진행한다.
        // program.Solution(new [,] {{"korean", "11:40", "30"}, {"english", "12:10", "20"}, {"math", "12:30", "40"}}).Should().BeEquivalentTo(new[] {"korean", "english", "math"});
        // program.Solution(new [,] {{"science", "12:40", "50"}, {"music", "12:20", "40"}, {"history", "14:00", "30"}, {"computer", "12:30", "100"}}).Should().BeEquivalentTo(new[] {"science", "history", "computer", "music"});
        // program.Solution(new [,] {{"aaa", "12:00", "20"}, {"bbb", "12:10", "30"}, {"ccc", "12:40", "10"}}).Should().BeEquivalentTo(new[] {"bbb", "ccc", "aaa"});
        // program.Solution(new [,] {{"a","09:00","30"},{"b","09:10","20"},{"c","09:15","20"},{"d","09:55","10"},{"e","10:50","5"}}).Should().BeEquivalentTo(new[]  {"c","b","d","a","e"});
        // program.Solution(new [,] {{"a", "09:00", "30"}, {"b", "09:20", "10"}, {"c", "09:40", "10"}}).Should().BeEquivalentTo(new[]  {"b", "a", "c"});
        
        // 다른 코드 테스트
        ProceedWithTheAssignment_2.Program program2 = new ProceedWithTheAssignment_2.Program();
        program2.Solution(new [,] {{"science", "12:40", "50"}, {"music", "12:20", "40"}, {"history", "14:00", "30"}, {"computer", "12:30", "100"}}).Should().BeEquivalentTo(new[] {"science", "history", "computer", "music"});
    }
}