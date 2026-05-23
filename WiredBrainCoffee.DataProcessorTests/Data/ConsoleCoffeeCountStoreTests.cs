using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Data;

public class ConsoleCoffeeCountStoreTests
{
    [Fact]
    public void ShouldWriteOutputToConsole()
    {
        // Arrange
        var coffeeCountItem = new CoffeeCountItem("Espresso", 2);
        var stringWriter = new StringWriter();
        var consoleCoffeeCountStore = new ConsoleCoffeeCountStore(stringWriter);

        // Act
        consoleCoffeeCountStore.Save(coffeeCountItem);

        // Assert
        var output = stringWriter.ToString();
        Assert.Contains($"{coffeeCountItem.CoffeeType}:{coffeeCountItem.Count}", output);
    }
}
