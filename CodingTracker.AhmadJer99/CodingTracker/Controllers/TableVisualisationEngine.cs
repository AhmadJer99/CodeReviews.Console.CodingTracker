using ConsoleTableExt;
using CodingTracker.Models;
namespace CodingTracker.Controllers;
internal class TableVisualisationEngine
{
    public static void ViewAsTable(List<CodingSession> codingSessions, TableAligntment tableAligntment, List<string> columnNames)
    {

        ConsoleTableBuilder.From(codingSessions)
            .WithColumn(columnNames)
                   .WithCharMapDefinition(
                       CharMapDefinition.FramePipDefinition,
                       new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                       })
                   .ExportAndWriteLine(tableAligntment);
    }

}

