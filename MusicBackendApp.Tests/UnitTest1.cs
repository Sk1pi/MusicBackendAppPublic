namespace MusicBackendApp.Tests;

public class UnitTest1
{
    [Fact]
    public void PipelineTest_ShouldAlwaysPass()
    {
        int expected = 2;
        int actual = 2;
        
        Assert.Equal(expected, actual);
    }
}
