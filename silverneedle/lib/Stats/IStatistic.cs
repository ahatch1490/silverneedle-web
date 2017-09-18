// Copyright (c) 2017 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle
{
    public interface IStatistic
    {
        string Name { get; }
        void AddModifier(IStatModifier modifier);
        int TotalValue { get; }
    }
}