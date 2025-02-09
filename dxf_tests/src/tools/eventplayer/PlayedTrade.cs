﻿#region License

/*
Copyright (c) 2010-2021 Devexperts LLC

This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
*/

#endregion

using com.dxfeed.api;
using com.dxfeed.api.events;
using System;
using com.dxfeed.api.data;

namespace com.dxfeed.tests.tools.eventplayer
{
    /// <summary>
    ///     Trade event implementation for test event player.
    /// </summary>
    internal class PlayedTrade : IPlayedEvent<DxTestTrade>, IDxTrade
    {
        internal PlayedTrade(string symbol, long time, int sequence, int time_nanos, char exchange_code, double price,
            double size, int tick, double change, int dayId, double day_volume, double day_turnover, int raw_flags,
            Direction direction, bool is_eth, Scope scope)
        {
            this.EventSymbol = symbol;
            this.Time = Tools.UnixTimeToDate(time);
            this.Sequence = sequence;
            this.TimeNanoPart = time_nanos;
            this.ExchangeCode = exchange_code;
            this.Price = price;
            this.Size = size;
            this.Tick = tick;
            this.Change = change;
            this.DayId = dayId;
            this.DayVolume = day_volume;
            this.DayTurnover = day_turnover;
            this.RawFlags = raw_flags;
            this.TickDirection = direction;
            this.IsExtendedTradingHours = is_eth;
            this.Scope = scope;

            Params = new EventParams(0, 0, 0);
            Data = new DxTestTrade(time, sequence, time_nanos, exchange_code, price, size, tick, change, dayId,
                day_volume, day_turnover, raw_flags, direction, is_eth, scope);
        }

        /// <summary>
        ///     Creates Trade event from another object.
        /// </summary>
        /// <param name="trade">Other Trade object.</param>
        internal PlayedTrade(IDxTrade trade)
        {
            this.EventSymbol = trade.EventSymbol;
            this.Time = trade.Time;
            this.Sequence = trade.Sequence;
            this.TimeNanoPart = trade.TimeNanoPart;
            this.ExchangeCode = trade.ExchangeCode;
            this.Price = trade.Price;
            this.Size = trade.Size;
            this.Tick = trade.Tick;
            this.Change = trade.Change;
            this.DayId = trade.DayId;
            this.DayVolume = trade.DayVolume;
            this.DayTurnover = trade.DayTurnover;
            this.RawFlags = trade.RawFlags;
            this.TickDirection = trade.TickDirection;
            this.IsExtendedTradingHours = trade.IsExtendedTradingHours;
            this.Scope = trade.Scope;

            Params = new EventParams(0, 0, 0);
            Data = new DxTestTrade(Tools.DateToUnixTime(Time), Sequence, TimeNanoPart, ExchangeCode, Price, (int) Size,
                Tick, Change, DayId, DayVolume, DayTurnover, RawFlags, TickDirection, IsExtendedTradingHours, Scope);
        }

        public DxTestTrade Data { get; private set; }

        public EventParams Params { get; private set; }


        object IPlayedEvent.Data
        {
            get { return Data as object; }
        }

        public DateTime Time { get; private set; }

        public int Sequence { get; private set; }

        public int TimeNanoPart { get; private set; }

        public char ExchangeCode { get; private set; }

        public double Price { get; private set; }

        public double Size { get; private set; }

        public int Tick { get; private set; }

        public double Change { get; private set; }

        public int DayId { get; private set; }

        public double DayVolume { get; private set; }

        public double DayTurnover { get; private set; }

        public Direction TickDirection { get; private set; }

        public bool IsExtendedTradingHours { get; private set; }

        public int RawFlags { get; private set; }

        public Scope Scope { get; private set; }

        public string EventSymbol { get; private set; }

        object IDxEventType.EventSymbol
        {
            get { return EventSymbol; }
        }

        public object Clone()
        {
            return new PlayedTrade(this) as object;
        }
    }
}