// Copyright (c) 2018 Trevor Redfern
// 
// This software is released under the MIT License.
// https://opensource.org/licenses/MIT

namespace SilverNeedle
{
    public interface IStatisticModifier
    {
        string StatisticName { get; }
        string StatisticType { get; }

    }
}