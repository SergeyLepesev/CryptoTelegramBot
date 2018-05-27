using System;

namespace CryptoBot.Models
{
    public enum TypeNotice
    {
        Volume,
        Percent,
        Cost
    }
    public class Notice
    {
        public Notice(long uerdId, string name, TypeNotice type)
        {
            UserId = uerdId;
            Name = name;
            Type = type;
        }

        public long UserId { get; }
        public string Name { get; }
        public TypeNotice Type { get; }
        public DateTime Time { get; } = DateTime.UtcNow;
    }
}