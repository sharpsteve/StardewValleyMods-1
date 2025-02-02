﻿using Microsoft.Xna.Framework;
using StardewModdingAPI;

namespace MapEdit
{
    public class ModConfig
    {
        public bool EnableMod { get; set; } = true;
        public bool UseSaveSpecificEdits { get; set; } = true;
        public bool IncludeGlobalEdits { get; set; } = true;
        public Color ExistsColor { get; set; } = new Color(1, 0, 0, 1f);
        public Color ActiveColor { get; set; } = new Color(0, 0, 1, 1f);
        public Color CopiedColor { get; set; } = new Color(0, 1, 0, 1f);
        public int BorderThickness { get; set; } = 4;
        public SButton ToggleButton { get; set; } = SButton.F10;
        public SButton RefreshButton { get; set; } = SButton.F5;
        public SButton CopyButton { get; set; } = SButton.MouseRight;
        public SButton PasteButton { get; set; } = SButton.MouseLeft;
        public SButton RevertButton { get; set; } = SButton.Delete;
        public SButton RevertModButton { get; set; } = SButton.LeftShift;
        public SButton ScrollUpButton { get; set; } = SButton.Up;
        public SButton ScrollDownButton { get; set; } = SButton.Down;
        public SButton LayerModButton { get; set; } = SButton.LeftShift;
        public SButton SheetModButton { get; set; } = SButton.LeftAlt;
        public string CopySound { get; set; } = "bigSelect";
        public string PasteSound { get; set; } = "hoeHit";
        public string ScrollSound { get; set; } = "toolSwap";
    }
}
