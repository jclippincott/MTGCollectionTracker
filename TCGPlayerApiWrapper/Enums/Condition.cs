using System.ComponentModel;

namespace TCGPlayerApiWrapper.Enums;

public enum Condition {
    [Description("Near Mint")] Nm = 1,
    [Description("Lightly Played")] Lp = 2,
    [Description("Moderately Played")] Mp = 3,
    [Description("Heavily Played")] Hp = 4,
    [Description("Damaged")] Dm = 5,
    [Description("Unopened")] U = 6
}