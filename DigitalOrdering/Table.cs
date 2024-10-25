namespace DigitalOrdering;

public class Table
{
    public int Capacity { get; set; }
    public int TableNumber { get; set; }
    public bool IsOccupied { get; set; }
    public Restaurant Restaurant { get; set; }
    
    public void OccupyTable()
    {
        IsOccupied = true;
    }
}