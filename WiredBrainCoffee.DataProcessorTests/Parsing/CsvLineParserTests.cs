namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParserTests
{
    [Fact]
    public void ShouldParseValidLine()
    {
        // Arrange 
        string[] csvLines = new string[] { "Cappuccino;10/27/2022 8:06:04 AM" };

        // Act
        var machineDataItems = CsvLineParser.Parse(csvLines);

        // Assert
        Assert.NotNull(machineDataItems);
        Assert.Single(machineDataItems);
        Assert.Equal("Cappuccino", machineDataItems[0].CoffeeType);
        Assert.Equal(new DateTime(2022, 10, 27, 8, 6, 4), machineDataItems[0].CreatedAt);
    }

    [Fact]
    public void ShouldSkippedEmptyLines()
    {
        // Arrange 
        string[] csvLines = new string[] { "", " " };

        // Act
        var machineDataItems = CsvLineParser.Parse(csvLines);


        // Assert
        Assert.NotNull(machineDataItems);
        Assert.Empty(machineDataItems);

    }

    [InlineData("Cappuccino", CsvLineParser.ErrorMessageInvalidLine)]
    [InlineData("Cappuccino;InvalidDateTime", CsvLineParser.ErrorMessageInvalidDateTime)]
    [Theory]
    public void ShouldThrowExceptionForInvalidLine(string csvLine, string expectedExceptionMessage)
    {
        // Arrange 
        var csvLines = new string[] { csvLine };

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => CsvLineParser.Parse(csvLines));

        Assert.Equal(expectedExceptionMessage, exception.Message);

    }
}
