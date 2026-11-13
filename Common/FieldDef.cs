using System.Data;

namespace AttaChakiPOS.Common
{
    public enum FieldType { Text, Combo, Date, Number, CheckBox }

    /// <summary>Describes one input field on a master-data form (label, column, type, lookup source).</summary>
    public class FieldDef
    {
        public string ColumnName { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public FieldType Type { get; set; } = FieldType.Text;
        public bool Required { get; set; } = true;

        // For Combo fields: query that returns (Id, DisplayText)
        public string? LookupSql { get; set; }
        public string? LookupIdColumn { get; set; }
        public string? LookupTextColumn { get; set; }

        public Control BoundControl { get; set; } = null!;

        public static FieldDef Text(string col, string label, bool required = true) =>
            new FieldDef { ColumnName = col, Label = label, Type = FieldType.Text, Required = required };

        public static FieldDef Number(string col, string label, bool required = true) =>
            new FieldDef { ColumnName = col, Label = label, Type = FieldType.Number, Required = required };

        public static FieldDef Combo(string col, string label, string lookupSql, string idCol, string textCol, bool required = true) =>
            new FieldDef { ColumnName = col, Label = label, Type = FieldType.Combo, LookupSql = lookupSql, LookupIdColumn = idCol, LookupTextColumn = textCol, Required = required };

        public static FieldDef Date(string col, string label, bool required = true) =>
            new FieldDef { ColumnName = col, Label = label, Type = FieldType.Date, Required = required };
    }
}
