using FlowSynx.Plugins.Azure.Files.Extensions;

namespace FlowSynx.Plugins.Azure.Files.UnitTests.Extensions;

public class ByteExtensionsTests
{
    [Fact]
    public void ToHexString_NullInput_ReturnsEmptyString()
    {
        // Arrange
        byte[]? input = null;

        // Act
        var result = input.ToHexString();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToHexString_EmptyArray_ReturnsEmptyString()
    {
        // Arrange
        byte[] input = Array.Empty<byte>();

        // Act
        var result = input.ToHexString();

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToHexString_SingleByte_ReturnsCorrectHex()
    {
        // Arrange
        byte[] input = { 0xAB };

        // Act
        var result = input.ToHexString();

        // Assert
        Assert.Equal("AB", result);
    }

    [Fact]
    public void ToHexString_MultipleBytes_ReturnsCorrectHex()
    {
        // Arrange
        byte[] input = { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };

        // Act
        var result = input.ToHexString();

        // Assert
        Assert.Equal("0123456789ABCDEF", result);
    }

    [Fact]
    public void ToHexString_LowerValues_ReturnsCorrectHex()
    {
        // Arrange
        byte[] input = { 0x00, 0x0F, 0x10, 0x1A };

        // Act
        var result = input.ToHexString();

        // Assert
        Assert.Equal("000F101A", result);
    }
}