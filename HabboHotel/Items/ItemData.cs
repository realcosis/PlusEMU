using System;
using System.Collections.Generic;

using Plus.HabboHotel.Items.Wired;

namespace Plus.HabboHotel.Items
{
    public class ItemData
    {
        public int Id { get; set; }
        public int SpriteId { get; set; }
        public string ItemName { get; set; }
        public string PublicName { get; set; }
        public char Type { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public double Height { get; set; }
        public bool Stackable { get; set; }
        public bool Walkable { get; set; }
        public bool IsSeat { get; set; }
        public bool AllowEcotronRecycle { get; set; }
        public bool AllowTrade { get; set; }
        public bool AllowMarketplaceSell { get; set; }
        public bool AllowGift { get; set; }
        public bool AllowInventoryStack { get; set; }
        public InteractionType InteractionType { get; set; }
        public int BehaviourData { get; set; }
        public int Modes { get; set; }
        public List<int> VendingIds { get; set; }
        public List<double> AdjustableHeights { get; set; }
        public int EffectId { get; set; }
        public WiredBoxType WiredType { get; set; }
        public bool IsRare { get; set; }
        public bool ExtraRot { get; set; }

        public ItemData(int id, int sprite, string name, string publicName, string type, int width, int length, double height, bool stackable, bool walkable, bool isSeat,
            bool allowRecycle, bool allowTrade, bool allowMarketplaceSell, bool allowGift, bool allowInventoryStack, InteractionType interactionType, int behaviourData, int modes,
            string vendingIds, string adjustableHeights, int effectId, bool isRare, bool extraRot)
        {
            this.Id = id;
            SpriteId = sprite;
            ItemName = name;
            this.PublicName = publicName;
            this.Type = char.Parse(type);
            this.Width = width;
            this.Length = length;
            this.Height = height;
            this.Stackable = stackable;
            this.Walkable = walkable;
            this.IsSeat = isSeat;
            AllowEcotronRecycle = allowRecycle;
            this.AllowTrade = allowTrade;
            this.AllowMarketplaceSell = allowMarketplaceSell;
            this.AllowGift = allowGift;
            this.AllowInventoryStack = allowInventoryStack;
            this.InteractionType = interactionType;
            BehaviourData = behaviourData;
            this.Modes = modes;
            this.VendingIds = new List<int>();
            if (vendingIds.Contains(","))
            {
                foreach (var vendingId in vendingIds.Split(','))
                {
                    try
                    {
                        this.VendingIds.Add(int.Parse(vendingId));
                    }
                    catch
                    {
                        Console.WriteLine("Error with Item " + ItemName + " - Vending Ids");
                        continue;
                    }
                }
            }
            else if (!String.IsNullOrEmpty(vendingIds) && (int.Parse(vendingIds)) > 0)
                this.VendingIds.Add(int.Parse(vendingIds));

            this.AdjustableHeights = new List<double>();
            if (adjustableHeights.Contains(","))
            {
                foreach (var h in adjustableHeights.Split(','))
                {
                    this.AdjustableHeights.Add(double.Parse(h));
                }
            }
            else if (!String.IsNullOrEmpty(adjustableHeights) && (double.Parse(adjustableHeights)) > 0)
                this.AdjustableHeights.Add(double.Parse(adjustableHeights));

            this.EffectId = effectId;

            var wiredId = 0;
            if (this.InteractionType == InteractionType.WiredCondition || this.InteractionType == InteractionType.WiredTrigger || this.InteractionType == InteractionType.WiredEffect)
                wiredId = BehaviourData;

            WiredType = WiredBoxTypeUtility.FromWiredId(wiredId);

            this.IsRare = isRare;
            this.ExtraRot = extraRot;
        }
    }
}