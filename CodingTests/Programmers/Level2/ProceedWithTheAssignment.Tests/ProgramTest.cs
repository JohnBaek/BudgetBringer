using FluentAssertions;
using JetBrains.Annotations;
using ProceedWithTheAssignment;
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
        Program program = new Program();

        // 첫번째 테스트
        program.Solution(new [,] {{"korean", "11:40", "30"}, {"english", "12:10", "20"}, {"math", "12:30", "40"}})
            .Should().BeEquivalentTo(new[] {"korean", "english", "math"});
    }
}