using System.ComponentModel;
using FluentAssertions;
using pkNX.Structures.FlatBuffers;

namespace pkNX.Tests.SourceGen;

public class SayHelloGeneratorTest
{
    [Fact]
    public void IsAttributeInserted()
    {
        var converter = TypeDescriptor.GetConverter(typeof(Vec2f));
        converter.Should().BeOfType<ExpandableObjectConverter>("Attribute should be inserted.");
    }
}
