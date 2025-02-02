using System.ComponentModel;
using FluentAssertions;
using pkNX.Structures.FlatBuffers;
using pkNX.Structures.FlatBuffers.Arceus;

namespace pkNX.Tests.SourceGen;

public class SayHelloGeneratorTest
{
    [Fact]
    public void IsAttributeInserted()
    {
        var converter = TypeDescriptor.GetConverter(typeof(Vec2f));
        converter.Should().BeOfType<ExpandableObjectConverter>("Attribute should be inserted.");
    }

    [Fact]
    public void IsAttributeInserted2()
    {
        var converter = TypeDescriptor.GetConverter(typeof(PlacementParameters));
        converter.Should().BeOfType<ExpandableObjectConverter>("Attribute should be inserted.");
    }
}
