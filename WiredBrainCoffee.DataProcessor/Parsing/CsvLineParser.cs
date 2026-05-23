using System.Globalization;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing;

public class CsvLineParser
{
    public const string ErrorMessageInvalidLine =
           "Line should have two parts separated with ';'.";

    public const string ErrorMessageInvalidDateTime =
        "Invalid date/time for request machine.";

    public static MachineDataItem[] Parse(string[] csvlines)
    {
        var machineDataItems = new List<MachineDataItem>();

        foreach (var csvLine in csvlines)
        {
            if (string.IsNullOrWhiteSpace(csvLine)) continue;

            var machineDataItem = Parse(csvLine);

            machineDataItems.Add(machineDataItem);
        }

        return machineDataItems.ToArray();
    }

    private static MachineDataItem Parse(string csvLine)
    {
        var lineItems = csvLine.Split(';');
        if (lineItems.Length != 2)
        {
            throw new Exception(ErrorMessageInvalidLine);
        }

        if(!DateTime.TryParse(lineItems[1],out DateTime dateTime))
        {
            throw new Exception(ErrorMessageInvalidDateTime);
        }

        return new MachineDataItem(lineItems[0], dateTime);
    }
}
