namespace Game2048.Models
{
    public class Tile
    {
        public int Value { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsNew { get; set; }
        public bool IsMerged { get; set; }

        public Tile(int row, int column, int value = 0)
        {
            Row = row;
            Column = column;
            Value = value;
            IsNew = value > 0;
            IsMerged = false;
        }

        public Tile Clone() => new Tile(Row, Column, Value)
        {
            IsNew = IsNew,
            IsMerged = IsMerged
        };
    }
}
