using GitServer.Helpers;

namespace GitServer.Test;
public class AnotherHelperTest
{
    [Fact]
    public void SpikePath_3partPath_ShouldReturnCorrectList()
    {
        string input = "a/example/path";
        List<string> expected = new() 
        { 
            "a",
            "a/example",
            "a/example/path",
        };
        
        var actual = AnotherHelper.SpikePath(input);
        
        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }
}
