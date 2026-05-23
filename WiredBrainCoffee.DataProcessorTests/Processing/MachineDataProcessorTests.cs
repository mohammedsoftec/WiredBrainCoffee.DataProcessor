using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing;

public class MachineDataProcessorTests : IDisposable
{
    private readonly FakeCoffeeCountStore _fakeCoffeeCountStore;
    private readonly MachineDataProcessor _machineDataProcessor;
    public MachineDataProcessorTests()
    {

         _fakeCoffeeCountStore = new FakeCoffeeCountStore();
         _machineDataProcessor = new MachineDataProcessor(_fakeCoffeeCountStore);
    }
    [Fact]
    public void ShouldSaveCountPerCoffeType()
    {
        // Arrange
        var machineDateItems = new[]
        {
            new MachineDataItem("Espresso",new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Espresso",new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Latte",new DateTime(2022,10,27,10,0,0))
        };


        // Act
        _machineDataProcessor.ProcessItems(machineDateItems);

        // Assert
        Assert.Equal(2, _fakeCoffeeCountStore.SavedItems.Count);
        var item1 = _fakeCoffeeCountStore.SavedItems[0];
        Assert.Equal("Espresso", item1.CoffeeType);
        Assert.Equal(2, item1.Count);

        var item2 = _fakeCoffeeCountStore.SavedItems[1];
        Assert.Equal("Latte", item2.CoffeeType);
        Assert.Equal(1, item2.Count);
    }

    [Fact]
    public void ShouldIgnoreItemsThatAreNotNewer()
    {
        // Arrange
        var machineDateItems = new[]
        {
            new MachineDataItem("Espresso",new DateTime(2022,10,27,8,0,0)),
            new MachineDataItem("Espresso",new DateTime(2022,10,27,7,0,0)),
            new MachineDataItem("Espresso",new DateTime(2022,10,27,7,10,0)),
            new MachineDataItem("Espresso",new DateTime(2022,10,27,9,0,0)),
            new MachineDataItem("Latte",new DateTime(2022,10,27,10,0,0)),
            new MachineDataItem("Latte",new DateTime(2022,10,27,10,0,0))
        };


        // Act
        _machineDataProcessor.ProcessItems(machineDateItems);

        // Assert
        Assert.Equal(2, _fakeCoffeeCountStore.SavedItems.Count);
        var item1 = _fakeCoffeeCountStore.SavedItems[0];
        Assert.Equal("Espresso", item1.CoffeeType);
        Assert.Equal(2, item1.Count);

        var item2 = _fakeCoffeeCountStore.SavedItems[1];
        Assert.Equal("Latte", item2.CoffeeType);
        Assert.Equal(1, item2.Count);
    }


    [Fact]
    public void ShouldClearPreviousCoffeeCount()
    {
        // Arrange
        var machineDateItems = new[]
        {
            new MachineDataItem("Espresso",new DateTime(2022,10,27,8,0,0)),
        };


        // Act
        _machineDataProcessor.ProcessItems(machineDateItems);
        _machineDataProcessor.ProcessItems(machineDateItems);

        // Assert
        Assert.Equal(2, _fakeCoffeeCountStore.SavedItems.Count);

        foreach (var item in _fakeCoffeeCountStore.SavedItems)
        {
            Assert.Equal("Espresso", item.CoffeeType);
            Assert.Equal(1, item.Count);
        }
    }

    public void Dispose()
    {
        // this runs after every test, so we can clear the saved items to ensure that each test starts with an empty list
        _fakeCoffeeCountStore.SavedItems.Clear();
    }
}

public class FakeCoffeeCountStore : ICoffeeCountStore
{
    public List<CoffeeCountItem> SavedItems { get; set; } = new();

    public void Save(CoffeeCountItem coffeeCountItem)
    {
        SavedItems.Add(coffeeCountItem);
    }
}
